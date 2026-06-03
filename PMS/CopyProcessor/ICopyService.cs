namespace PMS.CopyProcessor
{
    public interface ICopyService
    {
        Task<bool> CopyProjectService(int projectId, string projectName);
    }
}
