using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Linq;

namespace TweetbookAPI.Infrastructure
{
    public class RoutePrefixConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _routePrefix;

        public RoutePrefixConvention(IRouteTemplateProvider route)
        {
            _routePrefix = new AttributeRouteModel(route);
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var selector in application.Controllers.SelectMany(c => c.Selectors))
            {
                selector.AttributeRouteModel = selector.AttributeRouteModel != null ? AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel) : _routePrefix;
            }
        }
    }
}
