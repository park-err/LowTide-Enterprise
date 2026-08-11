using LowTideEnt.Application.Services.Admin.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LowTideEnt.Application.Interfaces
{
    public interface IAdminService
    {
        Task<RolePermissionsResponse> GetRolePermissionsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    }
}
