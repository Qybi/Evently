using FluentValidation;

namespace Evently.Modules.Events.Application.Events.Queries.SearchEvents;

internal sealed class SearchEventsQueryValidator : AbstractValidator<SearchEventsQuery>
{
    public SearchEventsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0.");
        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("PageSize must be greater than 0.");
    }
}
