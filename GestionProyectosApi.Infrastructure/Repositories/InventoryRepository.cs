using Dapper;
using GestionProyectosApi.Domain.Models.Generico.SP;
using GestionProyectosApi.Infrastructure.Repositories._UnitOfWork;
using GestionProyectosApi.Infrastructure.Repositories.Interfaces;

namespace GestionProyectosApi.Infrastructure.Repositories
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
