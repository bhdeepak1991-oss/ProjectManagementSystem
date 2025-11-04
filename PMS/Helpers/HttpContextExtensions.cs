namespace PMS.Helpers
{
    public static class HttpContextExtensions
    {
        public static int? GetUserId(this HttpContext httpContext) => httpContext?.Session?.GetInt32("userId");
        public static int? GetEmployeeId(this HttpContext httpContext) => httpContext?.Session?.GetInt32("employeeId");
        public static int? GetProjectId(this HttpContext httpContext) => httpContext?.Session?.GetInt32("selectedProjectId");
        public static int? GetRoleId(this HttpContext httpContext) => httpContext?.Session?.GetInt32("roleId");

    }
}
