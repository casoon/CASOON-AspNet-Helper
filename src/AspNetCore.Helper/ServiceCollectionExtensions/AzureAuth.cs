using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCore.Helper.ServiceCollectionExtensions {
    
    public static partial class ServiceCollectionExtensions
    {
        public static WebApplicationBuilder AddAzureAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
            return builder;
        }
        public static WebApplicationBuilder AddAzureAuthorization(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
            return builder;
        }

        public static WebApplicationBuilder AddAzureCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options => options.AddPolicy("allowAny", o => o.AllowAnyOrigin()));
            return builder;
        }
    }
}
