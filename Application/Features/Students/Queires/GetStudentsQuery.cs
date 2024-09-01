using MediatR;
using Application.DTOs;
using Application;

public class GetStudentsQuery : IRequest<PagedList<StudentDTO>>
{
    public PaginationDTO PaginationParams { get; set; }
}
