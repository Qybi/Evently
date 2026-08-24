using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(string Name) : ICommand<Guid>;
