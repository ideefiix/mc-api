using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Authorization;

public class HasClaim : TypeFilterAttribute
{
    public HasClaim(string claimType, string resourceIdentifierName) : base(typeof(AuthorizeCustomTokenFilter))
    {
        Arguments = new object[] { claimType, resourceIdentifierName};
    }

    public class AuthorizeCustomTokenFilter : IAuthorizationFilter
    {
        private readonly string _claimType;
        private readonly string _resourceIdentifierName;

        public AuthorizeCustomTokenFilter(string claimType, string resourceIdentifierName)
        {
            _claimType = claimType;
            _resourceIdentifierName = resourceIdentifierName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            
            if (!context.HttpContext.Request.RouteValues.TryGetValue(_resourceIdentifierName, out var resourceId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            
            var clientToken = authorizationHeader.ToString().TrimStart("Bearer".ToCharArray());
            clientToken = clientToken.Trim();

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(clientToken);
            var claimInToken = jwt.Claims.FirstOrDefault(c => c.Type == _claimType);

            if (claimInToken == null || resourceId == null || claimInToken.Value != resourceId.ToString())
            {
                context.Result = new UnauthorizedResult();
                return;
            }

        }
    }
}