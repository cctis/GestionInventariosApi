namespace GestionInventariosApi.Domain.Models.Generico.SP
{
    public class ProductoStockCriticoDto
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
    }
}
