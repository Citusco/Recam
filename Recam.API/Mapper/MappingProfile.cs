using System;
using Recam.Models.Entities;
using Recam.Models.DTOs;
using AutoMapper;

namespace Recam.API.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserCreateDto, User>();
    }
}
