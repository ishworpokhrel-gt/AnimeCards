using Common_Shared.Accessor;
using Common_Shared.CommonModel;
using Common_Shared.ResponseResult;
using Common_Shared.Token;
using Data;
using Entity;
using Microsoft.AspNetCore.Identity;
using Models;
using Models.Account;

namespace Business.Business.cms.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _dbContext;
        private readonly TokenProvider _tokenProvider;
        private readonly IUserAccessor _userAccessor;
        public AccountService(RoleManager<IdentityRole> roleManager,
                                          UserManager<ApplicationUser> userManager,
                                          AppDbContext dbContext,
                                          TokenProvider tokenProvider,
                                           IUserAccessor userAccessor)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _tokenProvider = tokenProvider;
            _userAccessor = userAccessor;
        }
        public async Task<ResponseResult> LogInAsync(LogInRequestModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return ResponseResult.Failed("User not found");
            }
            bool validUser = await _userManager.CheckPasswordAsync(user, model.Password);
            var roles = await _userManager.GetRolesAsync(user);

            if (!validUser)
            {
                user.AccessFailedCount++;
                await _userManager.UpdateAsync(user);
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
