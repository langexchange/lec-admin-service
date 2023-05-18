using AutoMapper;
using LE.AdminService.Dtos;
using LE.AdminService.Infrastructure.Infrastructure.Entities;
using LE.AdminService.Models.Requests;
using System;

namespace LE.AdminService.AutoMappers
{
    public class SettingProfile : Profile
    {
        public SettingProfile()
        {
            CreateMap<SettingKeyPairRequest, SettingDto>()
               .ForMember(d => d.Id, s => s.MapFrom(x => Guid.NewGuid()))
               .ForMember(d => d.Key, s => s.MapFrom(x => $"{x.Locale.ToLower()}.{x.Key.ToLower()}"))
               .ForMember(d => d.Value, s => s.MapFrom(x => x.Value));

            CreateMap<SettingDto, Setting>()
               .ForMember(d => d.SettingKey, s => s.MapFrom(x => x.Key))
               .ForMember(d => d.SettingValue, s => s.MapFrom(x => x.Value))
               .ReverseMap();
        }
    }
}
