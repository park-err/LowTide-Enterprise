using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LowTideEnt.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Returns the current health status of the API
        /// </summary>
        /// <returns>A JSON object containing the health status</returns>
        [HttpGet]
        public IActionResult GetHealth()
        {
            var healthStatus = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow
            };
            return Ok(healthStatus);
        }
    }
}
