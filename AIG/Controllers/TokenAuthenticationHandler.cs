using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AIG.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace AIG.Controllers
{
    public class TokenAuthenticationHandler : AuthenticationHandler<JwtBearerOptions>
    {

        public IConfiguration Configuration { get; }

        public TokenAuthenticationHandler(IOptionsMonitor<JwtBearerOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration configuration)
                : base(options, logger, encoder, clock)
        {
            Configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = null;
            string authorization = Request.Headers[AigAuthConstants.AuthHeader];

            if (string.IsNullOrEmpty(authorization))
            {
                return AuthenticateResult.Fail(AigAuthConstants.MissingAuthHeader);
            }

            if (authorization.StartsWith(AigAuthConstants.Bearer, StringComparison.OrdinalIgnoreCase))
            {
                token = authorization.Substring(AigAuthConstants.Bearer.Length).Trim();
            }

            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail(AigAuthConstants.MissingBearerToken);
            }
            return await ValidateJwtToken(token);
        }

        private async Task<AuthenticateResult> ValidateJwtToken(string token)
        {
            List<Exception> validationFailures = null;
            SecurityToken validatedToken;
            foreach (var validator in Options.SecurityTokenValidators)
            {
                if (validator.CanReadToken(token))
                {
                    ClaimsPrincipal principal;
                    try
                    {
                        principal = validator.ValidateToken(token, await GetValidationParamsAsync(Options), out validatedToken);
                        ValidateClaims(principal);
                    }
                    catch (Exception ex)
                    {
                        validationFailures = validationFailures ?? new List<Exception>(1);
                        validationFailures.Add(ex);
                        continue;
                    }
                    var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), this.Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
            }
            if (validationFailures != null)
            {
                var authenticationFailedContext = new AuthenticationFailedContext(Context, this.Scheme, Options)
                {
                    Exception = (validationFailures.Count == 1) ? validationFailures[0] : new AggregateException(validationFailures)
                };

                return AuthenticateResult.Fail(authenticationFailedContext.Exception);
            }

            return AuthenticateResult.Fail(AigAuthConstants.NoSecurityTokenValidator);
        }

        private void ValidateClaims(ClaimsPrincipal principal)
        {
            var clientIds = new List<string> { "31148fa5-8fb2-49d0-8ded-8f1cf01022a4" };//TODO: Get the allowed clientids from storage
            var scopeClaim = principal?.Claims.FirstOrDefault(c => c.Type == AigAuthConstants.ClientIdScope && clientIds.Contains(c.Value));
            if (scopeClaim == null)
            {
                throw new Exception(AigAuthConstants.InValidClient);
            }
        }

        private async Task<TokenValidationParameters> GetValidationParamsAsync(JwtBearerOptions Options)
        {
            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(AigAuthConstants.StsDiscoveryEndpoint, new OpenIdConnectConfigurationRetriever());
            OpenIdConnectConfiguration config = await configManager.GetConfigurationAsync();

            var validationParameters = Options.TokenValidationParameters.Clone();
            validationParameters.ValidateAudience = true;
            validationParameters.ValidAudience = Configuration.GetValue<string>("AadAuthorization:ClientId");
            validationParameters.IssuerSigningKeys = config.SigningKeys;
            validationParameters.ValidateLifetime = true;
            validationParameters.ValidateIssuer = true;
            validationParameters.ValidIssuer = $"https://login.microsoftonline.com/{Configuration.GetValue<string>("AadAuthorization:TenantId")}/v2.0";

            return validationParameters;
        }
    }
}
