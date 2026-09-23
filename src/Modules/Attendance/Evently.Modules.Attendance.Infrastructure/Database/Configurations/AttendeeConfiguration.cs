using Evently.Modules.Attendance.Domain.Attendees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Attendance.Infrastructure.Database.Configurations;

internal sealed class AttendeeConfiguration : IEntityTypeConfiguration<Attendee>
{
    public void Configure(EntityTypeBuilder<Attendee> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FirstName).HasMaxLength(200);

        builder.Property(a => a.LastName).HasMaxLength(200);

        builder.Property(a => a.Email).HasMaxLength(300);
    }
}
