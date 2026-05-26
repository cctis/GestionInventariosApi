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
    public class ProductoService : _Service, IProductoService
    {
        private readonly IProductoRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public ProductoService(IOptions<ConnectionStrings> connectionStrings) : base(connectionStrings.Value.ConnetionGestionInventario)
        {

        }

        public ProductoService(IProductoRepository repository, IUnitOfWork unitOfWork) : base(string.Empty)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        private TResult Execute<TResult>(Func<IProductoRepository, IUnitOfWork, TResult> operation)
            where TResult : ResultOperation, new()
        {
            if (_repository != null && _unitOfWork != null)
            {
                return WrapExecuteTrans(_repository, _unitOfWork, operation);
            }

            return WrapExecuteTrans<TResult, ProductoRepository>((repo, uow) => operation(repo, uow));
        }

        #region READ

        public ResultOperation<List<ProductoResponseDto>> GetAll()
        {
            var result = Execute<ResultOperation<List<ProductoResponseDto>>>((repo, uow) =>
            {
                var rst = new ResultOperation<List<ProductoResponseDto>>();

                try
                {
                    var list = repo.GetAll();

                    if (list == null || list.Count == 0)
                    {
                        rst.MessageResult = "No hay datos";
                        rst.Result = new List<ProductoResponseDto>();
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

        public ResultOperation<ProductoResponseDto> GetById(int id)
        {
            var result = Execute<ResultOperation<ProductoResponseDto>>((repo, uow) =>
            {
                var rst = new ResultOperation<ProductoResponseDto>();

                try
                {
                    var entity = repo.GetById(id);

                    if (entity == null)
                    {
                        rst.MessageResult = "El producto solicitado no existe";
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

        public ResultOperation<bool> Create(ProductoCreateDto dto)
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

        public ResultOperation<bool> Update(int id, ProductoUpdateDto dto)
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

                    // 1. Consultar el producto actual en base de datos
                    var actual = repo.GetById(id);
                    if (actual == null)
                    {
                        rst.stateOperation = false;
                        rst.MessageResult = "El producto a actualizar no existe";
                        return rst;
                    }

                    // 2. Rellenar/Mezclar solo lo que cambió de valores de texto y Swagger
                    if (string.IsNullOrWhiteSpace(dto.Nombre) || dto.Nombre.Trim().Equals("string", StringComparison.OrdinalIgnoreCase))
                    {
                        dto.Nombre = actual.Nombre;
                    }

                    if (string.IsNullOrWhiteSpace(dto.Sku) || dto.Sku.Trim().Equals("string", StringComparison.OrdinalIgnoreCase))
                    {
                        dto.Sku = actual.Sku;
                    }

                    if (string.IsNullOrWhiteSpace(dto.Descripcion) || dto.Descripcion.Trim().Equals("string", StringComparison.OrdinalIgnoreCase))
                    {
                        dto.Descripcion = actual.Descripcion;
                    }

                    // 3. Mezclar valores numéricos por defecto (Swagger 0 values protection)
                    if (!dto.PrecioUnitario.HasValue || (dto.PrecioUnitario.Value == 0 && actual.PrecioUnitario != 0))
                    {
                        dto.PrecioUnitario = actual.PrecioUnitario;
                    }

                    if (!dto.Stock.HasValue || (dto.Stock.Value == 0 && actual.Stock != 0))
                    {
                        dto.Stock = actual.Stock;
                    }

                    if (!dto.StockMinimo.HasValue || (dto.StockMinimo.Value == 0 && actual.StockMinimo != 0))
                    {
                        dto.StockMinimo = actual.StockMinimo;
                    }

                    if (!dto.CapacidadMaxima.HasValue || (dto.CapacidadMaxima.Value == 0 && actual.CapacidadMaxima != 0))
                    {
                        dto.CapacidadMaxima = actual.CapacidadMaxima;
                    }

                    if (!dto.CategoriaId.HasValue || (dto.CategoriaId.Value == 0 && actual.Categoria.Id != 0))
                    {
                        dto.CategoriaId = actual.Categoria.Id;
                    }

                    if (!dto.EstadoProductoId.HasValue || (dto.EstadoProductoId.Value == 0 && actual.EstadoProducto.Id != 0))
                    {
                        dto.EstadoProductoId = actual.EstadoProducto.Id;
                    }

                    // 4. Mandar el DTO reconstruido al repositorio
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
