using AnimeCards.Filters.AuthorizationFilters;
using Business.Business.cms.AdminCustomer;
using Common_Shared.Constants;
using Common_Shared.ResponseWrapper;
using Microsoft.AspNetCore.Mvc;
using Models.Admin;
using Models.PaginationModel;
using System.Threading.Tasks;

namespace AnimeCards.Controllers.Admin
{
    public class AdminCustomerController : BaseApiController
    {
        private readonly IAdminCustomerService _adminCustomerService;
        public AdminCustomerController(IAdminCustomerService adminCustomerService)
        {
            _adminCustomerService = adminCustomerService;
        }

        [HttpPost("Get All Customer")]
        [Permission(PermissionConstants.GetCustomer)]
        public async Task<IActionResult> GetAllCustomer(PaginationRequestModel model)
        {
            var responseData = await _adminCustomerService.GetAllCustomerAsync(model);
            if (responseData.IsSuccess)
            {
                return Ok(SuccessPaginateResponseWrapper<object, object>.WrapSuccess(responseData.Result, responseData.Pagination));
            }
            return BadRequest(ErrorResponseWrapper.ErrorApi(responseData.Message));

        }

        [HttpPost("Update Customer")]
        [Permission(PermissionConstants.UpdateCustomer)]
        public async Task<IActionResult> UpdateCustomer(string customerId, UpdateCustomerRequestModel model)
        {
            var responseData = await _adminCustomerService.UpdateCustomerAsync(customerId, model);
            if (responseData.IsSuccess)
            {
                return Ok(SuccessResponseWrapper<object>.SuccessApi(responseData.Result));
            }
            return BadRequest(ErrorResponseWrapper.ErrorApi(responseData.Message));

        }

        [HttpPost("Change Customer Password")]
        [Permission(PermissionConstants.ChangeCustomerPassword)]
        public async Task<IActionResult> ChangeCustomerPassword(string customerId, ChangeCustomerPasswordRequestModel model)
        {
            var responseData = await _adminCustomerService.ChangeCustomerPasswordAsync(customerId,model);
            if (responseData.IsSuccess)
            {
                return Ok(SuccessResponseWrapper<object>.SuccessApi(responseData.Result));
            }
            return BadRequest(ErrorResponseWrapper.ErrorApi(responseData.Message));

        }
    }
}
