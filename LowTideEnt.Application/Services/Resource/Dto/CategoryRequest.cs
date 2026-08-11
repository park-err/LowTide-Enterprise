using System;
using System.Collections.Generic;
using System.Text;

namespace LowTideEnt.Application.Services.Resource.Dto
{
    public class CategoryRequest : BaseRequest
    {
        public int? ParentId { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
