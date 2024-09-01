using MediatR;
using Application.DTOs;
using Application.Models;

public class GetStudentsQuery : IRequest<PagedList<StudentDTO>>
{
    public PaginationParams PaginationParams { get; set; }
}
