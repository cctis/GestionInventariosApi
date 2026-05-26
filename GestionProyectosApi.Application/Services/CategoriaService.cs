using GestionProyectosApi.Application.Services.Interfaces;
using GestionProyectosApi.Domain.Models;
using GestionProyectosApi.Domain.Models.Dto;
using GestionProyectosApi.Domain.Models.Generico.SP;
using GestionProyectosApi.Infrastructure.Repositories;
using GestionProyectosApi.Infrastructure.Repositories._UnitOfWork;
using GestionProyectosApi.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Options;

namespace GestionProyectosApi.Application.Services
{
    public class CategoriaService : _Service, ICategoriaService
    {
        private readonly ICategoriaRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public CategoriaService(IOptions<ConnectionStrings> connectionStrings) : base(connectionStrings.Value.ConnetionGestionInventario)
        {

        }

        public CategoriaService(ICategoriaRepository repository, IUnitOfWork unitOfWork) : base(string.Empty)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        private TResult Execute<TResult>(Func<ICategoriaRepository, IUnitOfWork, TResult> operation)
            where TResult : ResultOperation, new()
        {
            if (_repository != null && _unitOfWork != null)
            {
                return WrapExecuteTrans(_repository, _unitOfWork, operation);
            }

            return WrapExecuteTrans<TResult, CategoriaRepository>((repo, uow) => operation(repo, uow));
        }

        #region READ

        public ResultOperation<List<CategoriaResponseDto>> GetAll()
        {
            var result = Execute<ResultOperation<List<CategoriaResponseDto>>>((repo, uow) =>
            {
                var rst = new ResultOperation<List<CategoriaResponseDto>>();

                try
                {
                    var list = repo.GetAll();

                    if (list == null || list.Count == 0)
                    {
                        rst.MessageResult = "No hay datos";
                        rst.Result = new List<CategoriaResponseDto>();
                    }
                    else
                    {
                        rst.Result = list;
                    }

                    rst.stateOperation = true;
                }
                catch (Exception err)
                {
                    rst.RollBack = true;
                    rst.stateOperation = false;
                    rst.Result = null;
                    rst.MessageExceptionTechnical = $"{err.Message}{Environment.NewLine}{err.StackTrace}";
                }

                return rst;
            });

            return result;
        }

        public ResultOperation<CategoriaResponseDto> GetById(int id)
        {
            var result = Execute<ResultOperation<CategoriaResponseDto>>((repo, uow) =>
            {
                var rst = new ResultOperation<CategoriaResponseDto>();

                try
                {
                    var entity = repo.GetById(id);

                    if (entity == null)
                    {
                        rst.MessageResult = "La categoría solicitada no existe";
                        rst.stateOperation = true;
                    }
                    else
                    {
                        rst.Result = entity;
                        rst.stateOperation = true;
                    }
                }
                catch (Exception err)
                {
                    rst.RollBack = true;
                    rst.stateOperation = false;
                    rst.Result = null;
                    rst.MessageExceptionTechnical = $"{err.Message}{Environment.NewLine}{err.StackTrace}";
                }

                return rst;
            });

            return result;
        }

        #endregion

        #region POST

        public ResultOperation<bool> Create(CategoriaCreateDto dto)
        {
            var result = Execute<ResultOperation<bool>>((repo, uow) =>
            {
                var rst = new ResultOperation<bool>();

                try
                {
                    rst.Result = repo.Create(dto);
                    rst.stateOperation = true;
                }
                catch (Exception err)
                {
                    rst.RollBack = true;
                    rst.stateOperation = false;
                    rst.MessageExceptionTechnical = $"{err.Message}{Environment.NewLine}{err.StackTrace}";
                }

                return rst;
            });

            return result;
        }

        #endregion

        #region UPDATE

        public ResultOperation<bool> Update(int id, CategoriaUpdateDto dto)
        {
            var result = Execute<ResultOperation<bool>>((repo, uow) =>
            {
                var rst = new ResultOperation<bool>();

                try
                {
                    if (dto == null)
                    {
                        rst.stateOperation = false;
                        return rst;
                    }

                    // 1. Consultar el estado actual
                    var actual = repo.GetById(id);
                    if (actual == null)
                    {
                        rst.stateOperation = false;
                        rst.MessageResult = "La categoría a actualizar no existe";
                        return rst;
                    }

                    // 2. Limpiar valores por defecto de Swagger / Vacíos
                    if (string.IsNullOrWhiteSpace(dto.Nombre) || dto.Nombre.Trim().Equals("string", StringComparison.OrdinalIgnoreCase))
                    {
                        dto.Nombre = actual.Nombre;
                    }

                    if (string.IsNullOrWhiteSpace(dto.Descripcion) || dto.Descripcion.Trim().Equals("string", StringComparison.OrdinalIgnoreCase))
                    {
                        dto.Descripcion = actual.Descripcion;
                    }

                    if (!dto.Activo.HasValue)
                    {
                        dto.Activo = actual.Activo;
                    }

                    // 3. Ejecutar actualización
                    rst.Result = repo.Update(id, dto);
                    rst.stateOperation = true;
                }
                catch (Exception ex)
                {
                    rst.RollBack = true;
                    rst.stateOperation = false;
                    rst.MessageExceptionTechnical = $"{ex.Message}{Environment.NewLine}{ex.StackTrace}";
                }

                return rst;
            });

            return result;
        }

        #endregion

        #region DELETE

        public ResultOperation<bool> Delete(int id)
        {
            var result = Execute<ResultOperation<bool>>((repo, uow) =>
            {
                var rst = new ResultOperation<bool>();

                try
                {
                    rst.Result = repo.Delete(id);
                    rst.stateOperation = true;
                }
                catch (Exception err)
                {
                    rst.RollBack = true;
                    rst.stateOperation = false;
                    rst.MessageExceptionTechnical = $"{err.Message}{Environment.NewLine}{err.StackTrace}";
                }

                return rst;
            });

            return result;
        }

        #endregion
    }
}
