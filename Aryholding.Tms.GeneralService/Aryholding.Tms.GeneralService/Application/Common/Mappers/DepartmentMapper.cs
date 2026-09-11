using Aryholding.Tms.GeneralService.Application.DTOs;
using Aryholding.Tms.GeneralService.Domain.Entities;

namespace Aryholding.Tms.GeneralService.Application.Common.Mappers
{
    public static class DepartmentMapper
    {
        public static DepartmentResponseDTO MapToResponseDto(Department department)
        {
            return new DepartmentResponseDTO
            {
                Code = department.DepartmentCode,
                Name = department.DepartmentName
            };
        }

        public static IEnumerable<DepartmentResponseDTO> MapToResponseDtos(IEnumerable<Department> departments)
        {
            return departments.Select(MapToResponseDto);
        }
    }
}
