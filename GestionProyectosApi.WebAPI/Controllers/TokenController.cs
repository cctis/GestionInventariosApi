using GestionProyectosApi.Domain.Models;
using GestionProyectosApi.Domain.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GestionProyectosApi.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : BaseController
    {
        private readonly IConfiguration _configuration;
        public TokenController(IConfiguration configuration, IServiceProvider serviceProvider, IOptions<SectionConfiguration> sectionConfigurationOption)
            : base(serviceProvider, sectionConfigurationOption)
        {
            _configuration = configuration;
                
        }

        /// <summary>
        /// Token authentication by credentials.
        /// </summary>
        /// <param name="filters">Filters to apply.</param>
        /// <returns></returns>
        /// <response code="200">Succes, retrieve the token.</response>
        /// <response code="404">No Content</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Authentication")]
        public IActionResult Authentication(TokenRequest tokenRequest)
        {
            if (ValidateUser(tokenRequest.User, tokenRequest.Password))
            {
                var token = GenerateToken(tokenRequest);
                return Ok(new { token = token });
            }

            return BadRequest(new { errorMessage = "Invalid user" });
        }

        private bool ValidateUser(string User, string Password)
        {
            var authentication = _tokenService.Authentication(_encrytpionService.Decrypt(User), _encrytpionService.Decrypt(Password));

            return authentication;
        }

        private string GenerateToken(TokenRequest User)
        {
            var rol = _tokenService.GetRolByUser(_encrytpionService.Decrypt(User.User));
            // Header
            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            // Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, rol.Result.Nombres),
                new Claim(ClaimTypes.Email, rol.Result.Email),
                new Claim(ClaimTypes.Role, rol.Result.Rol),
            };

            // Payload 
            var payload = new JwtPayload
            (
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(int.Parse(_configuration["SectionConfiguration:DuracionEnMinutosToken"]))
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
