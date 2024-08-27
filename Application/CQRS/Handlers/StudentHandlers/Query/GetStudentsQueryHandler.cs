using Application.IServices;
using Application.DTOs;
using Application.Models;
using MediatR;

public class GetStudentsQueryHandler : BaseQueryStudentHandler, IRequestHandler<GetStudentsQuery, PagedList<StudentDTO>>
{
    public GetStudentsQueryHandler(IStudentService studentService) : base(studentService) { }

    public async Task<PagedList<StudentDTO>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
    {
        return await _studentService.GetAllStudentsAsync(request.PaginationParams);
    }
}
