using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;
using WebClient.Extensions;
using WebClient.Extentions;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    /// <summary>
    /// Account controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        /// <summary>
        /// Auth helper
        /// </summary>
        private AuthHelper authHelper;

        /// <summary>
        /// Account services
        /// </summary>
        private IAccountService accountService;

        /// <summary>
        /// Employee service
        /// </summary>
        private IEmployeeService employeeService;

        /// <summary>
        /// A contrustor
        /// </summary>
        /// <param name="authHelper">Auth helper</param>
        /// <param name="accountService">Account service</param>
        /// <param name="employeeService">Employee service</param>
        public AccountController(AuthHelper authHelper, IAccountService accountService, IEmployeeService employeeService)
        {
            this.authHelper = authHelper;
            this.accountService = accountService;
            this.employeeService = employeeService;
        }

        /// <summary>
        /// Login action
        /// </summary>
        /// <param name="loginVM">Login viewmodal</param>
        /// <returns>The token</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var account = await this.accountService.LoginAsync(loginVM.UserName, loginVM.PassWord);

            if (account == null || string.IsNullOrEmpty(account.UserCode))
            {
                return this.BadRequest("Tên đăng nhập hoặc mập khẩu không đúng");
            }

            return this.Ok(new
            {
                token = AuthHelper.BuildToken(account),
                username = account.UserName,
                name = account.EmployeeName,
                department = account.DepartmentName,
                usercode = account.UserCode
            });
        }

        /// <summary>
        /// The API support WebClient/Another systems to know current token is available
        /// </summary>
        /// <returns>Response statuscode is OK if the token is available</returns>
        [HttpGet("/available")]
        [Authorize]
        public IActionResult Test()
        {
            return this.Ok();
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="changePasswordVM">Chang password viewmodal</param>
        /// <returns>Ok or bad request</returns>
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            var userName = this.authHelper.Username;
            var userId = this.authHelper.UserId;
            var isSuccess = await this.accountService.ChangePassword(
                userName, 
                userId, 
                changePasswordVM.MatKhauCu, 
                changePasswordVM.MatKhauMoi);

            if (isSuccess)
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest("Mật khẩu không đúng");
            }
        }

        /// <summary>
        /// Update employee
        /// </summary>
        /// <param name="employeeVM">the newEmployee</param>
        /// <returns>view profile</returns>
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> SaveProfile(EmployeeVM employeeVM)
        {
            var employee = await this.employeeService.UpdateInformationEmployee(employeeVM, this.authHelper.UserId);
            return this.Ok(employee);
        }
    }
}
