using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace CompanySys.API;

public sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider)
    : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();

        if (authenticationSchemes.Any(authScheme => authScheme.Name == JwtBearerDefaults.AuthenticationScheme))
        {
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

            if (!document.Components.SecuritySchemes.ContainsKey("Bearer"))
            {
                document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT Bearer token"
                };
            }

            var bearerReference = new OpenApiSecuritySchemeReference("Bearer", document);

            var requirement = new OpenApiSecurityRequirement
            {
                [bearerReference] = new List<string>()
            };

            if (document.Paths is not null)
            {
                foreach (var path in document.Paths.Values)
                {
                    if (path?.Operations is null) continue;

                    foreach (var operation in path.Operations.Values)
                    {
                        if (operation is null) continue;

                        operation.Security ??= new List<OpenApiSecurityRequirement>();
                        operation.Security.Add(requirement);
                    }
                }
            }
        }
    }
}