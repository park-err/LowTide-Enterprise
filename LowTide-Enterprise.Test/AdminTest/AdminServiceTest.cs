using LowTideEnt.Application;
using LowTideEnt.Application.Services.Admin;
using LowTideEnt.Application.Services.User.Dto;
using LowTideEnt.Domain;
using LowTideEnt.Domain.Entities.GlobalConfig;
using LowTideEnt.Domain.Models;
using LowTideEnt.Infrastructure.Repositories.Interfaces;
using Moq;
using System.ComponentModel;
using System.Data.Common;
using Xunit;
using static LowTideEnt.Infrastructure.Middleware.ExceptionHandlerMiddleware;

namespace LowTideEnt_API.Test.ModalityTest
{
    public class AdminServiceTest
    {
        private class MockSessionContext : ISessionContext
        {
            public bool IsAuthenticated { get; set; } = true;
            public int UserId { get; set; } = 1;
            public string UserName { get; set; } = "test.user";
            public string? Email { get; set; } = "test@example.com";
            public IReadOnlyCollection<string> Roles { get; set; } = Array.Empty<string>();
            public string? GetClaimValue(string claimType) => null;
        }

        private readonly MockSessionContext sessionContext;
        private readonly Mock<IRoleRepository> mockRoleRepository;
        private readonly Mock<IPermissionRepository> mockPermissionRepository;
        private readonly AdminService service;
        private readonly CancellationToken cancellationToken = default;

        public AdminServiceTest()
        {
            sessionContext = new MockSessionContext();
            mockRoleRepository = new Mock<IRoleRepository>();
            mockPermissionRepository = new Mock<IPermissionRepository>();
            service = new AdminService(sessionContext, mockRoleRepository.Object, mockPermissionRepository.Object);
        }

        [Fact(DisplayName = "GetRolePermissionsByUserIdAsync: Returns role permissions when user exists")]
        public async Task GetRolePermissionsByUserIdAsync_ReturnsRolePermissions_WhenUserExists()
        {
            // Arrange
            var category = "ResourceLibrary";
            var expectedRolePermissions = new RolePermissionModel
            {
                Roles = new List<RoleModel> { new RoleModel { Id = 1, Name = "Admin" } },
                Permissions = new List<PermissionModel> { new PermissionModel { Category = category, Name = "TechResource", Type = "Read" },
                new PermissionModel { Category = category, Name = "TechResource", Type = "Write" } }
            };

            mockRoleRepository.Setup(repo => repo.GetRolesByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedRolePermissions.Roles);
            mockPermissionRepository.Setup(repo => repo.GetPermissionsByRoleIdAsync(It.IsAny<int[]>()))
                .ReturnsAsync(expectedRolePermissions.Permissions);

            // Act
            var result = await service.GetRolePermissionsByUserIdAsync(1, default);

            // Assert
            Assert.Equal(expectedRolePermissions.Roles.Count(), result.Roles.Count());
            Assert.Single(result.PermissionCategories);
            var permissionCategory = result.PermissionCategories.First();
            Assert.Equal(category, permissionCategory.Category);
            Assert.Single(permissionCategory.Permissions);
        }

        [Theory(DisplayName = "GetUserDetailByUserIdAsync: input invalid id, expect throw SqlException")]
        [InlineData(547)]   // fk violation for statusId or roleId
        [InlineData(2627)]  // pk violation
        [InlineData(2601)]  // pk violation
        [InlineData(-2)]    // connection timeout
        [Category("R&P")]
        public async Task GetRolePermissionsByUserIdAsync_ShouldThrowSqlException_RoleRepositoryThrows(int number)
        {
            // Arrange
            var sqlException =
                new SqlExceptionBuilder().WithErrorNumber(number)
                    .WithErrorMessage("Database exception occured...")
                    .Build();

            mockRoleRepository.Setup(repo => repo.GetRolesByUserIdAsync(It.IsAny<int>()))
                .ThrowsAsync(sqlException);

            // Act
            // Assert
            await Assert.ThrowsAsync<SqlException>(() => service.GetRolePermissionsByUserIdAsync(It.IsAny<int>(), default));
        }

        [Theory(DisplayName = "GetUserDetailByUserIdAsync: input invalid id, expect throw SqlException")]
        [InlineData(547)]   // fk violation for statusId or roleId
        [InlineData(2627)]  // pk violation
        [InlineData(2601)]  // pk violation
        [InlineData(-2)]    // connection timeout
        [Category("R&P")]
        public async Task GetRolePermissionsByUserIdAsync_ShouldThrowSqlException_PermissionRepositoryThrows(int number)
        {
            // Arrange
            var roles = new List<RoleModel> { new RoleModel { Id = 1, Name = "Admin" } };
            var sqlException =
                new SqlExceptionBuilder().WithErrorNumber(number)
                    .WithErrorMessage("Database exception occured...")
                    .Build();

            mockRoleRepository.Setup(repo => repo.GetRolesByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(roles);
            mockPermissionRepository.Setup(repo => repo.GetPermissionsByRoleIdAsync(It.IsAny<int[]>()))
                .ThrowsAsync(sqlException);

            // Act
            // Assert
            await Assert.ThrowsAsync<SqlException>(() => service.GetRolePermissionsByUserIdAsync(It.IsAny<int>(), default));
        }
    }
}