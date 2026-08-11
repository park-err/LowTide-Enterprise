using LowTideEnt.Application;
using LowTideEnt.Application.Services.User;
using LowTideEnt.Application.Services.User.Dto;
using LowTideEnt.Domain;
using LowTideEnt.Domain.Entities.GlobalConfig;
using LowTideEnt.Domain.Models;
using LowTideEnt.Domain.Queries;
using LowTideEnt.Infrastructure.Repositories.Interfaces;
using Moq;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using static LowTideEnt.Infrastructure.Middleware.ExceptionHandlerMiddleware;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LowTideEnt_API.Test.UserTest
{
    public class UserServiceTest
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

        public class MockUserQuery : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // change these lines
                yield return new object[] { new UserQuery { RoleId = null, StatusId = null } };
                yield return new object[] { new UserQuery { DisplayNameContains = "test", RoleId = null, StatusId = null } };
                yield return new object[] { new UserQuery { EmailContains = "test", RoleId = null, StatusId = null } };
                yield return new object[] { new UserQuery { RoleId = 1, StatusId = null } };
                yield return new object[] { new UserQuery { RoleId = null, StatusId = 1 } };
                // don't change these lines (except for query model name and its corresponding fields)
                yield return new object[] { new UserQuery { RoleId = null, StatusId = null,
                    CreatedByContains = "test.user" } };  // leave this line alone
                yield return new object[] { new UserQuery { RoleId = null, StatusId = null,
                    ModifiedByContains = "test.user" } };
                yield return new object[] { new UserQuery { RoleId = null, StatusId = null,
                    PageSize = 1 } };
                yield return new object[] { new UserQuery { RoleId = null, StatusId = null,
                    PageNumber = 2, PageSize = 2 } };
                yield return new object[] { new UserQuery { RoleId = null, StatusId = null,
                    CreatedFromDate = DateTime.Now } };
                yield return new object[] { new UserQuery { RoleId = null, StatusId = null,
                    CreatedToDate = DateTime.Now.AddDays(-2) } };
                yield return new object[] { new UserQuery { RoleId = null, StatusId = null,
                    ModifiedFromDate = DateTime.Now } };
                yield return new object[] { new UserQuery { RoleId = null, StatusId = null,
                    ModifiedToDate = DateTime.Now.AddDays(-2) } };
                yield return new object[] { new UserQuery { RoleId = null, StatusId = null,
                    PageSize = 0 } };       // empty return
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        MockSessionContext sessionContext;
        private readonly Mock<IUserRepository> mockRepository;
        private readonly UserService service;
        string testEmail = "test@lowtide.com";

        public UserServiceTest()
        {
            sessionContext = new MockSessionContext();
            mockRepository = new Mock<IUserRepository>();
            service = new UserService(sessionContext, mockRepository.Object);
        }

        [Fact(DisplayName = "GetUserByIdAsync: input user Id that exists, expect UserResponse")]
        [Category("User")]
        public async Task GetUserByIdAsync_ShouldReturnUserResponse()
        {
            // Arrange
            var userId = 1;
            var expectedEntity = new UserEntity("123", testEmail, "test test") 
            { Id = userId, CreatedBy = sessionContext.UserName, ModifiedBy = sessionContext.UserName };

            mockRepository
                .Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(expectedEntity);

            // Act
            var result = await service.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserResponse>(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(testEmail, result.Email);
        }

        [Fact(DisplayName = "GetUserByIdAsync: input user Id that DNE, expect throw ExpectedEntityNotFoundException")]
        [Category("User")]
        public async Task GetUserByIdAsync_ShouldThrowExpectedEntityNotFoundException()
        {
            // Arrange
            var userId = 99;

            mockRepository
                .Setup(x => x.GetByIdAsync(userId))
                .ThrowsAsync(new ExpectedEntityNotFoundException());

            // Act
            // Assert
            await Assert.ThrowsAsync<ExpectedEntityNotFoundException>(() => service.GetUserByIdAsync(userId));
        }

        [Theory(DisplayName = "GetUsersByQueryAsync: input UserQuery, expect list of UserResponse")]
        [ClassData(typeof(MockUserQuery))]
        public async Task GetUsersByQueryAsync_ShouldReturnUserResponseList(UserQuery query)
        {
            // Arrange
            var entities = new List<UserEntity>()
            {
                new UserEntity("1", "test@lowtide.com", "Test Test") { Id = 1, StatusId = Status.Active, IsAdmin = false, 
                    CreatedBy = sessionContext.UserName, CreatedDate = DateTime.Now, ModifiedBy = sessionContext.UserName, ModifiedDate = DateTime.Now },
                new UserEntity("2", "user@lowtide.com", "Test User") { Id = 2, StatusId = Status.Inactive, IsAdmin = false, 
                    CreatedBy = sessionContext.UserName, CreatedDate = DateTime.Now.AddDays(-3), ModifiedBy = sessionContext.UserName, ModifiedDate = DateTime.Now },
                new UserEntity("3", "user.test@lowtide.com", "User Test") { Id = 3, StatusId = Status.Active, IsAdmin = false, 
                    CreatedBy = sessionContext.UserName, CreatedDate = DateTime.Now.AddDays(-4), ModifiedBy = sessionContext.UserName, ModifiedDate = DateTime.Now.AddDays(-3) },
                new UserEntity("4", "user.user@lowtide.com", "User User") { Id = 4, StatusId = Status.Active, IsAdmin = true, 
                    CreatedBy = sessionContext.UserName, CreatedDate = DateTime.Now, ModifiedBy = "some.user", ModifiedDate = DateTime.Now },
                new UserEntity("5", "testing.user@lowtide.com", "Testing User") { Id = 5, StatusId = Status.Pending, IsAdmin = false, 
                    CreatedBy = "some.user", CreatedDate = DateTime.Now, ModifiedBy = sessionContext.UserName, ModifiedDate = DateTime.Now },
            };

            var expectedEntities = entities.Where(e =>
                e.StatusId == (Status)(query.StatusId ?? (int)e.StatusId)
                && (e.DisplayName?.ToLower().Contains(query.DisplayNameContains.ToLower()) ?? true)
                && e.Email.ToLower().Contains(query.EmailContains.ToLower())
                && e.CreatedBy.ToLower().Contains(query.CreatedByContains.ToLower())
                && e.CreatedDate <= query.CreatedFromDate
                && e.CreatedDate >= query.CreatedToDate
                && e.ModifiedBy.ToLower().Contains(query.ModifiedByContains.ToLower())
                && e.ModifiedDate <= query.ModifiedFromDate
                && e.ModifiedDate >= query.ModifiedToDate
                ).ToList();

            mockRepository
                .Setup(x => x.GetUsersByQueryAsync(query))
                .ReturnsAsync(expectedEntities);

            // Act
            var result = (await service.GetUsersByQueryAsync(query, default)).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<UserResponse>>(result);
            Assert.Equal(Math.Min(expectedEntities.Count(), query.PageSize), result.Count());
            if (result.Count > 0) Assert.Equal(expectedEntities[query.PageNumber - 1].UserName, result[0].UserName);
            else Assert.Empty(result);
        }

        [Fact(DisplayName = "AddUserAsync: input valid UserRequest, expect UserResponse")]
        [Category("User")]
        public async Task AddUserAsync_ShouldReturnUserResponse()
        {
            // Arrange
            var request = new UserRequest() { StatusId = Status.Active, Email = testEmail };
            var entity = new UserEntity() { StatusId = Status.Active, Email = testEmail, UserName = testEmail.Split("@")[0], IsAdmin = false, 
                CreatedBy = sessionContext.UserName, ModifiedBy = sessionContext.UserName };

            mockRepository
                .Setup(x => x.AddAsync(It.Is<UserEntity>(e => e.Email == request.Email)))
                .ReturnsAsync(entity);

            // Act
            var result = await service.AddUserAsync(request, default);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserResponse>(result);
            Assert.Equal(request.StatusId, result.StatusId);
            Assert.Equal(testEmail, result.Email);
        }

        [Theory(DisplayName = "AddUserAsync: input invalid UserRequest, expect throw InvalidRequestException")]
        [InlineData("test.test")]
        [InlineData("")]
        [Category("User")]
        public async Task AddUserAsync_ShouldThrowInvalidRequestException(string email)
        {
            // Arrange
            var request = new UserRequest() { StatusId = Status.Active, Email = email, IsAdmin = false };

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<UserEntity>()))
                .ReturnsAsync(It.IsAny<UserEntity>());

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidRequestException>(() => service.AddUserAsync(request, default));
        }

        [Theory(DisplayName = "AddUserAsync: input invalid FK, expect throw SqlException")]
        [InlineData(547)]   // fk violation for statusId or roleId
        [InlineData(2627)]  // pk violation
        [InlineData(2601)]  // pk violation
        [InlineData(-2)]    // connection timeout
        [Category("User")]
        public async Task AddUserAsync_ShouldThrowSqlException(int number)
        {
            // Arrange
            var sqlException =
                new SqlExceptionBuilder().WithErrorNumber(number)
                    .WithErrorMessage("Database exception occured...")
                    .Build();
            var userRequest = new UserRequest() { StatusId = Status.Active, Email = testEmail, IsAdmin = false };

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<UserEntity>()))
                .ThrowsAsync(sqlException);

            // Act
            // Assert
            await Assert.ThrowsAsync<SqlException>(() => service.AddUserAsync(userRequest, default));
        }

        [Fact(DisplayName = "UpdateUserAsync: input valid UserRequest, expect completed Task")]
        [Category("User")]
        public async Task UpdateUserAsync_ShouldReturnCompletedTask()
        {
            // Arrange 
            var request = new UserRequest() { Id = 1, StatusId = Status.Active, Email = testEmail, IsAdmin = false };

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<UserEntity>()));

            // Act
            // Assert
            await Assert.IsAssignableFrom<Task>(service.UpdateUserAsync(request, default));
        }

        [Theory(DisplayName = "UpdateUserAsync: input invalid UserRequest, expect throw InvalidRequestException")]
        [InlineData("test.test")]
        [InlineData("test.test@gmail.com")]
        [Category("User")]
        public async Task UpdateUserAsync_ShouldThrowInvalidRequestException(string email)
        {
            // Arrange 
            var request = new UserRequest() { Id = 1, StatusId = Status.Active, Email = email, IsAdmin = false };

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<UserEntity>()))
                .ReturnsAsync(It.IsAny<UserEntity>());

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidRequestException>(() => service.UpdateUserAsync(request, default));
        }

        [Theory(DisplayName = "UpdateUserAsync: input invalid UserRequest, expect throw SqlException")]
        [InlineData(207)]    // invalid column name
        [InlineData(-2)]    // connection timeout
        [Category("User")]
        public async Task UpdateUserAsync_ShouldThrowSqlException(int number)
        {
            // Arrange 
            var sqlException =
                new SqlExceptionBuilder().WithErrorNumber(number)
                    .WithErrorMessage("Database exception occured...")
                    .Build();
            var request = new UserRequest() { Id = 1, StatusId = Status.Active, Email = testEmail, IsAdmin = false };

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<UserEntity>()))
                .ThrowsAsync(sqlException);

            // Act
            // Assert
            await Assert.ThrowsAsync<SqlException>(() => service.UpdateUserAsync(request, default));
        }
    }
}
