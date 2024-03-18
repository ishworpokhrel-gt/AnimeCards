using Common_Shared.ResponseResult;
using Models.Account;

namespace Business.Business.cms.Account
{
    public interface IAccountService
    {
        Task<ResponseResult> LogInAsync(LogInRequestModel model);
        Task<ResponseResult> ChangePasswordAsync(ChangePasswordRequestModel passwordModel);
    }
}
