using GestionProyectosApi.Application.Services;
using GestionProyectosApi.Domain.Models.Generico.SP;
using GestionProyectosApi.Infrastructure.Repositories._UnitOfWork;
using GestionProyectosApi.Infrastructure.Repositories.Interfaces;
using Moq;

namespace GestionProyectosApi.Tests.Services;

public class CategoriaServiceTests
{
    private readonly Mock<ICategoriaRepository> _repository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    private CategoriaService CreateService() => new(_repository.Object, _unitOfWork.Object);

    [Fact]
    public void GetAll_WhenThereAreNoCategories_ReturnsSuccessfulEmptyResult()
    {
        _repository.Setup(repo => repo.GetAll()).Returns(new List<CategoriaResponseDto>());

        var result = CreateService().GetAll();

        Assert.True(result.stateOperation);
        Assert.Empty(result.Result);
        Assert.Equal("No hay datos", result.MessageResult);
    }

    [Fact]
    public void GetById_WhenCategoryExists_ReturnsCategory()
    {
        var category = new CategoriaResponseDto { Id = 1, Nombre = "Oficina" };
        _repository.Setup(repo => repo.GetById(1)).Returns(category);

        var result = CreateService().GetById(1);

        Assert.True(result.stateOperation);
        Assert.Same(category, result.Result);
    }

    [Fact]
    public void Update_WithPlaceholderValues_PreservesExistingCategoryValues()
    {
        var current = new CategoriaResponseDto
        {
            Nombre = "Electronica",
            Descripcion = "Equipos",
            Activo = true
        };
        var update = new CategoriaUpdateDto { Nombre = "string", Descripcion = " ", Activo = null };
        CategoriaUpdateDto? sentToRepository = null;
        _repository.Setup(repo => repo.GetById(7)).Returns(current);
        _repository.Setup(repo => repo.Update(7, It.IsAny<CategoriaUpdateDto>()))
            .Callback<int, CategoriaUpdateDto>((_, dto) => sentToRepository = dto)
            .Returns(true);

        var result = CreateService().Update(7, update);

        Assert.True(result.stateOperation);
        Assert.True(result.Result);
        Assert.NotNull(sentToRepository);
        Assert.Equal("Electronica", sentToRepository.Nombre);
        Assert.Equal("Equipos", sentToRepository.Descripcion);
        Assert.True(sentToRepository.Activo);
    }

    [Fact]
    public void Update_WhenCategoryDoesNotExist_DoesNotAttemptUpdate()
    {
        _repository.Setup(repo => repo.GetById(99)).Returns((CategoriaResponseDto)null!);

        var result = CreateService().Update(99, new CategoriaUpdateDto { Nombre = "Nueva" });

        Assert.False(result.stateOperation);
        Assert.Equal("La categoria a actualizar no existe", Normalize(result.MessageResult));
        _repository.Verify(repo => repo.Update(It.IsAny<int>(), It.IsAny<CategoriaUpdateDto>()), Times.Never);
    }

    [Fact]
    public void Create_WhenRepositoryFails_RollsBackUnitOfWork()
    {
        _repository.Setup(repo => repo.Create(It.IsAny<CategoriaCreateDto>()))
            .Throws(new InvalidOperationException("fallo"));

        var result = CreateService().Create(new CategoriaCreateDto { Nombre = "Insumos" });

        Assert.False(result.stateOperation);
        Assert.True(result.RollBack);
        _unitOfWork.Verify(uow => uow.Rollback(), Times.Once);
    }

    [Fact]
    public void Delete_WhenRepositoryDeletesCategory_ReturnsTrue()
    {
        _repository.Setup(repo => repo.Delete(4)).Returns(true);

        var result = CreateService().Delete(4);

        Assert.True(result.stateOperation);
        Assert.True(result.Result);
    }

    private static string? Normalize(string? value) =>
        value?.Normalize(System.Text.NormalizationForm.FormD)
            .Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
            .Aggregate(string.Empty, (text, c) => text + c);
}
