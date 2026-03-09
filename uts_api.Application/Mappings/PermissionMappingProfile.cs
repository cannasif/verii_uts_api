using AutoMapper;
using uts_api.Application.DTOs.PermissionDefinitions;
using uts_api.Application.DTOs.PermissionGroups;
using uts_api.Application.DTOs.UserPermissionGroups;
using uts_api.Domain.Entities;

namespace uts_api.Application.Mappings;

public sealed class PermissionMappingProfile : Profile
{
    public PermissionMappingProfile()
    {
        CreateMap<PermissionDefinition, PermissionDefinitionDto>();

        CreateMap<PermissionGroup, PermissionGroupListItemDto>()
            .ForMember(dest => dest.PermissionCount, opt => opt.MapFrom(src => src.PermissionGroupPermissionDefinitions.Count));

        CreateMap<PermissionGroup, PermissionGroupDetailDto>()
            .ForMember(dest => dest.PermissionDefinitionIds, opt => opt.MapFrom(src => src.PermissionGroupPermissionDefinitions.Select(x => x.PermissionDefinitionId)));

        CreateMap<PermissionGroup, UserPermissionGroupDto>()
            .ForMember(dest => dest.IsAssigned, opt => opt.Ignore());

        CreateMap<CreatePermissionGroupRequestDto, PermissionGroup>()
            .ForMember(dest => dest.NormalizedName, opt => opt.Ignore())
            .ForMember(dest => dest.PermissionGroupPermissionDefinitions, opt => opt.Ignore())
            .ForMember(dest => dest.UserPermissionGroups, opt => opt.Ignore());
    }
}
