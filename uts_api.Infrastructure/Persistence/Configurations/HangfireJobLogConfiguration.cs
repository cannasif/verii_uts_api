using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Configurations;

public sealed class HangfireJobLogConfiguration : IEntityTypeConfiguration<HangfireJobLog>
{
    public void Configure(EntityTypeBuilder<HangfireJobLog> builder)
    {
        builder.ToTable("RII_HANGFIRE_JOB_LOGS");
        builder.ConfigureBaseColumns();

        builder.Property(x => x.JobId).HasColumnName("JobId").HasMaxLength(100).IsRequired();
        builder.Property(x => x.JobName).HasColumnName("JobName").HasMaxLength(500).IsRequired();
        builder.Property(x => x.State).HasColumnName("State").HasMaxLength(50).IsRequired();
        builder.Property(x => x.OccurredAtUtc).HasColumnName("OccurredAtUtc").IsRequired();
        builder.Property(x => x.Reason).HasColumnName("Reason").HasMaxLength(2000);
        builder.Property(x => x.ExceptionType).HasColumnName("ExceptionType").HasMaxLength(500);
        builder.Property(x => x.ExceptionMessage).HasColumnName("ExceptionMessage").HasMaxLength(4000);
        builder.Property(x => x.StackTrace).HasColumnName("StackTrace").HasMaxLength(8000);
        builder.Property(x => x.Queue).HasColumnName("Queue").HasMaxLength(100);
        builder.Property(x => x.RetryCount).HasColumnName("RetryCount").IsRequired();

        builder.HasIndex(x => x.JobId).HasDatabaseName("IX_HangfireJobLog_JobId");
        builder.HasIndex(x => x.JobName).HasDatabaseName("IX_HangfireJobLog_JobName");
        builder.HasIndex(x => x.OccurredAtUtc).HasDatabaseName("IX_HangfireJobLog_OccurredAtUtc");
        builder.HasIndex(x => x.State).HasDatabaseName("IX_HangfireJobLog_State");
    }
}
