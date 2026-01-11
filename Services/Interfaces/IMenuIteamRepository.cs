namespace Api.Repositories;

public interface IMenuIteamRepository
{
    Task<IEnumerable<MenuItem>> GetAllowedMenuAsync(Guid patientId);
}
