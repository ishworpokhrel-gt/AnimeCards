using Common_Shared.Accessor;
using Common_Shared.ResponseResult;
using Common_Shared.SieveExtensio;
using Data;
using Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Admin;
using Models.PaginationModel;

namespace Business.Business.cms.AdminCustomer
{
    public class AdminCustomerService : IAdminCustomerService
    {
        private readonly AppDbContext _dbContext;
        private readonly ISieveService _sieveService;
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminCustomerService(AppDbContext dbContext
                                    , ISieveService sieveService
                                     , IUserAccessor userAccessor
                                      , UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _sieveService = sieveService;
            _userAccessor = userAccessor;
            _userManager = userManager;
        }

        public async Task<ResponseResult> ChangeCustomerPasswordAsync(string customerId, ChangeCustomerPasswordRequestModel model)
        {
            var loggedInUser = _userAccessor.GetUserId();
            try
            {
                var adminUser = await _dbContext.Users
                                                      .Where(a => a.Id == loggedInUser && !a.IsDeleted)
                                                      .FirstOrDefaultAsync();

                var customerUser = await _dbContext.Customer
                                                            .Include(a => a.User)
                                                            .Where(a => a.Id == customerId && !a.IsDeleted)
                                                            .FirstOrDefaultAsync();

                if (customerUser == null)
                {
                    return ResponseResult.Failed("User not found.");
                }

                if (model.Password != model.ConfirmPassword)
                {
                    return ResponseResult.Failed("Confirm password and password must be same.");
                }

                bool isPasswordValid = await _userManager.CheckPasswordAsync(adminUser, model.AdminPassword);

                if (!isPasswordValid)
                {
                    return ResponseResult.Failed("Incorrect Admin Password.");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(customerUser.User);
                var changePassword = await _userManager.ResetPasswordAsync(customerUser.User, token, model.Password);
                if (changePassword != IdentityResult.Success)
                {
                    var error = changePassword.Errors.Select(a => a.Description).FirstOrDefault();
                    return ResponseResult.Failed(error);
                }

                await _userManager.UpdateAsync(customerUser.User);

                return ResponseResult.Success("Customer password change successfully.");



            }
            catch (Exception ex)
            {
                throw new Exception("{@Message}", ex);
            }
        }

        public async Task<ResponseResult> GetAllCustomerAsync(PaginationRequestModel model)
        {
            var totalCustomer = _dbContext.Customer
                                                .Where(a => !a.IsDeleted)
                                                .AsQueryable();

            var (result, totalCount, totalPage) = await _sieveService.ApplyPagination(totalCustomer, model);

            var customerInfo = result.Select(a => new
            {
                a.Id,
                a.FullName,
                a.User.PhoneNumber,
                a.User.UserName,
                a.User.Email
            }).ToList();

            var pagination = new Pagination
            {
                Total = totalCount,
                TotalPage = totalPage,
                Current = model.PageNumber,
                PageSize = model.PageSize
            };
            return ResponseResult.Success(customerInfo, pagination);
        }


        public async Task<ResponseResult> UpdateCustomerAsync(string customerId, UpdateCustomerRequestModel model)
        {
            var loggedInAdmin = _userAccessor.GetUserId();
            try
            {
                var adminUser = await _dbContext.Admin.
                                                       Where(a => a.UserId == loggedInAdmin && !a.IsDeleted)
                                                       .FirstOrDefaultAsync();
                var customerInfo = await _dbContext.Customer
                                                          .Where(x => x.Id == customerId)
                                                          .FirstOrDefaultAsync();
                if (customerInfo == null)
                {
                    return ResponseResult.Failed("Customer Not found.");
                }
                var totalUser = await _dbContext.Users
                                                    .Where(a => !a.IsDeleted)
                                                    .ToListAsync();

                if (model.UserName != null || model.Email != null || model.PhoneNumber != null)
                {
                    if (model.UserName.ToLower() != customerInfo.User.UserName.ToLower())
                    {
                        var isUserExist = _userManager.FindByNameAsync(model.UserName);
                        if (isUserExist.Result != null)
                        {
                            return ResponseResult.Failed("UserName already exists, Try another one.");
                        }
                    }
                    if (model.Email.ToLower() != customerInfo.User.Email.ToLower())
                    {
                        var isEmailExist = _userManager.FindByEmailAsync(model.Email);
                        if (isEmailExist.Result != null)
                        {
                            return ResponseResult.Failed("Email already exists, Try another email.");
                        }
                    }
                    if (model.PhoneNumber != customerInfo.User.PhoneNumber)
                    {
                        bool isPhoneNumberExist = totalUser.Any(a => a.PhoneNumber == model.PhoneNumber);
                        if (isPhoneNumberExist)
                        {
                            return ResponseResult.Failed("Email already exists, Try another email.");
                        }

                        if (model.PhoneNumber is string number)
                        {
                            foreach (var num in number)
                            {
                                if (!char.IsDigit(num))
                                {
                                    return ResponseResult.Failed("Incorrect PhoneNumber format.");
                                }
                            }

                        }
                        else
                        {
                            return ResponseResult.Failed("Incorrect phoneNumber.");
                        }

                    }

                }

                customerInfo.User.UserName = model.UserName;
                customerInfo.User.PhoneNumber = model.PhoneNumber;
                customerInfo.FullName = model.FullName;
                customerInfo.User.FullName = model.FullName;
                customerInfo.User.Email = model.Email;
                await _userManager.UpdateAsync(customerInfo.User);
                _dbContext.Customer.Update(customerInfo);
                await _dbContext.SaveChangesAsync();

                return ResponseResult.Success("Customer Updated Successfully.");

            }
            catch (Exception ex)
            {
                throw new Exception("{@Message}", ex);
            }
        }


    }
}
