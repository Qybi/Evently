using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.TicketTypes;
using Evently.Modules.Ticketing.Domain.TicketTypes.Errors;

namespace Evently.Modules.Ticketing.Application.TicketTypes.Commands.UpdateTicketTypePrice;

internal sealed class UpdateTicketTypePriceCommandHandler(
    ITicketTypeRepository ticketTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateTicketTypePriceCommand>
{
    public async Task<Result> Handle(UpdateTicketTypePriceCommand request, CancellationToken cancellationToken)
    {
        TicketType? ticketType = await ticketTypeRepository.GetAsync(request.TicketTypeId, cancellationToken);

        if (ticketType is null)
        {
            return Result.Failure(TicketTypeErrors.NotFound(request.TicketTypeId));
        }

        ticketType.UpdatePrice(request.Price);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
