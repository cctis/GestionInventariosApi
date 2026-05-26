using GestionInventariosApi.Domain.Models.Generico.SP;
using GestionInventariosApi.Infrastructure.Repositories;

namespace GestionInventariosApi.Tests.Repositories;

public class RepositoryValidationTests
{
    [Fact]
    public void CategoriaCreate_WithoutName_ThrowsBeforeDatabaseExecution()
    {
        var repository = new CategoriaRepository();

        var exception = Assert.Throws<ArgumentException>(() =>
            repository.Create(new CategoriaCreateDto { Nombre = " " }));

        Assert.Contains("obligatorio", exception.Message);
    }

    [Fact]
    public void EstadoProductoUpdate_WithoutName_ThrowsBeforeDatabaseExecution()
    {
        var repository = new EstadoProductoRepository();

        var exception = Assert.Throws<ArgumentException>(() =>
            repository.Update(1, new EstadoProductoUpdateDto { Nombre = "" }));

        Assert.Contains("obligatorio", exception.Message);
    }

    [Fact]
    public void ProductoCreate_WithNegativeStock_ThrowsBusinessValidation()
    {
        var repository = new ProductoRepository();
        var dto = new ProductoCreateDto { Nombre = "Cable", Sku = "CAB-01", Stock = -1 };

        var exception = Assert.Throws<ArgumentException>(() => repository.Create(dto));

        Assert.Contains("stock inicial", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ProductoCreate_WithNegativeMinimumStock_ThrowsBusinessValidation()
    {
        var repository = new ProductoRepository();
        var dto = new ProductoCreateDto { Nombre = "Cable", Sku = "CAB-01", StockMinimo = -1 };

        var exception = Assert.Throws<ArgumentException>(() => repository.Create(dto));

        Assert.Contains("stock minimo", RemoveAccents(exception.Message), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ProductoUpdate_WithNullDto_ThrowsBeforeDatabaseExecution()
    {
        var repository = new ProductoRepository();

        Assert.Throws<ArgumentNullException>(() => repository.Update(1, null!));
    }

    private static string RemoveAccents(string value) =>
        value.Normalize(System.Text.NormalizationForm.FormD)
            .Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
            .Aggregate(string.Empty, (text, c) => text + c);
}
