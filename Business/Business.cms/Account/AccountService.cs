using Animals.Account;
using Common_Shared.Accessor;
using Common_Shared.CommonModel;
using Common_Shared.ResponseResult;
using Common_Shared.Token;
using Data;
using Entity;
using Microsoft.AspNetCore.Identity;

namespace Business.Business.cms.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _dbContext;
        private readonly TokenProvider _tokenProvider;
        private readonly IUserAccessor _userAccessor;
        public AccountService(RoleManager<ApplicationRole> roleManager,
                                          UserManager<ApplicationUser> userManager,
                                          AppDbContext dbContext,
                                          TokenProvider tokenProvider,
                                           IUserAccessor userAccessor,
                                           SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _tokenProvider = tokenProvider;
            _userAccessor = userAccessor;
            _signInManager = signInManager;
        }
        public async Task<ResponseResult> LogInAsync(LogInRequestModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return ResponseResult.Failed("User not found");
            }
            var validUser = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (validUser == SignInResult.Failed)
            {
                user.AccessFailedCount++;
                await _userManager.UpdateAsync(user);
            }
            if (user.AccessFailedCount > 5)
            {
                var lockOutTime = 2;
                var lockTime = DateTime.UtcNow.AddMinutes(lockOutTime);

                await _userManager.ResetAccessFailedCountAsync(user);
                await _userManager.SetLockoutEndDateAsync(user, lockTime);
                await _userManager.UpdateAsync(user);

                return ResponseResult.Failed("Too many login attempt please try again later.");
            }
            if (validUser.IsLockedOut || validUser.IsNotAllowed)
            {
                return ResponseResult.Failed("Too many login attempt please try again later.");
            }
            if (validUser == SignInResult.Failed)
            {
                return ResponseResult.Failed("Incorrect username or password.");
            }

            var tokenProvided = _tokenProvider.createadmincookies(user);

            _userAccessor.SetAuthCookiesInClient(new TokenModel
            {
                AccessToken = tokenProvided.Item1,
                AccessTokenExpiryInSeconds = tokenProvided.Item2
            }, user.UserName);

            return ResponseResult.Success("Login Successfully");

        }


    }
}
