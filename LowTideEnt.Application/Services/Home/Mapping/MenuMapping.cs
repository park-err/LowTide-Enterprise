using LowTideEnt.Application.Services.Home.Dto;
using LowTideEnt.Application.Services.Resource.Dto;
using LowTideEnt.Domain.Entities.ResourceManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace LowTideEnt.Application.Services.Home.Mapping
{
    public static class MenuMapping
    {
        public static MenuItem CategoryToSubMenuItem(this CategoryResponse model) => 
            new MenuItem
            {
                Id = model.Id,
                Title = model.Name,
                SubMenu = null
            };

        public static MenuItem CategoryToMenuItem(this CategoryResponse model, IEnumerable<MenuItem> subMenu) =>
            new MenuItem
            {
                Id = model.Id,
                Title = model.Name,
                SubMenu = subMenu
            };
    }
}
