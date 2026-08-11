using System;
using System.Collections.Generic;
using System.Text;

namespace LowTideEnt.Application.Services.Home.Dto
{
    public class MenuResponse
    {
        public IEnumerable<MenuItem> ResourceMenu { get; set; }
    }
    public class MenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<MenuItem>? SubMenu { get; set; }
    }
}
