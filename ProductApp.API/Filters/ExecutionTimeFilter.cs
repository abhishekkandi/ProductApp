using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace ProductApp.API.Filters;

public class ExecutionTimeFilter : IActionFilter
{
    private readonly ILogger<ExecutionTimeFilter> _logger;
    private Stopwatch _stopwatch;

    public ExecutionTimeFilter(ILogger<ExecutionTimeFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _stopwatch = Stopwatch.StartNew();
        var actionName = context.ActionDescriptor.DisplayName;
        _logger.LogInformation("Starting execution of action: {ActionName}", actionName);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _stopwatch.Stop();
        var actionName = context.ActionDescriptor.DisplayName;
        _logger.LogInformation("Finished execution of action: {ActionName} in {ElapsedMilliseconds}ms", 
            actionName, _stopwatch.ElapsedMilliseconds);
    }
}
