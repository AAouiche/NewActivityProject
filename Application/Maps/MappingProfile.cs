using Domain.DTO;
using Domain.Models;
using AutoMapper;

namespace NewActivityProject.Maps
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activity, ActivityDTO>()
                .ForMember(dest => dest.Host, opt => opt.MapFrom(src => src.Attendees.FirstOrDefault(a => a.IsHost)))
                .ForMember(dest => dest.Attendees, opt => opt.MapFrom(src => src.Attendees.Where(a => !a.IsHost))); // Exclude the host from attendees list

            CreateMap<ActivityAttendee, AttendeeDTO>()
    .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
    .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.ApplicationUser.DisplayName))
    .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.ApplicationUser.Biography))
    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ApplicationUser.Image.Url)); // Assumes each user has only one image or you're picking the first/primary image
        }
    
    }
}
