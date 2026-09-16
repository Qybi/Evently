using Evently.Modules.Users.Application.Users.Queries.ViewModels;
using Evently.Modules.Users.Domain.Users.Errors;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Users.Application.Users.Queries.GetUser;

internal sealed class GetUserQueryHandler(IUserQueries userQueries) : IQueryHandler<GetUserQuery, UserViewModel>
{
    public async Task<Result<UserViewModel>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        UserViewModel? userViewModel = await userQueries.GetAsync(request.UserId, cancellationToken);

        if (userViewModel is null)
        {
            return Result.Failure<UserViewModel>(UserErrors.NotFound(request.UserId));
        }

        return userViewModel;
    }
}
