using AutoMapper;
using Rekryteringsassistent.DTO;
using Rekryteringsassistent.Models;

namespace Rekryteringsassistent.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<RegisterUserDto, ApplicationUser>();
        CreateMap<ApplicationUser, LoggedInUserDto>();
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.DateOfBirth, opt => opt
                .MapFrom(src => src.DateOfBirth != null ? src.DateOfBirth.Value.Date : (DateTime?)null));

    }

}
