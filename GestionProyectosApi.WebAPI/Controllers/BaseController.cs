using GestionProyectosApi.Application.Services.Interfaces;
using GestionProyectosApi.Domain.Models;
using GestionProyectosApi.Utils.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GestionProyectosApi.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BaseController : Controller
    {
        protected IServiceProvider _serviceProvider;
        protected readonly SectionConfiguration _config;
        protected readonly IEncryptionService _encrytpionService;
        protected readonly ITokenService _tokenService;
        protected readonly ICategoriaService _categoriaService;
        protected readonly IEstadoProductoService _estadoProductoService;
        protected readonly IProductoService _productoService;
        protected readonly IInventoryService _inventoryService;
       
      

        public BaseController(IServiceProvider serviceProvider, IOptions<SectionConfiguration> configuration)
        {
            _config = configuration.Value;
            _serviceProvider = serviceProvider;
            _tokenService = serviceProvider.GetService(typeof(ITokenService)) as ITokenService;
            _encrytpionService = serviceProvider.GetService(typeof(IEncryptionService)) as IEncryptionService;
            _categoriaService = serviceProvider.GetService(typeof(ICategoriaService)) as ICategoriaService;
            _estadoProductoService = serviceProvider.GetService(typeof(IEstadoProductoService)) as IEstadoProductoService;
            _productoService = serviceProvider.GetService(typeof(IProductoService)) as IProductoService;
            _inventoryService = serviceProvider.GetService(typeof(IInventoryService)) as IInventoryService;
           
        }

        #region Validaciones
        public static string ValidateAuthorizationHeader(string authorizationHeader)
        {
            if (string.IsNullOrWhiteSpace(authorizationHeader) ||
                !authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("El encabezado de autorización es inválido.");
            }

            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("El token de autorización está vacío.");
            }

            return token;
        }

        public static JwtSecurityToken ValidateToken(string tokenString, string secretKey)
        {
            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = symetricSecurityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(tokenString, validationParameters, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }

        public static void ValidateRole(JwtSecurityToken token, string requiredRole)
        {
            var rolClaim = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
            if (string.IsNullOrWhiteSpace(rolClaim))
            {
                throw new UnauthorizedAccessException("El token no contiene información de roles.");
            }

            if (!rolClaim.Equals(requiredRole, StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException("El rol actual no tiene permisos para realizar esta acción.");
            }
        }
        #endregion
    }
}
