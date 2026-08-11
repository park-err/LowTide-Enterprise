using System;
using System.Collections.Generic;
using System.Text;

namespace LowTideEnt.Application.Services.User.Dto
{
    public class UserRequest : BaseRequest
    {
        public required Status StatusId { get; set; }
        public required string Email { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
