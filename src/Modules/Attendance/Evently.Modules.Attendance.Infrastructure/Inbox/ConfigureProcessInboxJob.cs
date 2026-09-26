using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;

namespace Evently.Modules.Attendance.Infrastructure.Inbox;

internal static class ConfigureProcessInboxJob
{
    internal static IQuartzBuilder AddProcessInboxJob(this IQuartzBuilder quartz)
    {
        string jobName = typeof(ProcessInboxJob).FullName!;

        quartz.ScheduleJob<ProcessInboxJob>(
            (serviceProvider, trigger) =>
            {
                InboxOptions inboxOptions = serviceProvider.GetRequiredService<IOptions<InboxOptions>>().Value;

                trigger
                    .WithIdentity(jobName)
                    .WithSimpleSchedule(TimeSpan.FromSeconds(inboxOptions.IntervalInSeconds));
            },
            (_, job) => job.WithIdentity(jobName));

        return quartz;
    }
}
