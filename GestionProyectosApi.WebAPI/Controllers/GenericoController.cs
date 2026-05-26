//using GestionProyectosApi.Domain.Models.Farma.SP;
//using GestionProyectosApi.Application.Services.Interfaces;
//using GestionProyectosApi.Domain.Models;
//using GestionProyectosApi.Domain.Models.Dto;
//using GestionProyectosApi.Domain.Models.Farma.Core;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using Models.Dto;
//using Newtonsoft.Json;
//using System.IdentityModel.Tokens.Jwt;
//using Microsoft.IdentityModel.Tokens;

//namespace GestionProyectosApi.WebAPI.Controllers
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    [ApiController]
//    [ApiExplorerSettings(IgnoreApi = true)]

//    public class GenericoController : BaseController
//    {
//        private readonly IConfiguration _configuration;
//        public GenericoController(IServiceProvider serviceProvider, IOptions<SectionConfiguration> configuration, IConfiguration configurationSettings)
//            : base(serviceProvider, configuration)
//        {
//            _configuration = configurationSettings;
//        }

//        #region POST
//        [HttpPost]
//        [Route("PostUsuarioAdministrativo")]
//        [ProducesResponseType(200, Type = typeof(bool))]
//        public ActionResult<ResultOperation<bool>> PostUsuarioAdministrativo([FromBody] UsuarioAdministrativoPost Usuario, [FromHeader] string Authorization)
//        {
//            try
//            {
//                var tokenString = ValidateAuthorizationHeader(Authorization);
//                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
//                ValidateRole(token, "Administrador");

//                if (Usuario == null)
//                {
//                    return BadRequest("El objeto Usuario no puede ser nulo.");
//                }

//                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Usuario.Contrasena);
//                string base64String = Convert.ToBase64String(bytes);

//                Usuario.Contrasena = base64String;

//                var response = _GenericoService.PostUsuarioAdministrativo(Usuario);
//                if (!response.stateOperation)
//                {
//                    return StatusCode(StatusCodes.Status500InternalServerError, response);
//                }

//                if (!string.IsNullOrEmpty(response.MessageResult))
//                {
//                    return Ok(response.MessageResult);
//                }

//                return Ok(response.Result);
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//            catch (UnauthorizedAccessException ex)
//            {
//                return Unauthorized(ex.Message);
//            }
//            catch (SecurityTokenException ex)
//            {
//                return Unauthorized($"Token inválido: {ex.Message}");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error al procesar la solicitud: {ex.Message}");
//            }

//        }
//        #endregion

//        #region GET
//        [HttpGet]
//        [Route("GetUsuarioAdministrativo")]
//        [ProducesResponseType(200, Type = typeof(GenericoResponse))]
//        public ActionResult<ResultOperation<GenericoResponse>> UsuarioAdministrativo([FromQuery] QueryFilter entity, [FromHeader] string Authorization)
//        {
//            try
//            {
//                var tokenString = ValidateAuthorizationHeader(Authorization);
//                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
//                ValidateRole(token, "Administrador");

//                var response = _GenericoService.GetUsuarioAdministrativo(entity);
//                if (!response.stateOperation)
//                {
//                    return StatusCode(StatusCodes.Status500InternalServerError, response);
//                }

//                if (!string.IsNullOrEmpty(response.MessageResult))
//                {
//                    return Ok(response.MessageResult);
//                }

//                var metadata = new
//                {
//                    response.Result.PagedUsuarioAdministrativo.TotalCount,
//                    response.Result.PagedUsuarioAdministrativo.PageSize,
//                    response.Result.PagedUsuarioAdministrativo.CurrentPage,
//                    response.Result.PagedUsuarioAdministrativo.TotalPages,
//                    response.Result.PagedUsuarioAdministrativo.HasNextPage,
//                    response.Result.PagedUsuarioAdministrativo.HasPreviousPage
//                };

//                Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metadata));

//                return Ok(response.Result.PagedUsuarioAdministrativo);
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//            catch (UnauthorizedAccessException ex)
//            {
//                return Unauthorized(ex.Message);
//            }
//            catch (SecurityTokenException ex)
//            {
//                return Unauthorized($"Token inválido: {ex.Message}");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error al procesar la solicitud: {ex.Message}");
//            }
//        }
//        #endregion

//        #region UPDATE
//        [HttpPatch]
//        [Route("UpdateUsuarioAdministrativo")]
//        [ProducesResponseType(200, Type = typeof(bool))]
//        public ActionResult<ResultOperation<bool>> UpdateUsuarioAdministrativo([FromBody] UsuarioAdministrativoUpdate Usuario, [FromHeader] string Authorization)
//        {
//            try
//            {
//                var tokenString = ValidateAuthorizationHeader(Authorization);
//                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
//                ValidateRole(token, "Administrador");

//                var response = _GenericoService.UpdateUsuarioAdministrativo(Usuario);
//                if (!response.stateOperation)
//                {
//                    return StatusCode(StatusCodes.Status500InternalServerError, response);
//                }

//                if (!string.IsNullOrEmpty(response.MessageResult))
//                {
//                    return Ok(response.MessageResult);
//                }

//                return Ok(response.Result);
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//            catch (UnauthorizedAccessException ex)
//            {
//                return Unauthorized(ex.Message);
//            }
//            catch (SecurityTokenException ex)
//            {
//                return Unauthorized($"Token inválido: {ex.Message}");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error al procesar la solicitud: {ex.Message}");
//            }
//        }
//        #endregion

//        #region DELETE
//        [HttpDelete]
//        [Route("DeleteUsuarioAdministrativo")]
//        [ProducesResponseType(200, Type = typeof(bool))]
//        public ActionResult<ResultOperation<bool>> DeleteUsuarioAdministrativo([FromBody] UsuarioAdministrativoDelete Usuario, [FromHeader] string Authorization)
//        {
//            try
//            {
//                var tokenString = ValidateAuthorizationHeader(Authorization);
//                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
//                ValidateRole(token, "Administrador");

//                var response = _GenericoService.DeleteUsuarioAdministrativo(Usuario.IdUsuario);
//                if (!response.stateOperation)
//                {
//                    return StatusCode(StatusCodes.Status500InternalServerError, response);
//                }

//                if (!string.IsNullOrEmpty(response.MessageResult))
//                {
//                    return Ok(response.MessageResult);
//                }

//                return Ok(response.Result);
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//            catch (UnauthorizedAccessException ex)
//            {
//                return Unauthorized(ex.Message);
//            }
//            catch (SecurityTokenException ex)
//            {
//                return Unauthorized($"Token inválido: {ex.Message}");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error al procesar la solicitud: {ex.Message}");
//            }            
//        }
//        #endregion
//    }
//}
