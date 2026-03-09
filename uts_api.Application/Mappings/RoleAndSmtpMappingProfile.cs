using AutoMapper;
using uts_api.Application.DTOs.Roles;
using uts_api.Application.DTOs.Smtp;
using uts_api.Domain.Entities;

namespace uts_api.Application.Mappings;

public sealed class RoleAndSmtpMappingProfile : Profile
{
    public RoleAndSmtpMappingProfile()
    {
        CreateMap<Role, RoleDto>();

        CreateMap<SmtpSetting, SmtpSettingDto>();

        CreateMap<SmtpSettingUpsertRequestDto, SmtpSetting>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }
}
