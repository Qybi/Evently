using Evently.Modules.Events.Application.Abstractions.Messaging;

namespace Evently.Modules.Events.Application.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(string Name) : ICommand<Guid>;
