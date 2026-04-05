using System;
using Remp.Models.Entities;
using Remp.Service.DTOs;
using AutoMapper;

namespace Remp.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserCreateDto, User>();
    }
}
