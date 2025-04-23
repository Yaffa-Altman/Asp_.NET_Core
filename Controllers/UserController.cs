
using Microsoft.AspNetCore.Mvc;
namespace _2025_02_11.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello from MyNewController!");
    }
}

