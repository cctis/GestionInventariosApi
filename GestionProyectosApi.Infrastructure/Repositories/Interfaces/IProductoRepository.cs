using GestionInventariosApi.Domain.Models.Generico.SP;

namespace GestionInventariosApi.Infrastructure.Repositories.Interfaces
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
