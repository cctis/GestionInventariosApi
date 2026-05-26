using GestionInventariosApi.Domain.Models.Dto;
using GestionInventariosApi.Domain.Models.Generico.SP;
using GestionInventariosApi.Domain.Entities.CustomEntities;

namespace GestionInventariosApi.Application.Services.Interfaces
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
