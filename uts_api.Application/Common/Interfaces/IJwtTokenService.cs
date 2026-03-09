using uts_api.Application.DTOs.Auth;
using uts_api.Domain.Entities;

namespace uts_api.Application.Common.Interfaces;

public interface IJwtTokenService
{
    AuthTokenDto CreateToken(User user, IReadOnlyCollection<string> permissions);
}
