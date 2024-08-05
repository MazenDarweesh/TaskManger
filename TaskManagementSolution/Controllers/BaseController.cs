using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace TaskManagementSolution.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected void LogInformation(string message, params object[] args)
    {
        Log.Information(message, args);
    }

    protected void LogWarning(string message, params object[] args)
    {
        Log.Warning(message, args);
    }

    protected void LogError(string message, params object[] args)
    {
        Log.Error(message, args);
    }
}

