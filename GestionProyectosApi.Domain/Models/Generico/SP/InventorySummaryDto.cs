namespace GestionInventariosApi.Domain.Models.Generico.SP
{
    public class InventorySummaryDto
    {
        public decimal ValorTotalInventario { get; set; }
        public List<ValorInventarioPorCategoriaDto> ValorPorCategoria { get; set; } = new();
        public List<ProductoStockCriticoDto> ProductosStockCritico { get; set; } = new();
        public decimal PorcentajeOcupacion { get; set; }
    }
}
