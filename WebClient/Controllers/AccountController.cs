using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebClient.Core.ViewModels;
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

            var token = AuthHelper.BuildToken(account);
            this.Response.Cookies.Append("token", token);

            return this.Ok(new
            {
                token = token,
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

        /// <summary>
        /// Create new account
        /// </summary>
        /// <param name="accountVM">Account VM</param>
        /// <returns>Action result</returns>
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> Create(AccountVM accountVM)
        {
            try
            {
                var account = await this.accountService.CreateAccount(accountVM, this.authHelper.UserId);
                return this.Ok(account);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Account
        /// </summary>
        /// <param name="code">account code</param>
        /// <returns>Action result</returns>
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> Delete(string code)
        {
            try
            {
                var account = await this.accountService.DeleteAccount(code, this.authHelper.UserId);
                return this.Ok(account);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="code">Account code</param>
        /// <returns>New password</returns>
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> ResetPassword(string code)
        {
            var password = await this.accountService.ResetPassword(code, this.authHelper.UserId);
            return this.Ok(password);
        }

        /// <summary>
        /// Get accounts by employeeId
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>List accounts</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByEmployeeId(int id)
        {
            try
            {
                var list = await this.accountService.GetAccountsByEmployeeId(id);
                return this.Ok(list);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets treenode accounts
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <returns>Tree node accounts</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetTreeNodeAccounts(int id)
        {
            try
            {
                var list = await this.accountService.GetTreeNodeAccounts(id);
                return this.Ok(list);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
