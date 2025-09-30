namespace PMS.Constants
{
    public static class ViewHelper
    {
        public static string GetPageViewName(string featureName, string viewName)
        {
            return $"Features/{featureName}/Views/{viewName}.cshtml";
        }
    }
}
