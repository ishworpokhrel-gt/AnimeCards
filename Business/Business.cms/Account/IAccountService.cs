using Animals.Account;
using Common_Shared.ResponseResult;

namespace Business.Business.cms.Account
{
    public interface IAccountService
    {
        //Task<ResponseResult> RegisterAsync(LogInRequestModel model);
        Task<ResponseResult> LogInAsync(LogInRequestModel model);
        //Task<ResponseResult> LogOutAsync();
    }
}
