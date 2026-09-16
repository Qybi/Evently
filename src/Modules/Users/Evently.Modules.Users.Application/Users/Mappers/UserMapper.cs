using Evently.Modules.Users.Application.Users.Queries.ViewModels;
using Evently.Modules.Users.Domain.Users;
using Riok.Mapperly.Abstractions;

namespace Evently.Modules.Users.Application.Users.Mappers;

[Mapper]
public static partial class UserMapper
{
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial UserViewModel ToViewModel(this User user);
    public static partial IQueryable<UserViewModel> ProjectToViewModel(this IQueryable<User> users);
}
