using AutoMapper;
using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.DTOs.Smtp;
using uts_api.Application.Interfaces;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Services;

public sealed class SmtpSettingService : ISmtpSettingService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public SmtpSettingService(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<SmtpSettingDto?> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.SmtpSettings
            .AsNoTracking()
            .OrderByDescending(x => x.IsActive)
            .ThenByDescending(x => x.CreatedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        return entity is null ? null : _mapper.Map<SmtpSettingDto>(entity);
    }

    public async Task<SmtpSettingDto> UpsertAsync(SmtpSettingUpsertRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.SmtpSettings.FirstOrDefaultAsync(cancellationToken);
        if (entity is null)
        {
            entity = new SmtpSetting();
            _dbContext.SmtpSettings.Add(entity);
        }

        _mapper.Map(request, entity);
        entity.Password = string.IsNullOrWhiteSpace(request.Password) ? entity.Password : request.Password.Trim();

        if (request.IsActive)
        {
            var others = await _dbContext.SmtpSettings
                .Where(x => x.Id != entity.Id && x.IsActive)
                .ToListAsync(cancellationToken);

            foreach (var other in others)
            {
                other.IsActive = false;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<SmtpSettingDto>(entity);
    }
}
