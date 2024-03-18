using Common_Shared.ResponseWrapper;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;

namespace AnimeCards.Filters.AuthorizationFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] _permission;
        public PermissionAttribute(params string[] permissions)
        {
            _permission = permissions;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User.FindFirst("userId")?.Value;

            var dbContext = context.HttpContext.RequestServices.GetService(typeof(AppDbContext)) as AppDbContext;

            if (_permission.Length == 0)
                return;
            var rolePermissionList = (from userRoles in dbContext.UserRoles
                                      join roles in dbContext.Roles
                                      on userRoles.RoleId equals roles.Id
                                      join roleClaims in dbContext.RoleClaims
                                      on roles.Id equals roleClaims.RoleId
                                      where userRoles.UserId == userId
                                      select roleClaims.Permissions).ToList();

            var userPermissionList = rolePermissionList.SelectMany(x => x).ToList();

            if (userPermissionList.Exists(_permission.Contains))
            {
                return;
            }

            var failedMessage = ErrorResponseWrapper.ErrorApi("Forbidden", 403);

            context.Result = new ObjectResult(failedMessage)
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
                ContentTypes = new Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection() { "application/json" }
            };
        }
    }
}
