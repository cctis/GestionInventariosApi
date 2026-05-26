using GestionProyectosApi.Application.Services;
using GestionProyectosApi.Domain.Models.Generico.SP;
using GestionProyectosApi.Infrastructure.Repositories._UnitOfWork;
using GestionProyectosApi.Infrastructure.Repositories.Interfaces;
using Moq;

namespace GestionProyectosApi.Tests.Services;

public class ProductoServiceTests
{
    private readonly Mock<IProductoRepository> _repository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    private ProductoService CreateService() => new(_repository.Object, _unitOfWork.Object);

    [Fact]
    public void GetAll_WhenProductsExist_ReturnsRepositoryResults()
    {
        var products = new List<ProductoResponseDto>
        {
            new() { Id = 1, Nombre = "Monitor", Sku = "MON-01" }
        };
        _repository.Setup(repo => repo.GetAll()).Returns(products);

        var result = CreateService().GetAll();

        Assert.True(result.stateOperation);
        Assert.Same(products, result.Result);
        Assert.Null(result.MessageResult);
    }

    [Fact]
    public void Create_WhenRepositoryCreatesProduct_ReturnsTrue()
    {
        var dto = new ProductoCreateDto { Nombre = "Monitor", Sku = "MON-01" };
        _repository.Setup(repo => repo.Create(dto)).Returns(true);

        var result = CreateService().Create(dto);

        Assert.True(result.stateOperation);
        Assert.True(result.Result);
    }

    [Fact]
    public void Update_WithEmptyOrZeroValues_PreservesPersistedValues()
    {
        var current = new ProductoResponseDto
        {
            Nombre = "Teclado",
            Sku = "TEC-01",
            Descripcion = "Mecanico",
            PrecioUnitario = 120,
            Stock = 12,
            StockMinimo = 3,
            CapacidadMaxima = 40,
            CategoriaId = 5,
            EstadoProductoId = 1
        };
        var update = new ProductoUpdateDto
        {
            Nombre = "string",
            Sku = "",
            Descripcion = " ",
            PrecioUnitario = 0,
            Stock = 0,
            StockMinimo = null,
            CapacidadMaxima = 0,
            CategoriaId = 0,
            EstadoProductoId = null
        };
        ProductoUpdateDto? sentToRepository = null;
        _repository.Setup(repo => repo.GetById(8)).Returns(current);
        _repository.Setup(repo => repo.Update(8, It.IsAny<ProductoUpdateDto>()))
            .Callback<int, ProductoUpdateDto>((_, dto) => sentToRepository = dto)
            .Returns(true);

        var result = CreateService().Update(8, update);

        Assert.True(result.stateOperation);
        Assert.True(result.Result);
        Assert.Equal("Teclado", sentToRepository!.Nombre);
        Assert.Equal("TEC-01", sentToRepository.Sku);
        Assert.Equal("Mecanico", sentToRepository.Descripcion);
        Assert.Equal(120, sentToRepository.PrecioUnitario);
        Assert.Equal(12, sentToRepository.Stock);
        Assert.Equal(3, sentToRepository.StockMinimo);
        Assert.Equal(40, sentToRepository.CapacidadMaxima);
        Assert.Equal(5, sentToRepository.CategoriaId);
        Assert.Equal(1, sentToRepository.EstadoProductoId);
    }

    [Fact]
    public void Update_WithNullDto_DoesNotCallRepository()
    {
        var result = CreateService().Update(1, null!);

        Assert.False(result.stateOperation);
        _repository.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Never);
        _repository.Verify(repo => repo.Update(It.IsAny<int>(), It.IsAny<ProductoUpdateDto>()), Times.Never);
    }

    [Fact]
    public void GetById_WhenRepositoryFails_RollsBackUnitOfWork()
    {
        _repository.Setup(repo => repo.GetById(1)).Throws(new InvalidOperationException("fallo"));

        var result = CreateService().GetById(1);

        Assert.False(result.stateOperation);
        Assert.True(result.RollBack);
        _unitOfWork.Verify(uow => uow.Rollback(), Times.Once);
    }

    [Fact]
    public void Delete_WhenRepositoryDeletesProduct_ReturnsTrue()
    {
        _repository.Setup(repo => repo.Delete(6)).Returns(true);

        var result = CreateService().Delete(6);

        Assert.True(result.stateOperation);
        Assert.True(result.Result);
    }
}
