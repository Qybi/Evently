using Evently.Shared.Domain;
using MediatR;

namespace Evently.Shared.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
