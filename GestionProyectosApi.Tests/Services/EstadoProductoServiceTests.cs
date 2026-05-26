using GestionInventariosApi.Application.Services;
using GestionInventariosApi.Domain.Models.Generico.SP;
using GestionInventariosApi.Infrastructure.Repositories._UnitOfWork;
using GestionInventariosApi.Infrastructure.Repositories.Interfaces;
using Moq;

namespace GestionInventariosApi.Tests.Services;

public class EstadoProductoServiceTests
{
    private readonly Mock<IEstadoProductoRepository> _repository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    private EstadoProductoService CreateService() => new(_repository.Object, _unitOfWork.Object);

    [Fact]
    public void GetAll_WhenStatesExist_ReturnsStates()
    {
        var states = new List<EstadoProductoResponseDto>
        {
            new() { Id = 1, Nombre = "Disponible" }
        };
        _repository.Setup(repo => repo.GetAll()).Returns(states);

        var result = CreateService().GetAll();

        Assert.True(result.stateOperation);
        Assert.Same(states, result.Result);
    }

    [Fact]
    public void GetById_WhenStateDoesNotExist_ReturnsBusinessMessage()
    {
        _repository.Setup(repo => repo.GetById(15)).Returns((EstadoProductoResponseDto)null!);

        var result = CreateService().GetById(15);

        Assert.True(result.stateOperation);
        Assert.Null(result.Result);
        Assert.Equal("El estado solicitado no existe", result.MessageResult);
    }

    [Fact]
    public void Update_WithSwaggerPlaceholder_PreservesExistingName()
    {
        _repository.Setup(repo => repo.GetById(2))
            .Returns(new EstadoProductoResponseDto { Id = 2, Nombre = "Disponible" });
        EstadoProductoUpdateDto? sentToRepository = null;
        _repository.Setup(repo => repo.Update(2, It.IsAny<EstadoProductoUpdateDto>()))
            .Callback<int, EstadoProductoUpdateDto>((_, dto) => sentToRepository = dto)
            .Returns(true);

        var result = CreateService().Update(2, new EstadoProductoUpdateDto { Nombre = "string" });

        Assert.True(result.stateOperation);
        Assert.Equal("Disponible", sentToRepository!.Nombre);
    }

    [Fact]
    public void Create_WhenRepositoryCreatesState_ReturnsTrue()
    {
        var dto = new EstadoProductoCreateDto { Nombre = "Agotado" };
        _repository.Setup(repo => repo.Create(dto)).Returns(true);

        var result = CreateService().Create(dto);

        Assert.True(result.stateOperation);
        Assert.True(result.Result);
    }

    [Fact]
    public void Delete_WhenRepositoryFails_RollsBackUnitOfWork()
    {
        _repository.Setup(repo => repo.Delete(3)).Throws(new InvalidOperationException("fallo"));

        var result = CreateService().Delete(3);

        Assert.False(result.stateOperation);
        Assert.True(result.RollBack);
        _unitOfWork.Verify(uow => uow.Rollback(), Times.Once);
    }
}
