using Microsoft.Extensions.DependencyInjection;
using NoviCode.Commands;

namespace NoviCode;

public interface IDispatcher
{
    Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken ct = default);
    Task<TResult> Ask<TResult>(IQuery<TResult> query, CancellationToken ct = default);
}

public sealed class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _sp;
    public Dispatcher(IServiceProvider sp) => _sp = sp;

    public Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken ct = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        dynamic handler = _sp.GetRequiredService(handlerType);
        return handler.HandleAsync((dynamic)command, ct);
    }

    public Task<TResult> Ask<TResult>(IQuery<TResult> query, CancellationToken ct = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        dynamic handler = _sp.GetRequiredService(handlerType);
        return handler.HandleAsync((dynamic)query, ct);
    }
}