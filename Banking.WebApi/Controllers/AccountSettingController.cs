using Banking.Business.Abstract;
using Banking.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Security.Claims;

namespace Banking.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("fixed")] 
    public class AccountSettingController : ControllerBase
    {
        private readonly IUserSettingService _userSettingService;

        public AccountSettingController(IUserSettingService userSettingService)
        {
            _userSettingService = userSettingService;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("UserSetting")]
        public async Task<IActionResult> GetUserSetting()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) { return BadRequest(new { Message = "user not find" }); }
          
            var setting=await _userSettingService.GetUserSetting(userId);
            return Ok(new SettingDto
            {
                AppNotificationsEnabled = setting.AppNotificationsEnabled,
                EmailNotificationsEnabled = setting.EmailNotificationsEnabled,
                LowBalanceAlertEnabled = setting.LowBalanceAlertEnabled,
                TwoFactorEnabled = setting.TwoFactorEnabled,
            });
        }


        [Authorize(Roles = "User,Admin")]
        [HttpPut("UpdateSettings")]
        public async Task<IActionResult> UpdateSettings([FromBody] SettingDto updatedSettings)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized(new { Message = "User not found" });

            var settings = await _userSettingService.GetUserSetting(userId);
            if (settings == null) return NotFound(new { Message = "Settings not found" });

            settings.TwoFactorEnabled = updatedSettings.TwoFactorEnabled;
            settings.EmailNotificationsEnabled = updatedSettings.EmailNotificationsEnabled;
            settings.AppNotificationsEnabled = updatedSettings.AppNotificationsEnabled;
            settings.LowBalanceAlertEnabled = updatedSettings.LowBalanceAlertEnabled;

            await _userSettingService.Update(settings);
            return Ok(new { Message = "Settings updated successfully" });
        }

         
        [Authorize(Roles = "User,Admin")]
        [HttpPatch("ToggleTwoFactor")]
        public async Task<IActionResult> ToggleTwoFactor([FromQuery] bool enable)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized(new { Message = "User not found" });

            var settings = await _userSettingService.GetUserSetting(userId);
            if (settings == null) return NotFound(new { Message = "Settings not found" });

            settings.TwoFactorEnabled = enable;
            await _userSettingService.Update(settings);

            return Ok(new { Message = $"Two-factor authentication {(enable ? "enabled" : "disabled")} successfully" });
        }
         
        [Authorize(Roles = "User,Admin")]
        [HttpPatch("ToggleLowBalanceAlert")]
        public async Task<IActionResult> ToggleLowBalanceAlert([FromQuery] bool enable)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized(new { Message = "User not found" });

            var settings = await _userSettingService.GetUserSetting(userId);
            if (settings == null) return NotFound(new { Message = "Settings not found" });

            settings.LowBalanceAlertEnabled = enable;
            await _userSettingService.Update(settings);

            return Ok(new { Message = $"Low balance alert {(enable ? "enabled" : "disabled")} successfully" });
        }


    }
}
