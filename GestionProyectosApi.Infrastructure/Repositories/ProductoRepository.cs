using Dapper;
using GestionInventariosApi.Domain.Models.Generico.SP;
using GestionInventariosApi.Infrastructure.Repositories._UnitOfWork;
using GestionInventariosApi.Infrastructure.Repositories.Interfaces;

namespace GestionInventariosApi.Infrastructure.Repositories
{
    public class ProductoRepository : Repository, IProductoRepository
    {
        

        public ProductoRepository() { }
        public ProductoRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        #region READ

        public List<ProductoResponseDto> GetAll()
        {
            var response = GetDataListOfProcedure<ProductoResponseDto>("Sp_QueryGetProductos");
            return response;
        }

        public ProductoResponseDto GetById(int id)
        {
            var prms = new DynamicParameters();
            prms.Add("Id", id);

            var response = GetDataOfProcedure<ProductoResponseDto>("Sp_QueryGetProductoById", prms);
            return response;
        }

        #endregion

        #region CREATE

        public bool Create(ProductoCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Nombre)) throw new ArgumentException("El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(dto.Sku)) throw new ArgumentException("El SKU es obligatorio.");

            // =========================================================================
            // VALIDACIÓN DE NEGOCIO 1: Stock no negativo
            // =========================================================================
            if (dto.Stock < 0)
                throw new ArgumentException("El stock inicial no puede ser un número negativo.");

            if (dto.StockMinimo < 0)
                throw new ArgumentException("El stock mínimo no puede ser un número negativo.");

            // =========================================================================
            // VALIDACIÓN DE NEGOCIO 2: SKU único en la base de datos
            // =========================================================================
            var checkParams = new DynamicParameters();
            checkParams.Add("Sku", dto.Sku.Trim());

            // Usamos el método GetExists
            bool elSkuYaExiste = GetExists("Productos", "Sku = @Sku", checkParams);

            if (elSkuYaExiste)
                throw new ArgumentException($"El SKU '{dto.Sku.Trim()}' ya se encuentra registrado en otro producto.");
            // =========================================================================

            var prms = new DynamicParameters();
            prms.Add("Nombre", dto.Nombre.Trim());
            prms.Add("Sku", dto.Sku.Trim());
            prms.Add("Descripcion", dto.Descripcion?.Trim());
            prms.Add("PrecioUnitario", dto.PrecioUnitario);
            prms.Add("Stock", dto.Stock);
            prms.Add("StockMinimo", dto.StockMinimo);
            prms.Add("CapacidadMaxima", dto.CapacidadMaxima);
            prms.Add("CategoriaId", dto.CategoriaId);
            prms.Add("EstadoProductoId", dto.EstadoProductoId);

            var response = Execute<int>("Sp_QueryPostProducto", prms);
            return response > 0;
        }

        #endregion

        #region UPDATE

        public bool Update(int id, ProductoUpdateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var prms = new DynamicParameters();
            prms.Add("Id", id);

            if (!string.IsNullOrWhiteSpace(dto.Nombre)) prms.Add("Nombre", dto.Nombre.Trim());
            if (!string.IsNullOrWhiteSpace(dto.Sku)) prms.Add("Sku", dto.Sku.Trim());
            if (dto.Descripcion != null) prms.Add("Descripcion", dto.Descripcion.Trim());
            if (dto.PrecioUnitario.HasValue) prms.Add("PrecioUnitario", dto.PrecioUnitario.Value);
            if (dto.Stock.HasValue) prms.Add("Stock", dto.Stock.Value);
            if (dto.StockMinimo.HasValue) prms.Add("StockMinimo", dto.StockMinimo.Value);
            if (dto.CapacidadMaxima.HasValue) prms.Add("CapacidadMaxima", dto.CapacidadMaxima.Value);
            if (dto.CategoriaId.HasValue) prms.Add("CategoriaId", dto.CategoriaId.Value);
            if (dto.EstadoProductoId.HasValue) prms.Add("EstadoProductoId", dto.EstadoProductoId.Value);

            var response = Execute<int>("Sp_QueryUpdateProducto", prms);
            return response > 0;
        }

        #endregion

        #region DELETE

        public bool Delete(int id)
        {
            var prms = new DynamicParameters();
            prms.Add("Id", id);

            var response = Execute<int>("Sp_QueryDeleteProducto", prms);
            return response > 0;
        }

        #endregion
    }
}
