using LowTideEnt.Application.Services.User.Dto;

namespace LowTideEnt.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserResponse>> GetUsersByQueryAsync(UserQuery query, CancellationToken cancellationToken);
        Task<UserResponse> AddUserAsync(UserRequest request, CancellationToken cancellationToken);
        Task UpdateUserAsync(UserRequest request, CancellationToken cancellationToken);
        Task RemoveUserByIdAsync(int id, CancellationToken cancellationToken);
    }
}
