using Microsoft.AspNetCore.Mvc;

namespace MLMS.API.Common;

[ApiController]
[Route("api/[controller]")]
public class TestController(IWebHostEnvironment env) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(env.WebRootPath);
    }
}