using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Departments;
using MLMS.Domain.Identity;
using MLMS.Domain.Majors;
using MLMS.Infrastructure.Common;
using MLMS.Infrastructure.Identity.Models;

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