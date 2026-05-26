using GestionInventariosApi.Application.Services.Interfaces;
using GestionInventariosApi.Domain.Models;
using GestionInventariosApi.Domain.Models.Dto;
using GestionInventariosApi.Domain.Models.Generico.SP;
using GestionInventariosApi.Infrastructure.Repositories;
using GestionInventariosApi.Infrastructure.Repositories._UnitOfWork;
using GestionInventariosApi.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Options;

namespace GestionInventariosApi.Application.Services
{
    public class InventoryService : _Service, IInventoryService
    {
        private readonly IInventoryRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public InventoryService(IOptions<ConnectionStrings> connectionStrings) : base(connectionStrings.Value.ConnetionGestionInventario)
        {

        }

        public InventoryService(IInventoryRepository repository, IUnitOfWork unitOfWork) : base(string.Empty)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        private TResult Execute<TResult>(Func<IInventoryRepository, IUnitOfWork, TResult> operation)
            where TResult : ResultOperation, new()
        {
            if (_repository != null && _unitOfWork != null)
            {
                return WrapExecuteTrans(_repository, _unitOfWork, operation);
            }

            return WrapExecuteTrans<TResult, InventoryRepository>((repo, uow) => operation(repo, uow));
        }

        public ResultOperation<InventorySummaryDto> GetSummary()
        {
            var result = Execute<ResultOperation<InventorySummaryDto>>((repo, uow) =>
            {
                var rst = new ResultOperation<InventorySummaryDto>();

                try
                {
                    var summary = new InventorySummaryDto
                    {
                        ValorTotalInventario = repo.GetValorTotalInventario(),
                        ValorPorCategoria = repo.GetValorPorCategoria() ?? new(),
                        ProductosStockCritico = repo.GetProductosStockCritico() ?? new(),
                        PorcentajeOcupacion = repo.GetPorcentajeOcupacion()
                    };

                    rst.Result = summary;
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
    }
}
