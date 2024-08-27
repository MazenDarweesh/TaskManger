using Application.IServices;

public abstract class BaseCommandTaskHandler
{
    protected readonly ITaskService _taskService;

    protected BaseCommandTaskHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }
}
