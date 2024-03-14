using Common_Shared.CommonModel;
using Models.Account;

namespace Common_Shared.Accessor
{
    public interface IUserAccessor
    {
        void SetAuthCookiesInClient(TokenModel tokenModel, string userName);
        LoggingUser GetUserName();
        string GetIpAddress();
        string GetUserId();
    }
}
