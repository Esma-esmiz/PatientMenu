namespace Api.Services;
public interface ITenantProvider
{
    Guid TenantId { get; }
}