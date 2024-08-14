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
                        .ForMember(dest => dest.Id, src => src.Ignore())
            // Map StudentId from the source (DTO) to StudentId in the destination (Domain)
                        .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => Ulid.Parse(src.StudentId.ToString())));

        CreateMap<Student, StudentDTO>().ReverseMap()
                        .ForMember(dest => dest.Id, src => src.Ignore());
                        

    }
}
   