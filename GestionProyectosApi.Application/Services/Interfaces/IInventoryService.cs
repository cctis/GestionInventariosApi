using GestionProyectosApi.Domain.Models.Dto;
using GestionProyectosApi.Domain.Models.Generico.SP;

namespace GestionProyectosApi.Application.Services.Interfaces
{
    public interface IInventoryService
    {
        ResultOperation<InventorySummaryDto> GetSummary();

    }
}
