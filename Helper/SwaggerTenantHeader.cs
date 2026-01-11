using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerTenantHeader : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        if (context.ApiDescription.RelativePath != null &&
      context.ApiDescription.RelativePath.StartsWith("api/tenants", StringComparison.OrdinalIgnoreCase))
            return;

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-Tenant-Id",
            In = ParameterLocation.Header,
            Required = true, // Swagger will show it as required
            Description = "Tenant Id GUID"
        });
    }
}
