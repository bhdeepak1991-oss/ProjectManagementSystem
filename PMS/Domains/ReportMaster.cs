using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Domains
{

    [Table("ReportMaster")]
    public class ReportMaster
    {
        public int Id { get; set; }
        public string ReportName { get; set; }= string.Empty;
        public string ReportQuery { get; set; }= string.Empty;
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string ReportDisplayName { get; set; }
    }
}
