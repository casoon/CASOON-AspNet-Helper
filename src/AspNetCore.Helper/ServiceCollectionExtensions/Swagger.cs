using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", builder.Configuration.GetSection("OpenApiInfo").Get<OpenApiInfo>());
            c.AddSecurityDefinition("oauth2",
                new OpenApiSecurityScheme
                {
                    Description = "OAuth2.0 Auth Code with PKCE",
                    Name = "oauth2",
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(builder.Configuration["OpenApiOAuthFlow:AuthorizationUrl"]),
                            TokenUrl = new Uri(builder.Configuration["OpenApiOAuthFlow:TokenUrl"]),
                            RefreshUrl = null,
                            Scopes = new Dictionary<string, string>
                            {
                                {
                                    builder.Configuration["OpenApiOAuthFlow:ApiScope"], "read the api"
                                }
                            },
                            Extensions = null
                        }
                    }
                });
            var requirement = new OpenApiSecurityRequirement();
            requirement.Add(new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}
            }, new[] {builder.Configuration["OpenApiOAuthFlow:ApiScope"]});
            c.AddSecurityRequirement(requirement);
        });

        return builder;
    }
}