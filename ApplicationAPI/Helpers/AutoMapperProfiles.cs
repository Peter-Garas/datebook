
using ApplicationAPI.DTOs;
using ApplicationAPI.Entities;
using ApplicationAPI.Extensions;
using AutoMapper;

namespace ApplicationAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, 
                    options => options.MapFrom(
                    src => src.Photos.FirstOrDefault(
                    x => x.IsMain).Url))
                .ForMember(dest => dest.Age,
                    options => options.MapFrom(
                    src => src.DateOfBirth.CalculateAge()));

            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, 
                    options => options.MapFrom(
                    src => src.Sender.Photos.FirstOrDefault(
                    x => x.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, 
                    options => options.MapFrom(
                    src => src.Recipient.Photos.FirstOrDefault(
                    x => x.IsMain).Url));
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
        }
    }
}