using LE.AdminService.Constants;
using LE.AdminService.JwtHelpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LE.AdminService.Extensions
{
    public static class AuthorizationExtensions
    {
        private const string Authorization = "Authorization";
        private static string GetAuthorizationValue(IHeaderDictionary Headers)
        {
            var key = Authorization;
            if (!Headers.ContainsKey(key))
                return null;
            if (!Headers.TryGetValue(key, out var Value))
                return null;
            if (Value.Count < 1)
                return null;
            var result = Value.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(result))
                return null;
            return result;
        }

        private static void ModifyPayloadJwt(IHeaderDictionary Headers, IConfiguration Configuration)
        {
            var token = GetAuthorizationValue(Headers);
            if (string.IsNullOrWhiteSpace(token))
                return;
            var newToken = string.Empty;
            var accessToken = token.Split(" ")[1];
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(accessToken) as JwtSecurityToken;

            var claimRole = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimConstant.TYPE && 
                                                                    (claim.Value == ClaimConstant.CUSTOMER || claim.Value == ClaimConstant.ADMIN));
            var tokenBuilder = new JwtTokenBuilder()
                                   .AddSecurityKey(JwtSecurityKey.Create(Configuration.GetValue<string>("JwtSettings:Secret")))
                                   .AddClaims(jwtToken.Claims.ToList());
            switch (claimRole.Value)
            {
                case UserRole.CUSTOMER:
                    tokenBuilder.AddRole(UserRole.CUSTOMER);
                    break;
                case UserRole.ADMIN:
                    tokenBuilder.AddRole(UserRole.ADMIN);
                    break;
            }

            Headers[Authorization] = $"{JwtBearerDefaults.AuthenticationScheme} {tokenBuilder.Build().Value}";
        }
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, IConfiguration Configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Env.SECRET_KEY))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var authorizationValue = GetAuthorizationValue(context.HttpContext.Request.Headers);
                        var isNotContainBearer = authorizationValue != null && !authorizationValue.Contains("Bearer");
                        if (isNotContainBearer)
                        {
                            var authorization = string.Format("{0} {1}", JwtBearerDefaults.AuthenticationScheme, authorizationValue);
                            context.HttpContext.Request.Headers.Remove("Authorization");
                            context.HttpContext.Request.Headers.Add("Authorization", authorization);
                        }
                        if (string.IsNullOrEmpty(accessToken) || GetAuthorizationValue(context.HttpContext.Request.Headers) != null)
                            return Task.CompletedTask;

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        ModifyPayloadJwt(context.HttpContext.Request.Headers, Configuration);
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        context.NoResult();
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.ContentType = "text/plain";
                        return context.Response.WriteAsync(context.Exception.ToString());
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(UserRole.CUSTOMER);
                });
            });
            return services;
        }
        public static IApplicationBuilder UseCustomAuthorization(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            app.Use((ctx, next) =>
            {
                if (ctx.Response.StatusCode != (int)HttpStatusCode.OK)
                    return Task.CompletedTask;
                if (!ctx.User.Identity.IsAuthenticated && GetAuthorizationValue(ctx.Request.Headers) != null)
                {
                    ctx.Response.Clear();
                    ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                }
                return next.Invoke();
            });
            return app;
        }
    }
}
