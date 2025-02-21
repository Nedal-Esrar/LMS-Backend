using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MLMS.API.Common.Conventions;

public class RoutePrefixConvention(string prefix) : IApplicationModelConvention
{
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            foreach (var selector in controller.Selectors)
            {
                if (selector.AttributeRouteModel is null)
                {
                    continue;
                }
                
                selector.AttributeRouteModel.Template = $"{prefix}/{selector.AttributeRouteModel.Template}";
            }
        }
    }
}