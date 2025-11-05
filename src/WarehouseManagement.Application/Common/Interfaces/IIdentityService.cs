namespace WarehouseManagement.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Guid> CreateUserAsync(string email, string password, string role, CancellationToken cancellationToken = default);
    Task<bool> CheckPasswordSignInAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);
}
