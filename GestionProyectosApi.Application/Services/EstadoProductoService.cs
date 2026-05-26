using GestionProyectosApi.Application.Services.Interfaces;
using GestionProyectosApi.Domain.Models;
using GestionProyectosApi.Domain.Models.Dto;
using GestionProyectosApi.Domain.Models.Generico.SP;
using GestionProyectosApi.Infrastructure.Repositories;
using Microsoft.Extensions.Options;

namespace GestionProyectosApi.Application.Services
{
    public class EstadoProductoService : _Service, IEstadoProductoService
    {
        public EstadoProductoService(IOptions<ConnectionStrings> connectionStrings) : base(connectionStrings.Value.ConnetionGestionInventario)
        {

        }

        #region READ

        public ResultOperation<List<EstadoProductoResponseDto>> GetAll()
        {
            var result = WrapExecuteTrans<ResultOperation<List<EstadoProductoResponseDto>>, EstadoProductoRepository>((repo, uow) =>
            {
                var rst = new ResultOperation<List<EstadoProductoResponseDto>>();

                try
                {
                    var list = repo.GetAll();

                    if (list == null || list.Count == 0)
                    {
                        rst.MessageResult = "No hay datos";
                        rst.Result = new List<EstadoProductoResponseDto>();
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

        public ResultOperation<EstadoProductoResponseDto> GetById(int id)
        {
            var result = WrapExecuteTrans<ResultOperation<EstadoProductoResponseDto>, EstadoProductoRepository>((repo, uow) =>
            {
                var rst = new ResultOperation<EstadoProductoResponseDto>();

                try
                {
                    var entity = repo.GetById(id);

                    if (entity == null)
                    {
                        rst.MessageResult = "El estado solicitado no existe";
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

        public ResultOperation<bool> Create(EstadoProductoCreateDto dto)
        {
            var result = WrapExecuteTrans<ResultOperation<bool>, EstadoProductoRepository>((repo, uow) =>
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

        public ResultOperation<bool> Update(int id, EstadoProductoUpdateDto dto)
        {
            var result = WrapExecuteTrans<ResultOperation<bool>, EstadoProductoRepository>((repo, uow) =>
            {
                var rst = new ResultOperation<bool>();

                try
                {
                    if (dto == null)
                    {
                        rst.stateOperation = false;
                        return rst;
                    }

                    var actual = repo.GetById(id);
                    if (actual == null)
                    {
                        rst.stateOperation = false;
                        rst.MessageResult = "El estado a actualizar no existe";
                        return rst;
                    }

                    if (string.IsNullOrWhiteSpace(dto.Nombre) || dto.Nombre.Trim().Equals("string", StringComparison.OrdinalIgnoreCase))
                    {
                        dto.Nombre = actual.Nombre;
                    }

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
            var result = WrapExecuteTrans<ResultOperation<bool>, EstadoProductoRepository>((repo, uow) =>
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
