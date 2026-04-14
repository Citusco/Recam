using System;
using Remp.Models.Entities;
using Remp.Service.DTOs;
using AutoMapper;

namespace Remp.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterRequestDto, User>();
        CreateMap<RegisterRequestDto, Agent>();
        CreateMap<Agent, UserResponseDto>();
        CreateMap<CreateListingCaseRequestDto, ListingCase>();
        CreateMap<ListingCase, ListingCaseResponseDto>();
        CreateMap<ListingCase, ListingCaseDetailResponseDto>();
    }
}
