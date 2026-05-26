using GestionProyectosApi.Domain.Models.Generico.SP;

namespace GestionProyectosApi.Infrastructure.Repositories.Interfaces
{
    public interface IInventoryRepository
    {
        decimal GetValorTotalInventario();
        List<ValorInventarioPorCategoriaDto> GetValorPorCategoria();
        List<ProductoStockCriticoDto> GetProductosStockCritico();
        decimal GetPorcentajeOcupacion();


    }
}
