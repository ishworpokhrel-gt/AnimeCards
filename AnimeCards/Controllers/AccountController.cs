using Animals.Account;
using AnimeCards.Filters.AuthorizationFilters;
using Business.Business.cms.Account;
using Common_Shared.Constants;
using Common_Shared.ResponseWrapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnimeCards.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("LogIn")]
        [Permission(PermissionConstants.Create)]
        public async Task<IActionResult> LogIn(LogInRequestModel model)
        {
            var result = await _accountService.LogInAsync(model);
            if (result.IsSuccess)
                return Ok(SuccessResponseWrapper<object>.SuccessApi(result.Result));
            return BadRequest(ErrorResponseWrapper.ErrorApi(result.Message));
        }
    }
}
