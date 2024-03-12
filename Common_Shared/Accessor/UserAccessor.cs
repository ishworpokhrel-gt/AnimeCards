using Common_Shared.CommonModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Web;

namespace Common_Shared.Accessor
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _webHostEnvironment;
        public UserAccessor(IHttpContextAccessor httpContextAccessor
                            , IHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
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
