﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using VacationManager.Models;
using BusinessManager.Data;
using BusinessManager.Services.DTOs.Projects;
using BusinessManager.Services.DTOs.Roles;
using BusinessManager.Services.DTOs.Teams;
using BusinessManager.Services.DTOs.TimeOffs;
using BusinessManager.Services.DTOs.Users;

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

            CreateMap<IdentityRole, BaseRoleDto>();
            CreateMap<IdentityRole, RoleDto>();
            CreateMap<RoleDto, BaseRoleDto>();
            CreateMap<BaseRoleDto, RoleDto>();

            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();
            CreateMap<CreateProjectDto, Project>();

            CreateMap<Team, TeamDto>();
            CreateMap<TeamDto, Team>();
            CreateMap<CreateTeamDto, Team>();

            CreateMap<TimeOff, TimeOffRequestDto>();
            CreateMap<TimeOffRequestDto, TimeOff>();
            CreateMap<CreateTimeOffRequestDto, TimeOff>();
        }
    }
}
