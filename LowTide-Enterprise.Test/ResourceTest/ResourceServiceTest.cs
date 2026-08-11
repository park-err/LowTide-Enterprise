using LowTideEnt.Application;
using LowTideEnt.Application.Services.Resource;
using LowTideEnt.Application.Services.Resource.Dto;
using LowTideEnt.Application.Services.Resource.Mapping;
using LowTideEnt.Domain;
using LowTideEnt.Domain.Entities.ResourceManager;
using LowTideEnt.Domain.Models;
using LowTideEnt.Domain.Queries;
using LowTideEnt.Infrastructure.Repositories.Interfaces;
using Moq;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using static LowTideEnt.Infrastructure.Middleware.ExceptionHandlerMiddleware;

namespace LowTideEnt_API.Test.ResourceTest
{
    public class ResourceServiceTest
    {
        private class MockSessionContext : ISessionContext
        {
            public bool IsAuthenticated { get; set; } = true;
            public int UserId { get; set; } = 1;
            public string UserName { get; set; } = "test-user";
            public string? Email { get; set; } = "test@example.com";
            public IReadOnlyCollection<string> Roles { get; set; } = Array.Empty<string>();
            public string? GetClaimValue(string claimType) => null;
        }

        public class MockResourceQuery : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // change these lines
                yield return new object[] { new ResourceQuery { ParentId = null } };
                yield return new object[] { new ResourceQuery { ParentId = null } };
                yield return new object[] { new ResourceQuery { ParentId = 1 } };
                yield return new object[] { new ResourceQuery { ContentContains = "Test", ParentId = null } };
                // don't change these lines (except for query model name and its corresponding fields)
                yield return new object[] { new ResourceQuery { ParentId = null,
                    CreatedByContains = "test.user" } };  // leave this line alone
                yield return new object[] { new ResourceQuery { ParentId = null,
                    ModifiedByContains = "test.user" } };
                yield return new object[] { new ResourceQuery { ParentId = null,
                    PageSize = 1 } };
                yield return new object[] { new ResourceQuery { ParentId = null,
                    PageNumber = 2, PageSize = 2 } };
                yield return new object[] { new ResourceQuery { ParentId = null,
                    CreatedFromDate = DateTime.Now } };
                yield return new object[] { new ResourceQuery { ParentId = null,
                    CreatedToDate = DateTime.Now.AddDays(-2) } };
                yield return new object[] { new ResourceQuery { ParentId = null,
                    ModifiedFromDate = DateTime.Now } };
                yield return new object[] { new ResourceQuery { ParentId = null,
                    ModifiedToDate = DateTime.Now.AddDays(-2) } };
                yield return new object[] { new ResourceQuery { ParentId = null,
                    PageSize = 0 } };       // empty return
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        MockSessionContext sessionContext;
        Mock<ICategoryRepository> categoryRepository;
        Mock<IResourceRepository> resourceRepository;
        ResourceService service;
        MetadataObject metadata = new MetadataObject { Department = "Test", Category = "Test" };
        private readonly CancellationToken cancellationToken = default;

        public ResourceServiceTest()
        {
            sessionContext = new MockSessionContext();
            categoryRepository = new Mock<ICategoryRepository>();
            resourceRepository = new Mock<IResourceRepository>();
            service = new ResourceService(sessionContext, resourceRepository.Object, categoryRepository.Object);
        }


        [Fact(DisplayName = "GetCategoryByIdAsync: input Id that exists, expect CategoryResponse")]
        [Category("Category")]
        public async Task GetCategoryByIdAsync_ShouldReturnCategoryById()
        {
            // Arrange
            var responseId = 1;
            var expectedEntity = new CategoryEntity
            {
                Id = 1,
                StatusId = Status.Active,
                Name = "Scheduling/Front desk",
                CreatedBy = sessionContext.UserName,
                ModifiedBy = sessionContext.UserName
            };
            categoryRepository
                .Setup(x => x.GetByIdAsync(responseId))
                .ReturnsAsync(expectedEntity);

            // Act

            var result = await service.GetCategoryByIdAsync(responseId, default);

            // Assert

            Assert.NotNull(result);
            Assert.IsType<CategoryResponse>(result);
            Assert.Equal(1, result!.Id);
            Assert.Equal("Scheduling/Front desk", result.Name);
        }

