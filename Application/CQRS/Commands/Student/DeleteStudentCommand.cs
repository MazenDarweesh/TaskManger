using MediatR;

public class DeleteStudentCommand : IRequest
{
    public string Id { get; set; }
}
