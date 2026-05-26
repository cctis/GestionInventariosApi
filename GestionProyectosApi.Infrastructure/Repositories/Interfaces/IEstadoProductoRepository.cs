using GestionInventariosApi.Domain.Models.Generico.SP;

namespace GestionInventariosApi.Infrastructure.Repositories.Interfaces
{
    public interface IEstadoProductoRepository
    {
        List<EstadoProductoResponseDto> GetAll();
        EstadoProductoResponseDto GetById(int id);
        bool Create(EstadoProductoCreateDto dto);
        bool Update(int id, EstadoProductoUpdateDto dto);
        bool Delete(int id);


    }
}
