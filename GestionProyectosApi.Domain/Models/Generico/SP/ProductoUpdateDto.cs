namespace GestionProyectosApi.Domain.Models.Generico.SP
{
    public class ProductoUpdateDto
    {
        public string? Nombre { get; set; }
        public string? Sku { get; set; }
        public string? Descripcion { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public int? Stock { get; set; }
        public int? StockMinimo { get; set; }
        public int? CapacidadMaxima { get; set; }
        public int? CategoriaId { get; set; }
        public int? EstadoProductoId { get; set; }
    }
}
