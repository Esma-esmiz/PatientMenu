using Xunit;
using Moq;
using Api.Data;
using Api.Repositories;
using Api.Services;
using Microsoft.Data.Sqlite;
using Dapper;
using System.Data;

using Xunit;
using Moq;
using Moq.Protected;
using Api.Data;
using Api.Repositories;
using Api.Services;
using Dapper;
using System.Data;
using System.Data.Common;

public class AllowedMenuTests
{
    private readonly Mock<IDbConnectionFactory> _mockFactory;
    private readonly Mock<DbConnection> _mockConnection;
    private readonly Mock<DbCommand> _mockCommand;
    private readonly Mock<ITenantProvider> _mockTenant;
    private readonly MenuItemRepository _repository;
    private readonly Guid _tenantId = Guid.NewGuid();

    public AllowedMenuTests()
    {
        _mockFactory = new Mock<IDbConnectionFactory>();
        _mockConnection = new Mock<DbConnection>();
        _mockCommand = new Mock<DbCommand>();
        _mockTenant = new Mock<ITenantProvider>();

        // Setup Factory to return Mock DbConnection
        _mockFactory.Setup(f => f.CreateConnection()).Returns(_mockConnection.Object);

        // Setup Connection to return Mock DbCommand
        _mockConnection.Protected()
            .Setup<DbCommand>("CreateDbCommand")
            .Returns(_mockCommand.Object);

        // Setup Tenant
        _mockTenant.Setup(t => t.TenantId).Returns(_tenantId);

        _repository = new MenuItemRepository(_mockFactory.Object, _mockTenant.Object);
    }

    [Fact]
    public async Task GetAllowedMenu_ReturnsItems_FromDataReader()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var expectedItemId = Guid.NewGuid();

        // Create a DataTable to simulate the Database ResultSet
        var table = new DataTable();
        table.Columns.Add("Id", typeof(Guid));
        table.Columns.Add("TenantId", typeof(Guid));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Category", typeof(string));
        table.Columns.Add("IsGlutenFree", typeof(bool));
        table.Columns.Add("IsSugarFree", typeof(bool));
        table.Columns.Add("IsHeartHealthy", typeof(bool));
        table.Columns.Add("CreatedAt", typeof(DateTime));
        table.Columns.Add("UpdatedAt", typeof(DateTime));

        // Add a row
        table.Rows.Add(expectedItemId, _tenantId, "GF Bread", "Bread", true, false, false, DateTime.UtcNow, DateTime.UtcNow);

        // Create DataReader from DataTable
        using var reader = table.CreateDataReader();

        // Setup Command to return Reader asynchronously
        _mockCommand.Protected()
            .Setup<Task<DbDataReader>>("ExecuteDbDataReaderAsync", ItExpr.IsAny<CommandBehavior>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(reader);

        // Mock Parameters
        var mockParams = new Mock<DbParameterCollection>();
        _mockCommand.Protected().Setup<DbParameter>("CreateDbParameter").Returns(new Mock<DbParameter>().Object);
        _mockCommand.Protected().Setup<DbParameterCollection>("DbParameterCollection").Returns(mockParams.Object);

        // Act
        var result = await _repository.GetAllowedMenuAsync(patientId);

        // Assert
        Assert.Single(result);
        Assert.Equal("GF Bread", result.First().Name);
        Assert.Equal(expectedItemId, result.First().Id);
    }

    [Fact]
    public async Task GetAllowedMenu_WhenEmpty_ReturnsEmptyList()
    {
        // Arrange
        var patientId = Guid.NewGuid();

        // Create Empty DataTable
        var table = new DataTable();
        table.Columns.Add("Id", typeof(Guid));
        table.Columns.Add("TenantId", typeof(Guid));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Category", typeof(string));
        table.Columns.Add("IsGlutenFree", typeof(bool));
        table.Columns.Add("IsSugarFree", typeof(bool));
        table.Columns.Add("IsHeartHealthy", typeof(bool));
        table.Columns.Add("CreatedAt", typeof(DateTime));
        table.Columns.Add("UpdatedAt", typeof(DateTime));

        using var reader = table.CreateDataReader();

        _mockCommand.Protected()
            .Setup<Task<DbDataReader>>("ExecuteDbDataReaderAsync", ItExpr.IsAny<CommandBehavior>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(reader);
        
        var mockParams = new Mock<DbParameterCollection>();
        _mockCommand.Protected().Setup<DbParameter>("CreateDbParameter").Returns(new Mock<DbParameter>().Object);
        _mockCommand.Protected().Setup<DbParameterCollection>("DbParameterCollection").Returns(mockParams.Object);

        // Act
        var result = await _repository.GetAllowedMenuAsync(patientId);

        // Assert
        Assert.Empty(result);
    }
}
