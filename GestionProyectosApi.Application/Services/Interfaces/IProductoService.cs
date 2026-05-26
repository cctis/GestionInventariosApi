using GestionProyectosApi.Domain.Models.Dto;
using GestionProyectosApi.Domain.Models.Generico.SP;

namespace GestionProyectosApi.Application.Services.Interfaces
{
    public interface IProductoService
    {
        ResultOperation<List<ProductoResponseDto>> GetAll();
        ResultOperation<ProductoResponseDto> GetById(int id);
        ResultOperation<bool> Create(ProductoCreateDto dto);
        ResultOperation<bool> Update(int id, ProductoUpdateDto dto);
        ResultOperation<bool> Delete(int id);

    }
}
