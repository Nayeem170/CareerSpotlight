using CareerSpotlightApi.Models;
using CareerSpotlightApi.Models.Settings;
using CareerSpotlightApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CareerSpotlightApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IJwtTokenService _tokenService;
        private readonly IdentitySettings _identitySettings;
        private readonly JwtSettings _jwtSettings;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailService emailService,
            IJwtTokenService tokenService,
            IOptions<IdentitySettings> identitySettingsOptions,
            IOptions<JwtSettings> _jwtSettingsOptions
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _tokenService = tokenService;
            _identitySettings = identitySettingsOptions.Value;
            _jwtSettings = _jwtSettingsOptions.Value;
        }

        [HttpPost("SendCode")]
        public async Task<IActionResult> SendCode([FromBody] EmailModel emailModel)
        {
            var user = await _userManager.FindByEmailAsync(emailModel.Email ?? string.Empty);
            if (user == null)
            {
                user = new User { UserName = emailModel.Email, Email = emailModel.Email };
                var result = await _userManager.CreateAsync(user, _identitySettings.Password ?? string.Empty);
                if (!result.Succeeded)
                {
                    Console.WriteLine("Unable to create new user.");
                    return BadRequest("Unable to create new user.");
                }
            }

            // Generate a short token using the custom provider
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, "EmailVerification");
            _emailService.SendEmail(emailModel.Email ?? string.Empty, "Your authentication code", $"Your authentication code is {code}");

            // Optionally log the token for debugging
            Console.WriteLine($"Generated token: {code}");

            return Ok("Verification code sent.");
        }

        [HttpPost("VerifyCode")]
        public async Task<IActionResult> VerifyCode([FromBody] VerificationModel verificationModel)
        {
            var user = await _userManager.FindByEmailAsync(verificationModel.Email ?? string.Empty);
            if (user == null)
            {
                return BadRequest("Invalid email address.");
            }

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "EmailVerification", verificationModel.Code ?? string.Empty);
            if (isValid)
            {
                var jwtTokens = _tokenService.GenerateTokens(user.Email ?? string.Empty);
                return Ok(new
                {
                    AccessToken = jwtTokens.accessToken,
                    RefreshToken = jwtTokens.refreshToken
                });
            }

            return Unauthorized("Invalid verification code.");
        }

        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken([FromBody] TokenModel tokenModel)
        {
            var email = _tokenService.GetEmail(tokenModel.RefreshToken);
            if (email is null)
            {
                return Unauthorized("Invalid refresh token.");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(email);
            return Ok(new { AccessToken = newAccessToken });
        }
    }
}
