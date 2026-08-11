using System;
using System.Collections.Generic;
using System.Text;

namespace LowTideEnt.Application.Services
{
    public class BaseRequest
    {
        public int Id { get; set; } = 0;
        public Status Status { get; set; } = Status.Active;
        public DateTime RequestDate { get; } = DateTime.Now;
    }
}
