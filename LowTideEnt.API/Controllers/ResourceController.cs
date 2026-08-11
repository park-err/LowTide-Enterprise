using LowTideEnt.Application.Interfaces;
using LowTideEnt.Application.Services.Resource.Dto;
using LowTideEnt.Domain.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LowTideEnt.API.Controllers
{
    [Route("category")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        IResourceService service;
        public ResourceController(IResourceService service)
        {
            this.service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken) 
        {
            var categories = await service.GetCategoriesAsync(cancellationToken);
            return Ok(categories);
        }

        [HttpGet]
        [Route("{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId, CancellationToken cancellationToken)
        {
            var category = await service.GetCategoryByIdAsync(categoryId, cancellationToken);
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await service.AddCategoryAsync(request, cancellationToken);
            return Ok(category);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryRequest request, CancellationToken cancellationToken)
        {
            await service.UpdateCategoryAsync(request, cancellationToken);
            return Ok();
        }

        [HttpPut]
        [Route("remove/{categoryId}")]
        public async Task<IActionResult> RemoveCategory(int categoryId, CancellationToken cancellationToken)
        {
            await service.RemoveCategoryByIdAsync(categoryId, cancellationToken);
            return Ok();
        }
        [HttpGet]
        [Route("{categoryId}/resources/all")]
        public async Task<IActionResult> GetResourceListByCategoryId(int categoryId, CancellationToken cancellationToken)
        {
            var categories = await service.GetResourceListByCategoryIdAsync(categoryId, cancellationToken);
            return Ok(categories);
        }

        [HttpGet]
        [Route("{categoryId}/resources")]
        public async Task<IActionResult> GetResourcesByQuery(int categoryId, [FromQuery] ResourceQuery query, CancellationToken cancellationToken) 
        {
            var categories = await service.GetResourceByQueryAsync(categoryId, query, cancellationToken);
            return Ok(categories);
        }

        [HttpGet]
        [Route("{categoryId}/resources/{resourceId}")]
        public async Task<IActionResult> GetResourceById(int categoryId, int resourceId)
        {
            var resource = await service.GetResourceByIdAsync(categoryId, resourceId);
            return Ok(resource);
        }

        [HttpGet]
        
        [Route("{categoryId}/resources/{resourceId}/content")]
        public async Task<IActionResult> GetResourceContentById(int categoryId, int resourceId)
        {
            var content = await service.GetResourceContentByIdAsync(categoryId, resourceId);
            return base.Content(content, "text/html");
        }

        [HttpPost]
        [Route("{categoryId}/resources")]
        public async Task<IActionResult> AddResource(int categoryId, [FromBody] ResourceRequest request, CancellationToken cancellationToken)
        {
            var resource = await service.AddResourceAsync(categoryId, request, cancellationToken);
            return Ok(resource);
        }

        [HttpPut]
        [Route("{categoryId}/resources")]
        public async Task<IActionResult> UpdateResource(int categoryId, [FromBody] ResourceRequest request, CancellationToken cancellationToken)
        {
            await service.UpdateResourceAsync(categoryId, request, cancellationToken);
            return Ok();
        }

        [HttpPut]
        [Route("{categoryId}/resources/remove/{resourceId}")]
        public async Task<IActionResult> RemoveResource(int categoryId, int resourceId, CancellationToken cancellationToken)
        {
            await service.RemoveResourceByIdAsync(categoryId, resourceId, cancellationToken);
            return Ok();
        }

    }
}
