using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo()
                {
                    Description = "",
                    Title = "",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Name = "", Url = new Uri("")
                    }
                });
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
                            AuthorizationUrl = new Uri(builder.Configuration["AuthorizationUrl"]),
                            TokenUrl = new Uri(builder.Configuration["TokenUrl"]),
                            RefreshUrl = null,
                            Scopes = new Dictionary<string, string>
                            {
                                {
                                    builder.Configuration["ApiScope"], "read the api"
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
            }, new[] {builder.Configuration["ApiScope"]});
            c.AddSecurityRequirement(requirement);
        });

        return builder;
    }
}