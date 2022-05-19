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

        [HttpGet("users/{username}")]
        public async Task<IActionResult>GetUser(string username)
        {
            var user = await _service.GetUser(username);

            return user != null ? Ok(user) : NotFound(username);
        }

        [HttpGet("orgs/{orgName}")]
        public async Task<IActionResult> GetUsers(string orgName)
        {
            var users = await _service.GetUsers(orgName);

            return users != null ? Ok(users) : NotFound(orgName);
        }
    }
}
