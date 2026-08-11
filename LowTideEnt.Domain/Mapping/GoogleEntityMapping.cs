using LowTideEnt.Domain.Models;
using LowTideEnt.Domain.Entities.GlobalConfig;

namespace LowTideEnt.Domain.Mapping
{
    public static class GoogleEntityMapping
    {
        public static UserEntity ToEntity(this GoogleAuthResponse response) => 
            new UserEntity(response.GoogleId, response.Email, response.AvatarUrl);
    }
}
