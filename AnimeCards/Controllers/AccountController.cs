using AnimeCards.Filters.AuthorizationFilters;
using Business.Business.cms.Account;
using Common_Shared.Constants;
using Common_Shared.ResponseWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Account;
using System;
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
        public async Task<IActionResult> LogIn(LogInRequestModel model)
        {
            var result = await _accountService.LogInAsync(model);
            if (result.IsSuccess)
                return Ok(SuccessResponseWrapper<object>.SuccessApi(result.Result));
            return BadRequest(ErrorResponseWrapper.ErrorApi(result.Message));
        }
        [Authorize]
        [HttpPost("ChangePassword")]
        [Permission(PermissionConstants.Update)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel model)
        {
            var result = await _accountService.ChangePasswordAsync(model);
            if (result.IsSuccess)
                return Ok(SuccessResponseWrapper<object>.SuccessApi(result.Result));
            return BadRequest(ErrorResponseWrapper.ErrorApi(result.Message));
        }
    }
}
