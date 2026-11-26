using Evently.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Users.Infrastructure.Database.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(p => p.Code);

        builder.Property(p => p.Code).HasMaxLength(100);

        // seeding for automatically creating migration with sql data
        builder.HasData(
            Permission.GetUser,
            Permission.ModifyUser,
            Permission.GetEvents,
            Permission.SearchEvents,
            Permission.ModifyEvents,
            Permission.GetTicketTypes,
            Permission.ModifyTicketTypes,
            Permission.GetCategories,
            Permission.ModifyCategories,
            Permission.GetCart,
            Permission.AddToCart,
            Permission.RemoveFromCart,
            Permission.GetOrders,
            Permission.CreateOrder,
            Permission.GetTickets,
            Permission.CheckInTicket,
            Permission.GetEventStatistics);

        builder
            .HasMany<User>()
            .WithMany(u => u.Permissions)
            .UsingEntity(joinBuilder =>
            {
                joinBuilder.ToTable("user_permissions");

                joinBuilder.Property("PermissionsCode").HasColumnName("permission_code");
            });
    }
}
