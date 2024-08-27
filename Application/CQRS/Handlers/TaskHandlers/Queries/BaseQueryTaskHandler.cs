using Application.IServices;

public abstract class BaseQueryTaskHandler
{
    protected readonly ITaskService _taskService;

    protected BaseQueryTaskHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }
}
