using Microsoft.AspNetCore.Mvc.Razor;

namespace PMS.Helpers
{
    public class FeatureViewLocationExpander: IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context) { }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            // Add custom location patterns
            return new[]
            {
            "/Features/{1}/Views/{0}.cshtml",   // {1} = Controller, {0} = View name
            "/Features/Shared/{0}.cshtml"
        }
            .Concat(viewLocations); // keep default locations
        }
    }
}
