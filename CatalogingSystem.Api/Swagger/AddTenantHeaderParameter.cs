using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cataloging.Api.Swagger; 
public class AddTenantHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Exclude endpoints from TenantsController
        if (context.ApiDescription.ActionDescriptor.EndpointMetadata
            .Any(em => em is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor descriptor &&
                       descriptor.ControllerName == "Tenants"))
        {
            return;
        }

        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "tenant",
            In = ParameterLocation.Header,
            Description = "Tenant identifier (e.g., tenant_BO-USFX-001)",
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "string"
            }
        });
    }
}