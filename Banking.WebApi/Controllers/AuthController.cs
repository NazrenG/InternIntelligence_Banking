using Banking.Business.Abstract;
using Banking.Entities.Models;
using Banking.WebApi.Dtos;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Banking.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly IUserSettingService _userSettingService;

        public AuthController(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager, IUserService userService, IUserSettingService userSettingService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _userSettingService = userSettingService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> SignUp(SignUpDto dto)
        {
            {
                var user = new User
                {
                    UserName = dto.Username,
                    Email = dto.Email,
                };

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(new { Status = "Error", Message = "User creation failed!", Errors = result.Errors });
                }


                var role = string.IsNullOrEmpty(dto.Role) || (dto.Role != "Admin" && dto.Role != "User") ? "User" : dto.Role;
                await _userManager.AddToRoleAsync(user, role);
                var userId = await _userService.GetLastUserId();
                await _userSettingService.Add(new UserSetting { UserId = userId });
                return Ok(new { Status = "Success", Message = "User created successfully!" });
            }

        }

        [HttpPost("Login")]
        public async Task<IActionResult> SignIn(SignInDto dto)
        {
            {
                var user = await _userManager.FindByNameAsync(dto.Username);
                if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                    return Unauthorized();

                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim("role", role));
                }

                var token = GetToken(authClaims);

                return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token), Expiration = token.ValidTo });
            }
        }
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(SignInDto dto)
        {
            {
                var user = await _userManager.FindByNameAsync(dto.Username);
                if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                    return Unauthorized();

                await _signInManager.SignOutAsync();
                return Ok(new { message = "Logout successful" });

            }
        }

    }
}
