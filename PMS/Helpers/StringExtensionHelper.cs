namespace PMS.Helpers
{
    public static class StringExtensionHelper
    {
        public static string TruncateWithEllipsis(this string? value, int maxLength=20)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
                return value ?? string.Empty;

            return value.Substring(0, maxLength) + "...";
        }
    }
}
