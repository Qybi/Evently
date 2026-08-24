using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.Categories.Commands.ArchiveCategory;

public sealed record ArchiveCategoryCommand(Guid CategoryId) : ICommand;
