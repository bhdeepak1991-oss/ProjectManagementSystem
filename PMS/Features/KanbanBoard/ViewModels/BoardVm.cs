namespace PMS.Features.KanbanBoard.ViewModels
{
    public class BoardVm
    {
        public string TaskType { get; set; }
        public string TaskCode{ get; set; }
        public string TaskName { get; set; }
        public string Priority { get; set; }
        public  string AssignedTo { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string ModuleName { get; set; }
        public string SprintName { get; set; }
        public string TaskStatus { get; set; }
        public int Id { get; set; }
        public int Sequence { get; set; }
    }
}
