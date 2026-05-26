using GestionProyectosApi.Domain.Models;
using GestionProyectosApi.Domain.Models.Dto;
using GestionProyectosApi.Domain.Entities.CustomEntities;
using GestionProyectosApi.Domain.Models.Generico.SP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

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
        [ProducesResponseType(200, Type = typeof(PagedList<ProductoResponseDto>))]
        public ActionResult<ResultOperation<PagedList<ProductoResponseDto>>> GetAll([FromQuery] ProductoFilterDto filter, [FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
                ValidateRole(token, "Administrador");

                var response = _productoService.GetAll(filter);

                if (!response.stateOperation)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }

                var metadata = new
                {
                    response.Result.TotalCount,
                    response.Result.PageSize,
                    response.Result.CurrentPage,
                    response.Result.TotalPages,
                    response.Result.HasNextPage,
                    response.Result.HasPreviousPage
                };

                Response.Headers["Pagination"] = JsonSerializer.Serialize(metadata);
                Response.Headers["Access-Control-Expose-Headers"] = "Pagination";

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
