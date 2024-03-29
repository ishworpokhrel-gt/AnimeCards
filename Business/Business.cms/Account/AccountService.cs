using Common_Shared.Accessor;
using Common_Shared.CommonModel;
using Common_Shared.Constants;
using Common_Shared.ResponseResult;
using Common_Shared.SystemList;
using Common_Shared.Token;
using Data;
using Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Account;

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
            _userManager = userManager;
            _dbContext = dbContext;
            _tokenProvider = tokenProvider;
            _userAccessor = userAccessor;
            _signInManager = signInManager;
            _roleManager = roleManager;
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

            var tokenProvided = _tokenProvider.createadmintoken(user);
            var tokenRefreshProvided = _tokenProvider.AdminRefreshToken(user);

            _userAccessor.SetAuthCookiesInClient(new TokenModel
            {
                AccessToken = tokenProvided.Item1,
                AccessTokenExpiryInSeconds = tokenProvided.Item2,
                RefreshToken = tokenRefreshProvided.Item2,
                RefreshTokenExpiryInSeconds = tokenRefreshProvided.Item1
            }, user.UserName);

            return ResponseResult.Success("Login Successfully");

        }

        public async Task<ResponseResult> ChangePasswordAsync(ChangePasswordRequestModel passwordModel)
        {
            var personLoggedIn = _userAccessor.GetUserId();

            var user = await _dbContext.Users
                                       .Where(a => a.Id == personLoggedIn)
                                       .FirstOrDefaultAsync();
            if (user == null)
            {
                return ResponseResult.Failed("User not found.");
            }
            bool validUser = await _userManager.CheckPasswordAsync(user, passwordModel.OldPassword);
            if (!validUser)
            {
                return ResponseResult.Failed("Incorrect old password.");
            }
            var validPassword = PasswordValidationCheck.PasswordValidators(passwordModel.NewPassword);

            if (!validPassword)
            {
                return ResponseResult.Failed("Please provide strong password with letters and special characters.");
            }
            var hashpassword = _userManager.PasswordHasher.HashPassword(user, passwordModel.NewPassword);

            var changePassword = await _userManager.ChangePasswordAsync(user, passwordModel.OldPassword, hashpassword);
            if (!changePassword.Succeeded)
            {
                return ResponseResult.Failed(changePassword.Errors.Select(a => a.Description).FirstOrDefault());
            }
            return ResponseResult.Success("password changed successfully.");
        }

        public async Task<ResponseResult> RegistrationAsync(RegistrationRequestModel Model)
        {
            var validUser = await _userManager.FindByNameAsync(Model.UserName);
            var checkPassword = await _userManager.CheckPasswordAsync(validUser, Model.Password);

            if (validUser != null && checkPassword)
            {
                return ResponseResult.Failed("User already exists.");
            }

            if (Model.PhoneNumber != null)
            {
                if (Model.PhoneNumber is string value)
                {
                    foreach (var obj in value)
                    {
                        if (!char.IsDigit(obj))
                        {
                            return ResponseResult.Failed("Phone number invalid.");
                        }
                    }

                }
                else
                {
                    return ResponseResult.Failed("Invalid phone number.");
                }
            }

            var validPassword = PasswordValidationCheck.PasswordValidators(Model.Password);
            if (!validPassword)
            {
                return ResponseResult.Failed("Password must have alteast one uppercase,lowercase,number and one unique character.");
            }
            var details = await _dbContext.Users
                                        .Where(a => !a.IsDeleted)
                                        .Select(a => new
                                        {
                                            Number = a.PhoneNumber,
                                            totalEmail = a.Email
                                        })
                                        .ToListAsync();
            if (details.Any(a => a.totalEmail == Model.Email))
            {
                return ResponseResult.Failed("Email already exists.");
            }

            if (details.Any(a => a.Number == Model.PhoneNumber))
            {
                return ResponseResult.Failed("Phone number alredy exists.");
            }

            if (await _dbContext.Users.AnyAsync(a => !a.IsDeleted && a.PhoneNumber == Model.PhoneNumber))
            {
                return ResponseResult.Failed("Phone number already exists.");
            }


            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = Model.UserName,
                FullName = Model.FullName,
                PasswordHash = Model.Password,
                Email = Model.Email,
                PhoneNumber = Model.PhoneNumber
            };

            var createuser = await _userManager.CreateAsync(user, Model.Password);
            if (!createuser.Succeeded)
            {
                var error = createuser.Errors.Select(a => a.Description).FirstOrDefault();
                return ResponseResult.Failed(error);
            }
            await _userManager.AddToRoleAsync(user, SystemConstant.CustomerRole);

            await _dbContext.SaveChangesAsync();
            return ResponseResult.Success("Registration successful.");

        }

        public async Task<ResponseResult> GetProfileAsync()
        {
            var loggedInUser = _userAccessor.GetUserId();

            var user = await (from users in _dbContext.Users
                              join userroles in _dbContext.UserRoles
                              on users.Id equals userroles.UserId
                              join roles in _dbContext.Roles
                              on userroles.RoleId equals roles.Id
                              where users.Id == loggedInUser
                              select new
                              {
                                  users,
                                  Roles = roles.Name
                              }).ToListAsync();
            var concatenatedRoles = string.Join(", ", user.Select(x => x.Roles).Where(x => x != null));

            var realUser = user.Select(a => a.users).FirstOrDefault();




            if (realUser == null)
            {
                return ResponseResult.Failed("User not found.");
            }
            var response = new
            {
                realUser.Id,
                realUser.UserName,
                realUser.Email,
                realUser.PhoneNumber,
                Role = concatenatedRoles
            };

            return ResponseResult.Success(response);

        }

        public async Task<ResponseResult> UpdateProfileAsync(string Id, UpdateProfileRequestModel model)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var User = await _dbContext.Users
                                                .Where(a => a.Id == Id && !a.IsDeleted)
                                                .FirstOrDefaultAsync();
                var userRole = await _userManager.GetRolesAsync(User);
                var userList = await _dbContext.Users
                                                .Where(a => !a.IsDeleted)
                                                .ToListAsync();

                if (User == null)
                {
                    return ResponseResult.Failed("User not found.");
                }

                var normalizedEmail = _userManager.NormalizeEmail(model.Email);
                if (User.Email != normalizedEmail)
                {
                    if (userList.Exists(x => x.Email == model.Email))
                    {
                        return ResponseResult.Failed("Email already exists, try another one.");
                    }

                }
                if (User.PhoneNumber != model.PhoneNumber)
                {
                    if (userList.Exists(x => x.PhoneNumber == model.PhoneNumber))
                    {
                        return ResponseResult.Failed("Number already exists, try another one.");
                    }

                }
                User.Email = normalizedEmail;
                User.PhoneNumber = model.PhoneNumber;
                User.UserName = model.UserName;
                
                foreach (var roleexist in model.Role.Split(","))
                {
                    bool isRoleExists = _roleManager.RoleExistsAsync(roleexist.ToString()).GetAwaiter().GetResult();
                    if (!isRoleExists)
                    {
                        return ResponseResult.Failed("Role doesnot exists.");
                    }
                }

                _dbContext.Users.Update(User);

                if (model.Role != null && model.Role.Count() > 0)
                {
                    await _userManager.RemoveFromRolesAsync(User, userRole);
                    foreach (var role in model.Role.Split(","))
                    {
                        await _userManager.AddToRoleAsync(User, role.ToString());

                    }

                }

                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return ResponseResult.Success("Profile updated successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResponseResult.Failed($"{ex.Message}");
            }


        }
    }
}
