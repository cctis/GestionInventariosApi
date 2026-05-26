using GestionProyectosApi.Domain.Models;
using GestionProyectosApi.Domain.Models.Dto;
using GestionProyectosApi.Domain.Models.Generico.SP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GestionProyectosApi.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false)]

    public class InventoryController : BaseController
    {
        private readonly IConfiguration _configuration;
        public InventoryController(IServiceProvider serviceProvider, IOptions<SectionConfiguration> configuration, IConfiguration configurationSettings)
            : base(serviceProvider, configuration)
        {
            _configuration = configurationSettings;
        }

        [HttpGet("summary")]
        [ProducesResponseType(200, Type = typeof(InventorySummaryDto))]
        public ActionResult<ResultOperation<InventorySummaryDto>> GetSummary([FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
                ValidateRole(token, "Administrador");

                var response = _inventoryService.GetSummary();

                if (!response.stateOperation)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }

                if (!string.IsNullOrEmpty(response.MessageResult))
                {
                    return Ok(response.MessageResult);
                }

                return Ok(response.Result);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (UnauthorizedAccessException ex) { return Unauthorized(ex.Message); }
            catch (SecurityTokenException ex) { return Unauthorized($"Token inválido: {ex.Message}"); }
            catch (Exception ex) { return BadRequest($"Error al procesar el reporte: {ex.Message}"); }
        }
    }
}
