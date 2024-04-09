using AnimeCards.Filters.AuthorizationFilters;
using Business.Business.cms.Account;
using Common_Shared.Constants;
using Common_Shared.ResponseWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Account;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;

namespace AnimeCards.Controllers.Admin
{

    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("Registration")]
        public async Task<IActionResult> Registration(RegistrationRequestModel model)
        {
            var result = await _accountService.RegistrationAsync(model);
            if (result.IsSuccess)
                return Ok(SuccessResponseWrapper<object>.SuccessApi(result.Result));
            return BadRequest(ErrorResponseWrapper.ErrorApi(result.Message));
        }

        [AllowAnonymous]
        [HttpPost("ValidRegistration")]
        public async Task<IActionResult> ValidRegistration(string UserId, string Otp)
        {
            var result = await _accountService.ValidRegistrationAsync(UserId, Otp);
            if (result.IsSuccess)
                return Ok(SuccessResponseWrapper<object>.SuccessApi(result.Result));
            return BadRequest(ErrorResponseWrapper.ErrorApi(result.Message));
        }



        [AllowAnonymous]
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(LogInRequestModel model)
        {
            var result = await _accountService.LogInAsync(model);
            if (result.IsSuccess)
                return Ok(SuccessResponseWrapper<object>.SuccessApi(result.Result));
            return BadRequest(ErrorResponseWrapper.ErrorApi(result.Message));
        }

        [HttpPost("LogOut")]
        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Delete("X-Access-Token");
            HttpContext.Response.Cookies.Delete("X-Access-Token-ExpiryInSeconds");
            HttpContext.Response.Cookies.Delete("X-Refresh-Token");
            HttpContext.Response.Cookies.Delete("X-Refresh-Token-ExpiryInSeconds");
            return Ok(SuccessResponseWrapper<object>.SuccessApi("Log out successfully."));
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel model)
        {
            var result = await _accountService.ChangePasswordAsync(model);
            if (result.IsSuccess)
                return Ok(SuccessResponseWrapper<object>.SuccessApi(result.Result));
            return BadRequest(ErrorResponseWrapper.ErrorApi(result.Message));
        }

        [HttpPost("GetProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _accountService.GetProfileAsync();
            if (result.IsSuccess)
                return Ok(SuccessResponseWrapper<object>.SuccessApi(result.Result));
            return BadRequest(ErrorResponseWrapper.ErrorApi(result.Message));
        }

        [HttpPost("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(string Id, UpdateProfileRequestModel model)
        {
            var result = await _accountService.UpdateProfileAsync(Id, model);
            if (result.IsSuccess)
                return Ok(SuccessResponseWrapper<object>.SuccessApi(result.Result));
            return BadRequest(ErrorResponseWrapper.ErrorApi(result.Message));
        }


    }
}
