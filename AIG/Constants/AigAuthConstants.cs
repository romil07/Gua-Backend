namespace AIG.Constants
{
    public static class AigAuthConstants
    {
        public const string TokenAuthenticationDefaultScheme = "TokenAuthenticationScheme";
        public const string AuthHeader = "Authorization";
        public const string Bearer = "Bearer ";
        public const string MissingAuthHeader = "Missing authorization header in the request.";
        public const string MissingBearerToken = "Missing bearer token.";
        public const string InValidBearerToken = "Invalid bearer token.";
        public const string StsDiscoveryEndpoint = "https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration";
        public const string NoSecurityTokenValidator = "No SecurityTokenValidator available.";
        public const string ClientIdScope = "azp";
        public const string InValidClient = "Client is not registered with AIG.";
    }
}
