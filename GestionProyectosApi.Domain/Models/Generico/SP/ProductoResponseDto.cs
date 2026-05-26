namespace GestionInventariosApi.Domain.Models.Generico.SP
{
    public class ProductoResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public int CapacidadMaxima { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        // ====== Propiedades planas auxiliares para el mapeo de Dapper ======
        private int _categoriaId;
        public int CategoriaId { get => _categoriaId; set { _categoriaId = value; Categoria.Id = value; } }

        private string _categoriaNombre = string.Empty;
        public string CategoriaNombre { get => _categoriaNombre; set { _categoriaNombre = value; Categoria.Nombre = value; } }

        private string? _categoriaDescripcion;
        public string? CategoriaDescripcion { get => _categoriaDescripcion; set { _categoriaDescripcion = value; Categoria.Descripcion = value; } }

        private bool _categoriaActivo;
        public bool CategoriaActivo { get => _categoriaActivo; set { _categoriaActivo = value; Categoria.Activo = value; } }

        private int _estadoProductoId;
        public int EstadoProductoId { get => _estadoProductoId; set { _estadoProductoId = value; EstadoProducto.Id = value; } }

        private string _estadoProductoNombre = string.Empty;
        public string EstadoProductoNombre { get => _estadoProductoNombre; set { _estadoProductoNombre = value; EstadoProducto.Nombre = value; } }
        // ===================================================================

        // Relaciones anidadas estructurales 
        public CategoriaResponseDto Categoria { get; set; } = new();
        public EstadoProductoResponseDto EstadoProducto { get; set; } = new();
    }
}
