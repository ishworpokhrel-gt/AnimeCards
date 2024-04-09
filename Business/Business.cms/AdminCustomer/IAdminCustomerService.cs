using Common_Shared.ResponseResult;
using Models.Admin;
using Models.PaginationModel;

namespace Business.Business.cms.AdminCustomer
{
    public interface IAdminCustomerService
    {
        Task<ResponseResult> GetAllCustomerAsync(PaginationRequestModel model);
        Task<ResponseResult> ChangeCustomerPasswordAsync(string customerId, ChangeCustomerPasswordRequestModel model);
        Task<ResponseResult> UpdateCustomerAsync(string customerId, UpdateCustomerRequestModel model);
    }
}
