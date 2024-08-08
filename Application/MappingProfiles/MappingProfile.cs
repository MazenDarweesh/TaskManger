   using AutoMapper;
   using Application.DTOs;
   using Domain.Models;
using Domain.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map TaskDomain to TaskDomainDTO and vice versa
        CreateMap<TaskDomain, TaskDomainDTO>().ReverseMap()
                        .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignore Id during mapping

        CreateMap<Student, StudentDTO>().ReverseMap()
                        .ForMember(dest => dest.Id, opt => opt.Ignore());

    }
}
   