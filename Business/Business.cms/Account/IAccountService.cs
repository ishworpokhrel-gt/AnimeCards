using Common_Shared.ResponseResult;
using Models.Account;

namespace Business.Business.cms.Account
{
    public interface IAccountService
    {
        Task<ResponseResult> LogInAsync(LogInRequestModel model);
        Task<ResponseResult> ChangePasswordAsync(ChangePasswordRequestModel passwordModel);
        Task<ResponseResult> RegistrationAsync(RegistrationRequestModel Model);
        Task<ResponseResult> GetProfileAsync();
        Task<ResponseResult> UpdateProfileAsync(string Id , UpdateProfileRequestModel model);
    }
}
