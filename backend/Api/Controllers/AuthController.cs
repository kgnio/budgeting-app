using Api.DTOs.Auth;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            JwtTokenService jwtTokenService
        )
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Bu email zaten kullanımda." });
            }

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(
                    new
                    {
                        message = "Kayıt başarısız.",
                        errors = result.Errors.Select(x => x.Description),
                    }
                );
            }

            var token = _jwtTokenService.CreateToken(user);

            return Ok(
                new AuthResponseDto
                {
                    Token = token,
                    UserId = user.Id,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName,
                }
            );
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return Unauthorized(new { message = "Email veya şifre hatalı." });
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!passwordValid)
            {
                return Unauthorized(new { message = "Email veya şifre hatalı." });
            }

            var token = _jwtTokenService.CreateToken(user);

            return Ok(
                new AuthResponseDto
                {
                    Token = token,
                    UserId = user.Id,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName,
                }
            );
        }
    }
}
