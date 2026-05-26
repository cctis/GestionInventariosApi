using GestionInventariosApi.Domain.Models.Dto;
using GestionInventariosApi.Domain.Models.Generico.SP;

namespace GestionInventariosApi.Application.Services.Interfaces
{
    public interface ICategoriaService
    {
        ResultOperation<List<CategoriaResponseDto>> GetAll();
        ResultOperation<CategoriaResponseDto> GetById(int id);
        ResultOperation<bool> Create(CategoriaCreateDto dto);
        ResultOperation<bool> Update(int id, CategoriaUpdateDto dto);
        ResultOperation<bool> Delete(int id);

    }
}
