using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VocationManager.Data;
using VocationManager.Services.DTOs;

namespace GlobalConstants.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, BaseUserDto>();
            CreateMap<BaseUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, CreateUserDto>();
            CreateMap<CreateUserDto, ApplicationUser>(); 
        }
    }
}
