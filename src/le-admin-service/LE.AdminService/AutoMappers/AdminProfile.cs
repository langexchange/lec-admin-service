using AutoMapper;
using LE.AdminService.Infrastructure.Infrastructure.Entities;
using LE.AdminService.Models.Requests;
using LE.AdminService.Models.Responses;

namespace LE.AdminService.AutoMappers
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<Admin, AuthResponse>()
                .ForMember(d => d.Id, s => s.MapFrom(x => x.Adminid));

            CreateMap<RegisterRequest, Admin>();
        }
    }
}
