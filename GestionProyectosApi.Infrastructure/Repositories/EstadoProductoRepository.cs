using Dapper;
using GestionProyectosApi.Domain.Models.Generico.SP;
using GestionProyectosApi.Infrastructure.Repositories._UnitOfWork;
using GestionProyectosApi.Infrastructure.Repositories.Interfaces;

namespace GestionProyectosApi.Infrastructure.Repositories
{
    public class EstadoProductoRepository : Repository, IEstadoProductoRepository
    {
        

        public EstadoProductoRepository() { }
        public EstadoProductoRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        #region READ

        public List<EstadoProductoResponseDto> GetAll()
        {
            var response = GetDataListOfProcedure<EstadoProductoResponseDto>("Sp_QueryGetEstadosProducto");
            return response;
        }

        public EstadoProductoResponseDto GetById(int id)
        {
            var prms = new DynamicParameters();
            prms.Add("Id", id);

            var response = GetDataOfProcedure<EstadoProductoResponseDto>("Sp_QueryGetEstadoProductoById", prms);
            return response;
        }

        #endregion

        #region CREATE

        public bool Create(EstadoProductoCreateDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Nombre))
            {
                throw new ArgumentException("El nombre del estado es obligatorio.");
            }

            var prms = new DynamicParameters();
            prms.Add("Nombre", dto.Nombre.Trim());

            var response = Execute<int>("Sp_QueryPostEstadoProducto", prms);
            return response > 0;
        }

        #endregion

        #region UPDATE

        public bool Update(int id, EstadoProductoUpdateDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Nombre))
            {
                throw new ArgumentException("El nombre del estado es obligatorio para actualizar.");
            }

            var prms = new DynamicParameters();
            prms.Add("Id", id);
            prms.Add("Nombre", dto.Nombre.Trim());

            var response = Execute<int>("Sp_QueryUpdateEstadoProducto", prms);
            return response > 0;
        }

        #endregion

        #region DELETE

        public bool Delete(int id)
        {
            var prms = new DynamicParameters();
            prms.Add("Id", id);

            var response = Execute<int>("Sp_QueryDeleteEstadoProducto", prms);
            return response > 0;
        }

        #endregion
    }
}
