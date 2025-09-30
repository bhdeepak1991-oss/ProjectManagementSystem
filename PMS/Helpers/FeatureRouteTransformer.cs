using Microsoft.AspNetCore.Mvc.Routing;

namespace PMS.Helpers
{
    public class FeatureRouteTransformer : DynamicRouteValueTransformer
    {
        public override ValueTask<RouteValueDictionary?> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            if (values["featureName"] is string feature)
            {
                // Map featureName directly to controller
                values["controller"] = feature;
            }

            return new ValueTask<RouteValueDictionary?>(values);
        }
    }
}
