using LowTideEnt.Application.Services.User.Dto;
using LowTideEnt.Domain.Entities.GlobalConfig;
using System;
using System.Collections.Generic;
using System.Text;

namespace LowTideEnt.Application.Services.User.Mapping
{
    public static class UserMapping
    {
        public static UserResponse ToResponse(this UserEntity entity) =>
            new UserResponse()
            {
                Id = entity.Id,
                StatusId = entity.StatusId,
                UserName = entity.UserName ?? throw new Exception("User name is required"),
                DisplayName = entity.DisplayName,
                Email = entity.Email,
                AvatarUrl = entity.AvatarUrl,
                CreatedBy = entity.CreatedBy,
                CreatedDate = entity.CreatedDate,
                ModifiedBy = entity.ModifiedBy,
                ModifiedDate = entity.ModifiedDate,
            };
        public static UserEntity ToAddEntity(this UserRequest request, string user) =>
            new UserEntity(request.StatusId, request.Email, request.IsAdmin, user);
        public static UserEntity ToUpdateEntity(this UserRequest request, string user) =>
            new UserEntity(request.StatusId, request.Email, request.IsAdmin, user) { Id = request.Id };
    }
}
