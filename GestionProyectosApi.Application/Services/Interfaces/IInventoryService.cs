using GestionInventariosApi.Domain.Models.Dto;
using GestionInventariosApi.Domain.Models.Generico.SP;

namespace GestionInventariosApi.Application.Services.Interfaces
{
    public interface IInventoryService
    {
        ResultOperation<InventorySummaryDto> GetSummary();

    }
}
