using Common_Shared.CommonModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Models.Account;
using System.Security.Claims;
using System.Web;

namespace Common_Shared.Accessor
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAccessor(IHttpContextAccessor httpContextAccessor
                            , IHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public LoggingUser GetUserName()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return new LoggingUser
                {
                    IpAddress = GetIpAddress()
                };
            }
            else
            {
                var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                if (claimsIdentity != null && claimsIdentity.FindFirst("userId") != null)
                {
                    return new LoggingUser()
                    {
                        IpAddress = GetIpAddress(),
                        UserId = claimsIdentity.FindFirst("userId").Value,
                        UserName = claimsIdentity.FindFirst(ClaimTypes.Name).Value
                    };
                }
                else
                {
                    return new LoggingUser
                    {
                        IpAddress = GetIpAddress(),
                        UserName = _httpContextAccessor.HttpContext?.Request?.Cookies["X-Username"]?.ToString()
                    };
                }

            }

        }

        public string GetIpAddress()
        {
            var ip = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = _httpContextAccessor.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }

            return ip;
        }

        public string GetUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var claimsIdentity = httpContext.User?.Identity as ClaimsIdentity;
                if (claimsIdentity != null && claimsIdentity.FindFirst("userId") != null)
                {
                    return claimsIdentity.FindFirst("userId").Value;
                }
            }
            return string.Empty;
        }
        public void SetAuthCookiesInClient(TokenModel tokenModel, string userName)
        {
            var expirytimeAccess = DateTimeOffset.UtcNow.AddSeconds(5000);
            var expirytimeRefresh = DateTimeOffset.UtcNow.AddSeconds(5000);

            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Access-Token", HttpUtility.UrlEncode(tokenModel.AccessToken),
                                                                                              new CookieOptions()
                                                                                              {
                                                                                                  HttpOnly = true,
                                                                                                  SameSite = SameSiteMode.Strict,
                                                                                                  Secure = true,
                                                                                                  Expires = expirytimeAccess
                                                                                              });

            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Access-Token-ExpiryInSeconds", tokenModel.AccessTokenExpiryInSeconds.ToString(),
                                                                                                    new CookieOptions()
                                                                                                    {
                                                                                                        HttpOnly = true,
                                                                                                        SameSite = SameSiteMode.Strict,
                                                                                                        Secure = true,
                                                                                                        Expires = expirytimeAccess
                                                                                                    });

            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Refresh-Token", HttpUtility.UrlEncode(tokenModel.RefreshToken),
                                                                                             new CookieOptions()
                                                                                             {
                                                                                                 HttpOnly = true,
                                                                                                 SameSite = SameSiteMode.Strict,
                                                                                                 Secure = true,
                                                                                                 Expires = expirytimeRefresh
                                                                                             });

            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Refresh-Token-ExpiryInSeconds", tokenModel.RefreshTokenExpiryInSeconds.ToString(),
                                                                                                    new CookieOptions()
                                                                                                    {
                                                                                                        HttpOnly = true,
                                                                                                        SameSite = SameSiteMode.Strict,
                                                                                                        Secure = true,
                                                                                                        Expires = expirytimeRefresh
                                                                                                    });
            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Username", userName,
                                                                    new CookieOptions()
                                                                    {
                                                                        HttpOnly = true,
                                                                        SameSite = SameSiteMode.Strict,
                                                                        Secure = true,
                                                                        Expires = expirytimeRefresh
                                                                    });

        }

    }
}
