using GestionInventariosApi.Domain.Models.Dto;
using GestionInventariosApi.Domain.Models.Generico.SP;

namespace GestionInventariosApi.Application.Services.Interfaces
{
    public interface IEstadoProductoService
    {
        ResultOperation<List<EstadoProductoResponseDto>> GetAll();
        ResultOperation<EstadoProductoResponseDto> GetById(int id);
        ResultOperation<bool> Create(EstadoProductoCreateDto dto);
        ResultOperation<bool> Update(int id, EstadoProductoUpdateDto dto);
        ResultOperation<bool> Delete(int id);

    }
}
