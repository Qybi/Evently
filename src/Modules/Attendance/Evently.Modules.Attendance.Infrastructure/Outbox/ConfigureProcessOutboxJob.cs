using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;

namespace Evently.Modules.Attendance.Infrastructure.Outbox;

internal static class ConfigureProcessOutboxJob
{
    internal static IQuartzBuilder AddProcessOutboxJob(this IQuartzBuilder quartz)
    {
        string jobName = typeof(ProcessOutboxJob).FullName!;

        quartz.ScheduleJob<ProcessOutboxJob>(
            (serviceProvider, trigger) =>
            {
                OutboxOptions outboxOptions = serviceProvider.GetRequiredService<IOptions<OutboxOptions>>().Value;

                trigger
                    .WithIdentity(jobName)
                    .WithSimpleSchedule(TimeSpan.FromSeconds(outboxOptions.IntervalInSeconds));
            },
            (_, job) => job.WithIdentity(jobName));

        return quartz;
    }
}
