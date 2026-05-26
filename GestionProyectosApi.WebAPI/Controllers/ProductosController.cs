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

    public class ProductosController : BaseController
    {
        private readonly IConfiguration _configuration;
        public ProductosController(IServiceProvider serviceProvider, IOptions<SectionConfiguration> configuration, IConfiguration configurationSettings)
            : base(serviceProvider, configuration)
        {
            _configuration = configurationSettings;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ProductoResponseDto>))]
        public ActionResult<ResultOperation<List<ProductoResponseDto>>> GetAll([FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
                ValidateRole(token, "Administrador");

                var response = _productoService.GetAll();

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
        [ProducesResponseType(200, Type = typeof(ProductoResponseDto))]
        public ActionResult<ResultOperation<ProductoResponseDto>> GetById([FromRoute] int id, [FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
                ValidateRole(token, "Administrador");

                var response = _productoService.GetById(id);
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
        public ActionResult<ResultOperation<bool>> Create([FromBody] ProductoCreateDto dto, [FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
                ValidateRole(token, "Administrador");

                var response = _productoService.Create(dto);
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
        public ActionResult<ResultOperation<bool>> Update([FromRoute] int id, [FromBody] ProductoUpdateDto dto, [FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
                ValidateRole(token, "Administrador");

                var response = _productoService.Update(id, dto);
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

                var response = _productoService.Delete(id);
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
