using AutoMapper;
using LE.AdminService.Dtos;
using LE.AdminService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Dtos;

namespace LE.AdminService.AutoMappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                 .ForMember(d => d.Avatar, s => s.MapFrom(x => x.Avartar))
                 .ForMember(d => d.Id, s => s.MapFrom(x => x.Userid));

            CreateMap<Language, LanguageDto>()
               .ForMember(d => d.Id, s => s.MapFrom(x => x.Langid))
               .ReverseMap();
            CreateMap<Targetlang, LanguageDto>()
               .ForMember(d => d.Id, s => s.MapFrom(x => x.Langid))
               .ForMember(d => d.Level, s => s.MapFrom(x => x.TargetLevel))
               .ReverseMap();
        }
    }
}
