using LowTideEnt.Application.Services.User.Dto;
using LowTideEnt.Application.Services.User.Mapping;
using System.Text.RegularExpressions;

namespace LowTideEnt.Application.Services.User
{
    public class UserService : IUserService
    {
        ISessionContext sessionContext;
        IUserRepository repository;

        public UserService(ISessionContext sessionContext, IUserRepository repository)
        {
            this.sessionContext = sessionContext;
            this.repository = repository;
        }

        public async Task<UserResponse> GetUserByIdAsync(int userId) =>
            (await repository.GetByIdAsync(userId)).ToResponse();
        public async Task<IEnumerable<UserResponse>> GetUsersByQueryAsync(UserQuery query, CancellationToken cancellationToken) =>
            (await repository.GetUsersByQueryAsync(query, cancellationToken))
            .Select(u => u.ToResponse()).ToList();
        public async Task<UserResponse> AddUserAsync(UserRequest userRequest, CancellationToken cancellationToken)
        {
            userRequest.Email = userRequest.Email.ToLower().Trim();
            var emailVerification = Regex.Match(userRequest.Email, @"([a-z]*\.[a-z]*)@lowtide.com");     // TODO: data driven
            if (!emailVerification.Success) throw new InvalidRequestException();
            return (await repository.AddAsync(userRequest.ToAddEntity(sessionContext.UserName), cancellationToken)).ToResponse();
        }
        public async Task UpdateUserAsync(UserRequest userRequest, CancellationToken cancellationToken)
        {
            userRequest.Email = userRequest.Email.ToLower().Trim();
            var emailVerification = Regex.Match(userRequest.Email, @"([a-z]*\.[a-z]*)@lowtide.com");     // TODO: data driven
            if (userRequest.Id <= 0
                || !emailVerification.Success) throw new InvalidRequestException();
            await repository.AddAsync(userRequest.ToUpdateEntity(sessionContext.UserName), cancellationToken);
        }
        public async Task RemoveUserByIdAsync(int id, CancellationToken cancellationToken) => 
            await repository.RemoveUserByIdAsync(id, sessionContext.UserName, cancellationToken);
    }
}
