namespace PMS.Helpers
{
    public static class HttpContextExtensions
    {
        public static int? GetUserId(this HttpContext httpContext)
        {
            return httpContext?.Session?.GetInt32("userId");
        }

        public static int? GetEmployeeId(this HttpContext httpContext)
        {
            return httpContext?.Session?.GetInt32("employeeId");
        }

        public static int? GetProjectId(this HttpContext httpContext)
        {
            return httpContext?.Session?.GetInt32("selectedProjectId");
        }
    }
}
