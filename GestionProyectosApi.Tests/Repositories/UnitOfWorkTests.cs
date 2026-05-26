using System.Data;
using GestionProyectosApi.Infrastructure.Repositories._UnitOfWork;
using Moq;

namespace GestionProyectosApi.Tests.Repositories;

public class UnitOfWorkTests
{
    [Fact]
    public void Rollback_WithoutStartedTransaction_DoesNotThrow()
    {
        var unitOfWork = new UnitOfWork(new Mock<IDbConnection>().Object);

        var exception = Record.Exception(unitOfWork.Rollback);

        Assert.Null(exception);
    }

    [Fact]
    public void BeginAndRollback_RollsBackCreatedTransaction()
    {
        var transaction = new Mock<IDbTransaction>();
        var connection = new Mock<IDbConnection>();
        connection.Setup(item => item.BeginTransaction()).Returns(transaction.Object);
        var unitOfWork = new UnitOfWork(connection.Object);

        unitOfWork.Begin();
        unitOfWork.Rollback();

        transaction.Verify(item => item.Rollback(), Times.Once);
    }
}
