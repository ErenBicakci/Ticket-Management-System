namespace Aryholding.Tms.GeneralService.Application.Commands.TicketCategoryManagement.CreateTicketCategory
{
    public class CreateTicketCategoryRequestDto
    {
        public string CategoryName { get; set; } = default!;
        public string CategoryCode { get; set; } = default!;
        public string DepartmentCode { get; set; } = default!;
    }
}
