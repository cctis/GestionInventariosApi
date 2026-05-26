using Dapper;
using GestionInventariosApi.Domain.Models.Generico.SP;
using GestionInventariosApi.Infrastructure.Repositories._UnitOfWork;
using GestionInventariosApi.Infrastructure.Repositories.Interfaces;

namespace GestionInventariosApi.Infrastructure.Repositories
{
    public class CategoriaRepository : Repository, ICategoriaRepository
    {
        

        public CategoriaRepository() { }
        public CategoriaRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        #region READ

        public List<CategoriaResponseDto> GetAll()
        {
            var response = GetDataListOfProcedure<CategoriaResponseDto>("Sp_QueryGetCategorias");
            return response;
        }

        public CategoriaResponseDto GetById(int id)
        {
            var prms = new DynamicParameters();
            prms.Add("Id", id);

            var response = GetDataOfProcedure<CategoriaResponseDto>("Sp_QueryGetCategoriaById", prms);
            return response;
        }

        #endregion

        #region CREATE

        public bool Create(CategoriaCreateDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Nombre))
            {
                throw new ArgumentException("El nombre de la categoría es obligatorio.");
            }

            var prms = new DynamicParameters();
            prms.Add("Nombre", dto.Nombre.Trim());
            prms.Add("Descripcion", dto.Descripcion?.Trim());

            var response = Execute<int>("Sp_QueryPostCategoria", prms);
            return response > 0;
        }

        #endregion

        #region UPDATE

        public bool Update(int id, CategoriaUpdateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("Los datos de actualización no pueden ser nulos.");
            }

            var prms = new DynamicParameters();
            prms.Add("Id", id);

            if (!string.IsNullOrWhiteSpace(dto.Nombre))
            {
                prms.Add("Nombre", dto.Nombre.Trim());
            }

            if (dto.Descripcion != null)
            {
                prms.Add("Descripcion", dto.Descripcion.Trim());
            }

            if (dto.Activo.HasValue)
            {
                prms.Add("Activo", dto.Activo.Value);
            }

            var response = Execute<int>("Sp_QueryUpdateCategoria", prms);
            return response > 0;
        }

        #endregion

        #region DELETE

        public bool Delete(int id)
        {
            var prms = new DynamicParameters();
            prms.Add("Id", id);

            var response = Execute<int>("Sp_QueryDeleteCategoria", prms);
            return response > 0;
        }

        #endregion
    }
}
