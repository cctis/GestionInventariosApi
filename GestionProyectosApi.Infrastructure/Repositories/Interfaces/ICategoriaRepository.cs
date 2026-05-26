using GestionInventariosApi.Domain.Models.Generico.SP;

namespace GestionInventariosApi.Infrastructure.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        List<CategoriaResponseDto> GetAll();
        CategoriaResponseDto GetById(int id);
        bool Create(CategoriaCreateDto dto);
        bool Update(int id, CategoriaUpdateDto dto);
        bool Delete(int id);


    }
}
