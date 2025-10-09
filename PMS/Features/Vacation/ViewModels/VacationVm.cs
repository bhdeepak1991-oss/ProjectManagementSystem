namespace PMS.Features.Vacation.ViewModels
{
    public class VacationVm
    {
        public int Id { get; set; }
        public string CreatedByName { get; set; }
        public string? VacationType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string VacationName { get; set; } = string.Empty;
        public string VacationDetailText { get; set; } = string.Empty;
        public string ProjectInvolved { get; set; } = string.Empty;
    }
}
