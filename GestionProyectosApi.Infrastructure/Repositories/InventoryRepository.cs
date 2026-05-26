using Dapper;
using GestionInventariosApi.Domain.Models.Generico.SP;
using GestionInventariosApi.Infrastructure.Repositories._UnitOfWork;
using GestionInventariosApi.Infrastructure.Repositories.Interfaces;

namespace GestionInventariosApi.Infrastructure.Repositories
{
    public class InventoryRepository : Repository, IInventoryRepository
    {
        

        public InventoryRepository() { }
        public InventoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        #region Métodos de Reporte

        public decimal GetValorTotalInventario()
        {
            return GetDataOfProcedure<decimal>("Sp_QueryGetTotalInventoryValue");
        }

        public List<ValorInventarioPorCategoriaDto> GetValorPorCategoria()
        {
            return GetDataListOfProcedure<ValorInventarioPorCategoriaDto>("Sp_QueryGetInventoryValueByCategory");
        }

        public List<ProductoStockCriticoDto> GetProductosStockCritico()
        {
            return GetDataListOfProcedure<ProductoStockCriticoDto>("Sp_QueryGetCriticalStockProducts");
        }

        public decimal GetPorcentajeOcupacion()
        {
            return GetDataOfProcedure<decimal>("Sp_QueryGetInventoryOccupationPercentage");
        }

        #endregion
    }
}
