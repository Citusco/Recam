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
        CreateMap<UpdateListingCaseRequestDto, ListingCase>();
        CreateMap<MediaAsset, CreateMediaAssetResponseDto>();
        CreateMap<MediaAsset, MediaAssetResponseDto>();
        CreateMap<SelectedMedia, SelectMediaResponseDto>();
        CreateMap<AgentListingCase, AgentListingCaseResponseDto>();
        CreateMap<AgentPhotographyCompany, AgentPhotographyCompanyResponseDto>();
        CreateMap<Agent, AgentResponseDto>()
            .IncludeBase<Agent, UserResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
            .ForMember(dest => dest.CompanyNames, opt => opt.MapFrom(
                        src => src.AgentPhotographyCompanies != null
                        ? src.AgentPhotographyCompanies
                        .Select(apc => apc.PhotographyCompany.PhotographyCompanyName)
                        .ToList()
                        : new List<string>()
                        )
                    );
    }
}