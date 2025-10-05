using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Domains
{

    [Table("Sprints", Schema ="dbo")]
    public class Sprint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(300)]
        public string SprintName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int DurationDays { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        public string SprintGoal { get; set; }

        public string BusinessGoal { get; set; }

        public DateTime? DemoDate { get; set; }

        public bool IsActive { get; set; }

        public int ProjectId { get; set; }
        public bool IsDeleted { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int SprintStatus { get; set; }

    }
}
