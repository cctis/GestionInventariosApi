//using GestionProyectosApi.Domain.Models;
//using GestionProyectosApi.Domain.Models.Dto;
//using GestionProyectosApi.Domain.Models.Generico.SP;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;

//namespace GestionProyectosApi.WebAPI.Controllers
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    [ApiController]
//    [ApiExplorerSettings(IgnoreApi = false)]

//    public class ProjectsController : BaseController
//    {
//        private readonly IConfiguration _configuration;
//        public ProjectsController(IServiceProvider serviceProvider, IOptions<SectionConfiguration> configuration, IConfiguration configurationSettings)
//            : base(serviceProvider, configuration)
//        {
//            _configuration = configurationSettings;
//        }

//        [HttpGet]
//        [ProducesResponseType(200, Type = typeof(List<ProjectDto>))]
//        public ActionResult<ResultOperation<List<ProjectDto>>> GetAll([FromQuery] ProjectFiltersDto filters, [FromHeader] string Authorization)
//        {
//            try
//            {
//                var tokenString = ValidateAuthorizationHeader(Authorization);
//                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
//                ValidateRole(token, "Administrador");

//                var response = _ProjectService.GetAll(filters);

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


//        [HttpGet("{id}")]
//        [ProducesResponseType(200, Type = typeof(ProjectDto))]

//        public ActionResult<ResultOperation<ProjectDto>> GetById([FromRoute] int id, [FromHeader] string Authorization)
//        {
//            try
//            {
//                var tokenString = ValidateAuthorizationHeader(Authorization);
//                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
//                ValidateRole(token, "Administrador");


//                var response = _ProjectService.GetById(id);
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

//        [HttpGet("by-cell/{id}")]
//        [ProducesResponseType(200, Type = typeof(ProjectDto))]

//        public ActionResult<ResultOperation<ProjectDto>> GetAllByIdCell([FromRoute] int id, [FromHeader] string Authorization)
//        {
//            try
//            {
//                var tokenString = ValidateAuthorizationHeader(Authorization);
//                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
//                ValidateRole(token, "Administrador");



//                var response = _ProjectService.GetAllByIdCell(id);
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

//        [HttpPost]
//        [ProducesResponseType(200, Type = typeof(bool))]
//        public ActionResult<ResultOperation<bool>> Create([FromBody] CreateProjectDto dto, [FromHeader] string Authorization)
//        {
//            try
//            {
//                var tokenString = ValidateAuthorizationHeader(Authorization);
//                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
//                ValidateRole(token, "Administrador");


//                var response = _ProjectService.Create(dto);
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

//        [HttpPatch("{id}")]
//        [ProducesResponseType(200, Type = typeof(bool))]
//        public ActionResult<ResultOperation<bool>> Update([FromRoute] int id, [FromBody] UpdateProjectDto dto, [FromHeader] string Authorization)
//        {
//            try
//            {
//                var tokenString = ValidateAuthorizationHeader(Authorization);
//                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
//                ValidateRole(token, "Administrador");

//                //dto.Id = $"project-{id}";
//                dto.Id = id.ToString();
//                var response = _ProjectService.Update(dto);
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

//        [HttpDelete("{id}")]
//        [ProducesResponseType(200, Type = typeof(bool))]
//        public ActionResult<ResultOperation<bool>> Delete(int id, [FromHeader] string Authorization)
//        {
//            try
//            {
//                var tokenString = ValidateAuthorizationHeader(Authorization);
//                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"]);
//                ValidateRole(token, "Administrador");

//                var response = _ProjectService.Delete(id);
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
//    }
//}
