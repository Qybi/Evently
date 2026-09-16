using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Ticketing.Infrastructure.Database.Configurations;

internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.HasOne<TicketType>().WithMany().HasForeignKey(oi => oi.TicketTypeId);
    }
}
