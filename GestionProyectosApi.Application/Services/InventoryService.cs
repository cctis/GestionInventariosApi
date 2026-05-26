using GestionProyectosApi.Application.Services.Interfaces;
using GestionProyectosApi.Domain.Models;
using GestionProyectosApi.Domain.Models.Dto;
using GestionProyectosApi.Domain.Models.Generico.SP;
using GestionProyectosApi.Infrastructure.Repositories;
using Microsoft.Extensions.Options;

namespace GestionProyectosApi.Application.Services
{
    public class InventoryService : _Service, IInventoryService
    {
        public InventoryService(IOptions<ConnectionStrings> connectionStrings) : base(connectionStrings.Value.ConnetionGestionInventario)
        {

        }

        public ResultOperation<InventorySummaryDto> GetSummary()
        {
            var result = WrapExecuteTrans<ResultOperation<InventorySummaryDto>, InventoryRepository>((repo, uow) =>
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
