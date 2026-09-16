using Evently.Modules.Ticketing.Application.Orders.ViewModels;
using Evently.Modules.Ticketing.Domain.Orders;
using Riok.Mapperly.Abstractions;

namespace Evently.Modules.Ticketing.Application.Orders.Mappers;

[Mapper]
public static partial class OrderMapper
{
    // Order -> single order (detail)
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial GetOrderViewModel ToGetOrderViewModel(this Order order);
    public static partial IQueryable<GetOrderViewModel> ProjectToGetOrderViewModel(this IQueryable<Order> orders);

    // Order -> order list row
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial GetOrdersViewModel ToGetOrdersViewModel(this Order order);
    public static partial IQueryable<GetOrdersViewModel> ProjectToGetOrdersViewModel(this IQueryable<Order> orders);

    // OrderItem -> item row
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    [MapProperty(nameof(OrderItem.Id), nameof(OrderItemViewModel.OrderItemId))]
    public static partial OrderItemViewModel ToOrderItemViewModel(this OrderItem orderItem);
}

