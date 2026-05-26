using GestionProyectosApi.Application.Services;
using GestionProyectosApi.Domain.Models.Generico.SP;
using GestionProyectosApi.Infrastructure.Repositories._UnitOfWork;
using GestionProyectosApi.Infrastructure.Repositories.Interfaces;
using Moq;

namespace GestionProyectosApi.Tests.Services;

public class InventoryServiceTests
{
    private readonly Mock<IInventoryRepository> _repository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    private InventoryService CreateService() => new(_repository.Object, _unitOfWork.Object);

    [Fact]
    public void GetSummary_ComposesTotalsAndConvertsNullCollectionsToEmpty()
    {
        _repository.Setup(repo => repo.GetValorTotalInventario()).Returns(1530.50m);
        _repository.Setup(repo => repo.GetValorPorCategoria())
            .Returns((List<ValorInventarioPorCategoriaDto>)null!);
        _repository.Setup(repo => repo.GetProductosStockCritico())
            .Returns((List<ProductoStockCriticoDto>)null!);
        _repository.Setup(repo => repo.GetPorcentajeOcupacion()).Returns(65.25m);

        var result = CreateService().GetSummary();

        Assert.True(result.stateOperation);
        Assert.Equal(1530.50m, result.Result.ValorTotalInventario);
        Assert.Equal(65.25m, result.Result.PorcentajeOcupacion);
        Assert.Empty(result.Result.ValorPorCategoria);
        Assert.Empty(result.Result.ProductosStockCritico);
    }

    [Fact]
    public void GetSummary_WhenRepositoryFails_RollsBackUnitOfWork()
    {
        _repository.Setup(repo => repo.GetValorTotalInventario())
            .Throws(new InvalidOperationException("fallo"));

        var result = CreateService().GetSummary();

        Assert.False(result.stateOperation);
        Assert.True(result.RollBack);
        Assert.Null(result.Result);
        _unitOfWork.Verify(uow => uow.Rollback(), Times.Once);
    }
}
