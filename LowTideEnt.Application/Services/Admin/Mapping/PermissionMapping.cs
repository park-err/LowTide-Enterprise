using LowTideEnt.Application.Services.Admin.Dto;

namespace LowTideEnt.Application.Services.Admin.Mapping
{
    public static class PermissionMapping
    {
        public static IEnumerable<Permission> ToResponse(this IEnumerable<PermissionModel> model)
        {
            return model.GroupBy(x => x.Name).Select(x => new Permission
            {
                Name = x.Key,
                Types = x.Select(y => y.Type).ToArray()
            });
        }
    }
}
