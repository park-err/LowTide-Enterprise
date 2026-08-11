namespace LowTideEnt.Application.Services.Resource.Dto
{
    public class CategoryResponse : BaseResponse
    {
        public CategoryResponse(int id, string name, int? parentId = null)
        {
            Id = id;
            Name = name;
            ParentId = parentId;
        }

        public int? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<CategoryResponse>? ChildCategories { get; set; } = null;
    }
}
