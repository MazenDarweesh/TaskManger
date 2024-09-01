using MediatR;
using Application.DTOs;
using Application;
using Application.IServices;

public class GetStudentsQuery : IRequest<PagedList<StudentDTO>>
{
    public PaginationDTO PaginationParams { get; set; }
}


public class GetStudentsQueryHandler(IStudentService studentService) : IRequestHandler<GetStudentsQuery, PagedList<StudentDTO>>
{
    private readonly IStudentService _studentService;
    public async Task<PagedList<StudentDTO>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
    {
        return await _studentService.GetAllStudentsAsync(request.PaginationParams);
    }
}
