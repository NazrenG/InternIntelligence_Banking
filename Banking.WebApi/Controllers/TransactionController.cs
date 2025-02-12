using Banking.Business.Abstract;
using Banking.Entities.Models;
using Banking.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Banking.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    { 
         
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;

        public TransactionController(ITransactionService transactionService, IAccountService accountService, IUserService userService)
        {
            _transactionService = transactionService;
            _accountService = accountService;
            _userService = userService;
        } 

        [Authorize(Roles = "User")]
        [HttpPost("MoneyTransfer")]
        public async Task<IActionResult> PostTransferMoney([FromBody] TransactionDto dto)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) { return BadRequest(new { Message = "user not find" }); }
            if (dto.Amount <= 0) { return BadRequest(new { Message = "amount must be greater than 0" }); }

            if (dto.SenderAccountNumber.Length != 16 || dto.ReceiverAccountNumber.Length != 16) { return BadRequest(new { Message = "write correct account number" }); }


            var recieverUserId= await _accountService.GetUserIdByAccountNumber(dto.ReceiverAccountNumber);
            var senderUserId = await _accountService.GetUserIdByAccountNumber(dto.SenderAccountNumber);
            if (senderUserId != userId) return Unauthorized(new { Message = "you are not authorized for this account" });


            var recieverAccountId = await _accountService.GetAccountIdByNumber(dto.ReceiverAccountNumber);
            var senderAccountId = await _accountService.GetAccountIdByNumber(dto.SenderAccountNumber);
            if (await _accountService.ChangeBalance(senderAccountId, recieverAccountId, dto.Amount))
            {
                var receiverName=await _userService.GetById(recieverUserId);
                var senderName=await _userService.GetById(senderUserId);
                await _transactionService.AddTransaction(new Transaction
                {
                    Amount = dto.Amount,
                    ReceiverAccountId = recieverAccountId,
                    SenderAccountId = senderAccountId,
                    Created = DateTime.Now,
                    Status=dto.Status,
                    Message = $"{senderName.UserName} sent {dto.Amount}$ to {receiverName.UserName}"
                });
                return Ok(new { Message = "money transfer succesfully" });
            }
            return BadRequest(new { Message = "your acount number is wrong" });
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("AllTransactions")]
        public async Task<IActionResult> GetTransaction()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest(new { Message = "User not found" });
            }

            var user = await _userService.GetById(userId);
            if (user == null)
            {
                return BadRequest(new { Message = "User does not exist" });
            }

            var userAccounts = await _accountService.GetAccounts(userId);
            if (userAccounts == null || !userAccounts.Any())
            {
                return BadRequest(new { Message = "You have no accounts yet" });
            }
             
            var accountIds = userAccounts.Select(a => a.Id).ToList(); 
            var transactions = await _transactionService.GetAllTransactions(accountIds);

            return Ok(transactions);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdatedTransferStatus/{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] string status)
        {
            var transaction=await _transactionService.GetById(id);
            transaction.Status = status;    
            await _transactionService.UpdateTransaction(transaction);

            return Ok(new { Message = "updated transaction successfully" });

        }
    }
}
