using Dapper;
using System.Data;
using Api.Services;
using Api.Repositories;

namespace Api.Data
{
    public class MenuItemRepository :IMenuIteamRepository
    {
        private readonly IDbConnectionFactory _factory;
        private readonly ITenantProvider _tenant;

        public MenuItemRepository(
            IDbConnectionFactory factory,
            ITenantProvider tenant)
        {
            _factory = factory;
            _tenant = tenant;
        }

        public async Task<IEnumerable<MenuItem>> GetAllowedMenuAsync(Guid patientId)
        {
            using var conn = _factory.CreateConnection();

            const string sql = """
            SELECT m.*
            FROM MenuItems m
            INNER JOIN Patients p ON p.Id = @PatientId
            WHERE
                p.TenantId = @TenantId
            AND m.TenantId = @TenantId
            AND (
                p.DietaryRestrictionCode = 'NONE'
                OR (p.DietaryRestrictionCode = 'GF' AND m.IsGlutenFree = 1)
                OR (p.DietaryRestrictionCode = 'SF' AND m.IsSugarFree = 1)
            )
        """;

            return await conn.QueryAsync<MenuItem>(sql, new
            {
                PatientId = patientId,
                TenantId = _tenant.TenantId
            });
        }
    }
}