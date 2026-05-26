using GestionProyectosApi.Domain.Models.Dto;
using GestionProyectosApi.Domain.Models.Generico.SP;
using GestionProyectosApi.Domain.Entities.CustomEntities;

namespace GestionProyectosApi.Application.Services.Interfaces
{
    public interface IProductoService
    {
        ResultOperation<PagedList<ProductoResponseDto>> GetAll(ProductoFilterDto filter);
        ResultOperation<ProductoResponseDto> GetById(int id);
        ResultOperation<bool> Create(ProductoCreateDto dto);
        ResultOperation<bool> Update(int id, ProductoUpdateDto dto);
        ResultOperation<bool> Delete(int id);

    }
}
