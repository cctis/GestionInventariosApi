using GestionProyectosApi.Domain.Models.Generico.SP;

namespace GestionProyectosApi.Infrastructure.Repositories.Interfaces
{
    public interface IProductoRepository
    {
        List<ProductoResponseDto> GetAll();
        ProductoResponseDto GetById(int id);
        bool Create(ProductoCreateDto dto);
        bool Update(int id, ProductoUpdateDto dto);
        bool Delete(int id);


    }
}
