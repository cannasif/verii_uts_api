using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.Users;

namespace uts_api.Application.Interfaces;

public interface IUserService
{
    Task<PagedResult<UserListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<UserDetailDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<UserDetailDto> CreateAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default);
    Task<UserDetailDto> UpdateAsync(long id, UpdateUserRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
