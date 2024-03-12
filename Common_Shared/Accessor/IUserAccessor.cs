using Common_Shared.CommonModel;

namespace Common_Shared.Accessor
{
    public interface IUserAccessor
    {
        void SetAuthCookiesInClient(TokenModel tokenModel, string userName);
    }
}
