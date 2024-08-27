using Application.IServices;

public abstract class BaseQueryStudentHandler
{
    protected readonly IStudentService _studentService;

    protected BaseQueryStudentHandler(IStudentService studentService)
    {
        _studentService = studentService;
    }
}
