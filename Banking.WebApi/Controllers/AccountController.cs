using Banking.Business.Abstract;
using Banking.Entities.Models;
using Banking.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Banking.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public AccountController(IAccountService accountService, IUserService userService, UserManager<User> userManager)
        {
            _accountService = accountService;
            _userService = userService;
            _userManager = userManager;
        }

        [Authorize(Roles ="User")]
        [HttpGet("UserAccounts")]
        public async Task<IActionResult> GetUserAccounts()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var accounts = await _accountService.GetAccounts(userId);
            var list = accounts.Select(u => new AccountDto
            {
                AccountNumber = u.AccountNumber,
                Limit = u.Limit,
                Balance = u.Balance,
            }).ToList();
            return Ok(list);
        }

        [Authorize(Roles = "User")]
        [HttpPost("NewAccount")]
        public async Task<IActionResult> PostNewAccount([FromBody] AccountDto dto)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) { return BadRequest(new { Message = "user not find" }); }

            if (dto.Balance < 0) { return BadRequest(new { Message = "balance cannot less than 0" }); }
            if (dto.Limit < 0) { return BadRequest(new { Message = "limit cannot less than 0" }); }
            if (string.IsNullOrEmpty(dto.AccountNumber) || dto.AccountNumber.Length != 16)
            { return BadRequest(new { Message = "write correct account number" }); }
            if (await _accountService.CheckAccountNumber(dto.AccountNumber))
            {
                var account = new Account
                {
                    AccountNumber = dto.AccountNumber,
                    Limit = dto.Limit,
                    Balance = dto.Balance,
                    UserId = userId
                };
                await _accountService.AddAccount(account); return Ok(account);
            }
            return BadRequest(new { Message = "This account already yet" });

        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete("DeletedAccount/{id}")]
        public async Task<IActionResult> DeleteAccount(int id, [FromBody] PasswordDto dto)
        { 

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) { return BadRequest(new { Message = "user not find" }); }
            var user = await _userService.GetById(userId);
            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return BadRequest(new { Message = "user password not correct" });
            }

            await _accountService.DeleteAccount(id);
            return Ok(new { Message = "deleted account succesfully" });
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdatedAccount/{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] AccountDto dto)
        {
            if (dto.Balance < 0) { return BadRequest(new { Message = "balance cannot less than 0" }); }
            if (dto.Limit < 0) { return BadRequest(new { Message = "limit cannot less than 0" }); }
            if (string.IsNullOrEmpty(dto.AccountNumber) || dto.AccountNumber.Length != 16)
            { return BadRequest(new { Message = "write correct account number" }); }

            var account = await _accountService.GetAccountById(id);
            account.AccountNumber = dto.AccountNumber;
            account.Balance = dto.Balance;
            account.Limit = dto.Limit;

            await _accountService.UpdateAccount(account);
            return Ok(new { Message = "updated account succesfully" });
        }

    }
}
