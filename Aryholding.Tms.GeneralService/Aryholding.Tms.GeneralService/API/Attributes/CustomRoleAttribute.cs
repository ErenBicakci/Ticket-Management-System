using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Aryholding.Tms.GeneralService.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _requiredDepartmentCode;
        private readonly int _minimumPriority;
        private readonly bool _requireAuthentication;

        public CustomRoleAttribute(string departmentCode = "", int minimumPriority = -1, bool requireAuthentication = true)
        {
            _requiredDepartmentCode = departmentCode ?? "";
            _minimumPriority = minimumPriority;
            _requireAuthentication = requireAuthentication;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (_requireAuthentication && !(user?.Identity?.IsAuthenticated ?? false))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!_requireAuthentication && !(user?.Identity?.IsAuthenticated ?? false))
                return;

            var claims = user!.Claims;

            var isSuperUser = claims.FirstOrDefault(c => c.Type == "superUser")?.Value;
            if (bool.TryParse(isSuperUser, out var super) && super)
                return;

            if (!string.IsNullOrWhiteSpace(_requiredDepartmentCode))
            {
                var userDeptCodes = claims
                    .Where(c => c.Type == "departmentCode")
                    .Select(c => c.Value)
                    .ToList();

                var deptOk = userDeptCodes.Any(dc =>
                    string.Equals(dc, _requiredDepartmentCode, StringComparison.OrdinalIgnoreCase));

                if (!deptOk)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }

            if (_minimumPriority > 0)
            {
                var userPriorities = claims
                    .Where(c => c.Type == "priority")
                    .Select(c => c.Value)
                    .Select(v => int.TryParse(v, out var p) ? p : (int?)null)
                    .Where(p => p.HasValue)
                    .Select(p => p!.Value)
                    .ToList();

                var prioOk = userPriorities.Any(p => p >= _minimumPriority);

                if (!prioOk)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }
    }
}
