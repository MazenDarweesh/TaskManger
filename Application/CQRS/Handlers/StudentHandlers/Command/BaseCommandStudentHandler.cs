using Application.IServices;

public abstract class BaseCommandStudentHandler
{
    protected readonly IStudentService _studentService;

    protected BaseCommandStudentHandler(IStudentService studentService)
    {
        _studentService = studentService;
    }
}
