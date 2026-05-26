namespace GestionInventariosApi.Domain.Models.Generico.SP
{
    public class ProductoCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; } = 5; 
        public int CapacidadMaxima { get; set; } = 100; 
        public int CategoriaId { get; set; }
        public int EstadoProductoId { get; set; }
    }
}
