using App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGithubService _service;

        public UserController(IGithubService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult>Get(string username)
        {
            var user = await _service.Get(username);

            return Ok(user);
        }
    }
}
