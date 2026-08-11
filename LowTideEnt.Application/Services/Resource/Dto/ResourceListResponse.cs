namespace LowTideEnt.Application.Services.Resource.Dto
{
    public class ResourceListResponse
    {
        public int CategoryId { get; set; }
        public required string CategoryName { get; set; }
        public IEnumerable<Resource>? Resources { get; set; } 
        public IEnumerable<ResourceListResponse>? ChildList { get; set; }
    }
    public class Resource
    {
        public int Id { get; set; }
        public required string Title { get; set; }
    }
}
