namespace PMS.Features.Dashboard.ViewModels
{
    public class DrilldownSeries
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<object> data { get; set; }
        public string mappingName { get; set; }
    }
}