        [Fact(DisplayName = "GetCategoryByIdAsync: input Id that DNE, expect ExpectedEntityNotFoundException")]
        [Category("Category")]
        public async Task GetCategoryByIdAsync_ShouldThrowExpectedEntityNotFoundException()
        {
            // Arrange
            var categoryId = 0;
            categoryRepository
                .Setup(x => x.GetByIdAsync(categoryId))
                .ThrowsAsync(new ExpectedEntityNotFoundException());

            // Act
            // Assert

            await Assert.ThrowsAsync<ExpectedEntityNotFoundException>(() => service.GetCategoryByIdAsync(categoryId, default));
        }

        [Fact(DisplayName = "GetCategoriesAsync: input none, expect CategoryResponse")]
        [Category("Category")]
        public async Task GetCategoriesAsync_ShouldReturnCategoryList()
        {
            // Arrange
            var expectedEntities = new List<CategoryEntity>() {
                new CategoryEntity { Id = 1, StatusId = Status.Active, Name = "Scheduling/Front desk",
                    CreatedBy = sessionContext.UserName, ModifiedBy = sessionContext.UserName },
                new CategoryEntity { Id = 2, StatusId = Status.Active, Name = "Technicians",
                    CreatedBy = sessionContext.UserName, ModifiedBy = sessionContext.UserName },
                new CategoryEntity { Id = 3, StatusId = Status.Active, Name = "Administrators",
                    CreatedBy = sessionContext.UserName, ModifiedBy = sessionContext.UserName }
            };

            categoryRepository
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(expectedEntities);

            // Act

            var result = (await service.GetCategoriesAsync(default)).ToList();

            // Assert

            Assert.NotNull(result);
            Assert.IsType<List<CategoryResponse>>(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal("Technicians", result[1].Name);
            Assert.Equal("Scheduling/Front desk", result[0].Name);
        }
        [Fact(DisplayName = "GetCategoriesAsync: input none, expect empty CategoryResponse")]
        [Category("Category")]
        public async Task GetCategoriesAsync_ShouldReturnEmptyCategoryList()
        {
            // Arrange
            var expectedEntities = new List<CategoryEntity>();

            categoryRepository
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(expectedEntities);

            // Act

            var result = (await service.GetCategoriesAsync(default)).ToList();

            // Assert

            Assert.NotNull(result);
            Assert.IsType<List<CategoryResponse>>(result);
            Assert.Empty(result);
        }

        [Fact(DisplayName = "AddCategoryAsync: input CategoryRequest, expect CategoryResponse")]
        [Category("Category")]
        public async Task AddCategoryAsync_ShouldReturnNewCategory()
        {
            // Arrange
            var entity = new CategoryEntity() { Id = 1, Name = "Test Category", StatusId = Status.Active };
            var request = new CategoryRequest() { Name = "Test Category", Description = "Test Description" };

            categoryRepository
                .Setup(x => x.AddAsync(It.Is<CategoryEntity>(e => e.Name == request.Name)))
                .ReturnsAsync(entity);

            // Act
            var result = await service.AddCategoryAsync(request, default);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CategoryResponse>(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Category", result.Name);
        }

        [Fact(DisplayName = "AddCategoryAsync: input CategoryRequest with empty Name, expect InvalidRequestException")]
        [Category("Category")]
        public async Task AddCategoryAsync_ShouldThrowInvalidRequestException()
        {
            // Arrange
            var badRequest = new CategoryRequest() { Name = "" };

            categoryRepository
                .Setup(x => x.AddAsync(badRequest.ToAddEntity(sessionContext.UserName)))
                .Throws<InvalidRequestException>();

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidRequestException>(() => service.AddCategoryAsync(badRequest, default));
        }

        [Fact(DisplayName = "UpdateCategoryAsync: input CategoryRequest, expect completed Task")]
        [Category("Category")]
        public async Task UpdateCategoryAsync_ShouldReturnCompletedTask()
        {
            // Arrange 
            var request = new CategoryRequest() { Id = 1, Name = "Test", Description = "Test" };

            categoryRepository
                .Setup(x => x.UpdateAsync(request.ToUpdateEntity(sessionContext.UserName)))
                .Returns(Task.CompletedTask);

            // Act
            // Assert
            await Assert.IsAssignableFrom<Task>(service.UpdateCategoryAsync(request, default));
        }

        [Theory(DisplayName = "UpdateCategoryAsync: input invalid CategoryRequest, expect InvalidRequestException")]
        [InlineData(0, "Test")]
        [InlineData(1, "")]
        [InlineData(1, null)]
        [Category("Category")]
        public async Task UpdateCategoryAsync_ShouldThrowInvalidRequestException(int id, string? title)
        {
            // Arrange 
            var badRequest = new CategoryRequest() { Id = id, Name = title, Description = "Test" };

            categoryRepository
                .Setup(x => x.UpdateAsync(badRequest.ToUpdateEntity(sessionContext.UserName)))
                .Returns(Task.CompletedTask);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidRequestException>(() => service.UpdateCategoryAsync(badRequest, default));
        }

        [Fact(DisplayName = "GetResourceByIdAsync: input existing resource id, expect ResourceResponse")]
        [Category("Resource")]
        public async Task GetResourceByIdAsync_ShouldReturnResource()
        {
            // Arrange
            var categoryId = 1;
            var resourceId = 1;
            var path = new byte[0];
            var resource = new ResourceModel() { Id = 1, Title = "Test", CategoryId = 1, Metadata = metadata };

            resourceRepository
                .Setup(x => x.GetByIdAsync(categoryId, resourceId))
                .ReturnsAsync(resource);

            // Act
            var result = await service.GetResourceByIdAsync(categoryId, resourceId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ResourceResponse>(result);
            Assert.Equal(resource.Id, result.Id);
        }

        [Fact(DisplayName = "GetResourceByIdAsync: input non-existing resource id, expect ExpectedEntityNotFoundException")]
        [Category("Resource")]
        public async Task GetResourceByIdAsync_ShouldReturnExpectedEntityNotFoundException()
        {
            // Arrange
            var resourceId = 99;
            var categoryId = 99;

            resourceRepository
                .Setup(x => x.GetByIdAsync(categoryId, resourceId))
                .ThrowsAsync(new ExpectedEntityNotFoundException());

            // Act
            // Assert
            await Assert.ThrowsAsync<ExpectedEntityNotFoundException>(() => service.GetResourceByIdAsync(categoryId, resourceId));
        }

        [Theory(DisplayName = "GetResourcesByQueryAsync: input query model, expect list of ResourceModel")]
        [ClassData(typeof(MockResourceQuery))]
        [Category("Resource")]
        public async Task GetResourcesByQueryAsync_ShouldReturnResourceModelList(ResourceQuery query)
        {
            // Arrange
            var categoryId = 1;
            var path = new byte[0];
            var resources = new List<ResourceModel>
            {
                new ResourceModel { Id = 1, Title = "Test Resource", CategoryId = 1, Metadata = metadata },
                new ResourceModel { Id = 2, Title = "Test Resource 2", CategoryId = 1, Metadata = metadata },
                new ResourceModel { Id = 3, Title = "Test", CategoryId = 1, Metadata = metadata },
            };
            var expectedResources = resources.Where(r =>
                r.CategoryId == categoryId
                && r.ParentId == (query.ParentId ?? r.ParentId)
                && (r.Title?.ToLower().Contains(query.ContentContains.ToLower()) ?? true)
                && r.ModifiedBy.ToLower().Contains(query.ModifiedByContains.ToLower())
                && r.ModifiedDate <= query.ModifiedFromDate
                && r.ModifiedDate >= query.ModifiedToDate
                ).ToList();

            resourceRepository
                .Setup(x => x.GetResourcesByQueryAsync(categoryId, query))
                .ReturnsAsync(expectedResources);

            // Act
            var result = (await service.GetResourceByQueryAsync(categoryId, query, default)).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ResourceResponse>>(result);
            Assert.Equal(Math.Min(expectedResources.Count(), query.PageSize), result.Count());
            if (result.Count > 0) Assert.Equal(expectedResources[query.PageNumber - 1].Title, result[0].Title);
            else Assert.Empty(result);
        }

        [Fact(DisplayName = "AddResourceAsync: input valid ResourceRequest, expect ResourceResponse")]
        [Category("Resource")]
        public async Task AddResourceAsync_ShouldReturnResourceModel()
        {
            // Arrange
            var resultId = 1;
            var categoryId = 1;
            var entity = new ResourceEntity(categoryId, "Test", "#Test", metadata) { Id = resultId };
            var request = new ResourceRequest() { Title = "Test", Content = "#Test", Metadata = metadata };
            
            resourceRepository
                .Setup(x => x.AddAsync(It.Is<ResourceEntity>(e => e.Title == request.Title)))
                .ReturnsAsync(entity);

            // Act
            var result = await service.AddResourceAsync(categoryId, request, default);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ResourceResponse>(result);
            Assert.Equal(resultId, result.Id);
            Assert.Equal(request.Title, result.Title);
        }

        [Fact(DisplayName = "AddResourceAsync: input invalid ResourceRequest, expect throw InvalidRequestException")]
        [Category("Resource")]
        public async Task AddResourceAsync_ShouldThrowInvalidRequestException()
        {
            // Arrange
            var categoryId = 1;
            var entity = new ResourceEntity(categoryId, "Test", "#Test", metadata);
            var badRequest = new ResourceRequest() { Title = "", Content = "#Test", Metadata = metadata };

            resourceRepository
                .Setup(x => x.AddAsync(badRequest.ToAddEntity(categoryId, sessionContext.UserName)))
                .ReturnsAsync(entity);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidRequestException>(() => service.AddResourceAsync(categoryId, badRequest, default));
        }

        [Theory(DisplayName = "AddResourceAsync: input invalid FK, expect throw SqlException")]
        [Category("Resource")]
        [InlineData(547)]   // fk violation for statusId or roleId
        [InlineData(2627)]  // pk violation
        [InlineData(2601)]  // pk violation
        [InlineData(-2)]    // connection timeout
        public async Task AddResourceAsync_ShouldThrowSqlException(int number)
        {
            // Arrange
            var sqlException =
                new SqlExceptionBuilder().WithErrorNumber(number)
                    .WithErrorMessage("Database exception occured...")
                    .Build();
            var categoryId = -99;
            var badRequest = new ResourceRequest() { Title = "Test", Content = "#Test", Metadata = metadata };

            resourceRepository
                .Setup(x => x.AddAsync(It.Is<ResourceEntity>(e => e.Title == badRequest.Title)))
                .ThrowsAsync(sqlException);

            // Act
            // Assert
            await Assert.ThrowsAsync<SqlException>(() => service.AddResourceAsync(categoryId, badRequest, default));
        }

        [Fact(DisplayName = "UpdateResourceAsync: input valid ResourceRequest, expect ResourceResponse")]
        [Category("Resource")]
        public async Task UpdateResourceAsync_ShouldReturnResourceModel()
        {
            // Arrange
            var categoryId = 1;
            var path = new byte[0];
            var request = new ResourceRequest() { Id = 1, Title = "Updated Resource", Content = "#Test", Metadata = metadata };

            resourceRepository
                .Setup(x => x.UpdateAsync(request.ToUpdateEntity(categoryId, sessionContext.UserName)))
                .Returns(Task.CompletedTask);

            // Act
            // Assert
            await Assert.IsAssignableFrom<Task>(service.UpdateResourceAsync(categoryId, request, default));
        }

        [Theory(DisplayName = "UpdateResourceAsync: input invalid ResourceRequest, expect throw InvalidRequestException")]
        [Category("Resource")]
        [InlineData(0, "Test")]
        [InlineData(1, "")]
        [InlineData(1, null)]
        public async Task UpdateResourceAsync_ShouldThrowInvalidRequestException(int id, string? title)
        {
            // Arrange
            var categoryId = 1;
            var badRequest = new ResourceRequest() { Id = id, Title = title, Content = "#Test", Metadata = metadata };
            
            resourceRepository
                .Setup(x => x.UpdateAsync(badRequest.ToUpdateEntity(categoryId, sessionContext.UserName)))
                .Returns(Task.CompletedTask);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidRequestException>(() => service.UpdateResourceAsync(categoryId, badRequest, default));
        }

        [Theory(DisplayName = "UpdateResourceAsync: input invalid ResourceRequest, expect throw SqlException")]
        [Category("Resource")]
        [InlineData(547)]   // fk violation for statusId or roleId
        [InlineData(2627)]  // pk violation
        [InlineData(2601)]  // pk violation
        [InlineData(-2)]    // connection timeout
        public async Task UpdateResourceAsync_ShouldThrowSqlException(int number)
        {
            // Arrange
            var sqlException =
                new SqlExceptionBuilder().WithErrorNumber(number)
                    .WithErrorMessage("Database exception occured...")
                    .Build();
            var categoryId = -99;
            var badRequest = new ResourceRequest() { Id = 1, Title = "Test", Metadata = metadata, Content = "#Test" };

            resourceRepository
                .Setup(x => x.UpdateAsync(It.Is<ResourceEntity>(e => e.Id == badRequest.Id && e.Title == badRequest.Title)))
                .ThrowsAsync(sqlException);

            // Act
            // Assert
            await Assert.ThrowsAsync<SqlException>(() => service.UpdateResourceAsync(categoryId, badRequest, default));
        }
    }
}
