using GestionInventariosApi.Domain.Models;
using GestionInventariosApi.Domain.Models.Dto;
using GestionInventariosApi.Domain.Models.Generico.SP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GestionInventariosApi.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false)]

    public class CategoriasController : BaseController
    {
        private readonly IConfiguration _configuration;
        public CategoriasController(IServiceProvider serviceProvider, IOptions<SectionConfiguration> configuration, IConfiguration configurationSettings)
            : base(serviceProvider, configuration)
        {
            _configuration = configurationSettings;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CategoriaResponseDto>))]
        public ActionResult<ResultOperation<List<CategoriaResponseDto>>> GetAll([FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
                ValidateRole(token, "Administrador");

                var response = _categoriaService.GetAll();

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
            catch (Exception ex) { return BadRequest($"Error al procesar la solicitud: {ex.Message}"); }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CategoriaResponseDto))]
        public ActionResult<ResultOperation<CategoriaResponseDto>> GetById([FromRoute] int id, [FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
                ValidateRole(token, "Administrador");

                var response = _categoriaService.GetById(id);
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
            catch (Exception ex) { return BadRequest($"Error al procesar la solicitud: {ex.Message}"); }
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(bool))]
        public ActionResult<ResultOperation<bool>> Create([FromBody] CategoriaCreateDto dto, [FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
                ValidateRole(token, "Administrador");

                var response = _categoriaService.Create(dto);
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
            catch (Exception ex) { return BadRequest($"Error al procesar la solicitud: {ex.Message}"); }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public ActionResult<ResultOperation<bool>> Update([FromRoute] int id, [FromBody] CategoriaUpdateDto dto, [FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
                ValidateRole(token, "Administrador");

                var response = _categoriaService.Update(id, dto);
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
            catch (Exception ex) { return BadRequest($"Error al procesar la solicitud: {ex.Message}"); }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public ActionResult<ResultOperation<bool>> Delete([FromRoute] int id, [FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
                ValidateRole(token, "Administrador");

                var response = _categoriaService.Delete(id);
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
            catch (Exception ex) { return BadRequest($"Error al procesar la solicitud: {ex.Message}"); }
        }
    
}
}
