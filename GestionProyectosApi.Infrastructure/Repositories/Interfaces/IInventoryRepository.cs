using GestionInventariosApi.Domain.Models.Generico.SP;

namespace GestionInventariosApi.Infrastructure.Repositories.Interfaces
{
    public interface IInventoryRepository
    {
        decimal GetValorTotalInventario();
        List<ValorInventarioPorCategoriaDto> GetValorPorCategoria();
        List<ProductoStockCriticoDto> GetProductosStockCritico();
        decimal GetPorcentajeOcupacion();


    }
}
