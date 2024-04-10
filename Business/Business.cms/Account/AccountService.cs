using Common_Shared.Accessor;
using Common_Shared.CommonModel;
using Common_Shared.Constants;
using Common_Shared.Otp;
using Common_Shared.ResponseResult;
using Common_Shared.SystemList;
using Common_Shared.Token;
using Data;
using Entity;
using Entity.Account;
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
        private readonly OtpGenerator _otpGenerator;
        public AccountService(RoleManager<ApplicationRole> roleManager,
                                          UserManager<ApplicationUser> userManager,
                                          AppDbContext dbContext,
                                          TokenProvider tokenProvider,
                                           IUserAccessor userAccessor,
                                           SignInManager<ApplicationUser> signInManager,
                                            OtpGenerator otpGenerator)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _tokenProvider = tokenProvider;
            _userAccessor = userAccessor;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _otpGenerator = otpGenerator;

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
            if (passwordModel.NewPassword == passwordModel.OldPassword)
            {
                return ResponseResult.Failed("Old password and new password is same.");
            }
            var previousPassword = await _dbContext.PreviousPassword
                                                                    .Where(a => a.UserId == personLoggedIn && !a.IsDeleted)
                                                                    .OrderBy(a => a.CreatedOn)
                                                                    .ToListAsync();

            var validPassword = PasswordValidationCheck.PasswordValidators(passwordModel.NewPassword);

            foreach (var password in previousPassword)
            {
                if (_userManager.PasswordHasher.VerifyHashedPassword(user, password.HashPassword, passwordModel.NewPassword) == PasswordVerificationResult.Success)
                {
                    return ResponseResult.Failed("Old Password, choose new one.");
                }
            }

            int totalPreviousPassword = 5;

            if (!validPassword)
            {
                return ResponseResult.Failed("Please provide strong password with letters and special characters.");
            }

            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {

                if (totalPreviousPassword > 0 && previousPassword.Count >= totalPreviousPassword)
                {
                    var removableData = previousPassword.Count - (totalPreviousPassword - 1);
                    if (removableData > 0)
                    {
                        var dataToRemove = previousPassword.Take(removableData);

                        _dbContext.PreviousPassword.RemoveRange(dataToRemove);
                    }
                }

                var hashpassword = _userManager.PasswordHasher.HashPassword(user, passwordModel.NewPassword);

                var changePassword = await _userManager.ChangePasswordAsync(user, passwordModel.OldPassword, passwordModel.NewPassword);
                if (!changePassword.Succeeded)
                {
                    return ResponseResult.Failed(changePassword.Errors.Select(a => a.Description).FirstOrDefault());
                }

                var previousPasswordStore = new PreviousPassword
                {
                    UserId = user.Id,
                    HashPassword = hashpassword
                };

                await _dbContext.PreviousPassword.AddAsync(previousPasswordStore);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return ResponseResult.Success("password changed successfully.");
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ResponseResult> RegistrationAsync(RegistrationRequestModel Model)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
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
                var hashPassword = _userManager.PasswordHasher.HashPassword(user, Model.Password);

                var previousPassword = new PreviousPassword
                {
                    UserId = user.Id,
                    HashPassword = hashPassword
                };

                var generatedOtp = _otpGenerator.GenerateOtp();
                var hashOtp = _userManager.PasswordHasher.HashPassword(user, generatedOtp.Item1);

                var otp = new UserOtp
                {
                    OtpCode = hashOtp,
                    UserId = user.Id,
                    OtpModule = SystemConstant.CustomerRole,
                    Type = OtpType.SignUp
                };

                var customer = new Customer
                {
                    UserId = user.Id,
                    FullName = user.FullName
                };

                await _dbContext.UserOtp.AddAsync(otp);
                await _dbContext.PreviousPassword.AddAsync(previousPassword);
                await _dbContext.Customer.AddAsync(customer);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return ResponseResult.Success("Registration successful.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("{@Message}", ex);
            }

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
                var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.Id == Id && !a.IsDeleted);
                var totalUser = await _dbContext.Users.Where(a => !a.IsDeleted).ToListAsync();
                if (user == null)
                {
                    return ResponseResult.Failed("User not found.");
                }

                var normalizedEmail = _userManager.NormalizeEmail(model.Email);
                if (user.Email != normalizedEmail)
                {
                    var emailExists = totalUser.Any(x => x.Email == model.Email);
                    if (emailExists)
                    {
                        return ResponseResult.Failed("Email already exists, try another one.");
                    }
                    user.Email = normalizedEmail;
                }

                if (user.PhoneNumber != model.PhoneNumber)
                {
                    var phoneNumberExists = totalUser.Any(x => x.PhoneNumber == model.PhoneNumber);
                    if (phoneNumberExists)
                    {
                        return ResponseResult.Failed("Number already exists, try another one.");
                    }
                    user.PhoneNumber = model.PhoneNumber;
                }

                user.UserName = model.UserName;

                if (!string.IsNullOrWhiteSpace(model.Role))
                {
                    var roles = model.Role.Split(',');
                    var rolesExist = await _roleManager.Roles.AnyAsync(r => roles.Contains(r.Name));
                    if (!rolesExist)
                    {
                        return ResponseResult.Failed("One or more roles do not exist.");
                    }

                    await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                    await _userManager.AddToRolesAsync(user, roles);
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

        public async Task<ResponseResult> ValidRegistrationAsync(string UserId, string Otp)
        {
            try
            {
                var registratingUser = await _dbContext.UserOtp
                                                            .Include(x => x.User)
                                                            .Where(a => a.UserId == UserId)
                                                            .Select(a => new
                                                            {
                                                                a.OtpCode,
                                                                a.User
                                                            })
                                                            .FirstOrDefaultAsync();
                if (registratingUser.User == null)
                {
                    return ResponseResult.Failed("User not found");
                }

                var result = _userManager.PasswordHasher.VerifyHashedPassword(registratingUser.User, registratingUser.OtpCode, Otp);

                if (result == PasswordVerificationResult.Failed)
                {
                    return ResponseResult.Failed("Invalid Otp");
                }

                registratingUser.User.IsRegistrationComplete = true;
                _dbContext.Users.Update(registratingUser.User);
                await _dbContext.SaveChangesAsync();

                return ResponseResult.Success("Registration complete.");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
