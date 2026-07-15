using Microsoft.Extensions.Logging;
using NoviCode.Commands;
namespace NoviCode.Decorators;

public sealed class LoggingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _inner;
    private readonly ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> _logger;

    public LoggingQueryHandlerDecorator(
        IQueryHandler<TQuery, TResult> inner,
        ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TQuery query, CancellationToken ct = default)
    {
        _logger.LogInformation("Query {Query} started", typeof(TQuery).Name);
        var result = await _inner.HandleAsync(query, ct);
        _logger.LogInformation("Query {Query} completed", typeof(TQuery).Name);
        return result;
    }
}

public sealed class LoggingCommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _inner;
    private readonly ILogger<LoggingCommandHandlerDecorator<TCommand, TResult>> _logger;

    public LoggingCommandHandlerDecorator(
        ICommandHandler<TCommand, TResult> inner,
        ILogger<LoggingCommandHandlerDecorator<TCommand, TResult>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken ct = default)
    {
        _logger.LogInformation("Command {Command} started", typeof(TCommand).Name);
        var result = await _inner.HandleAsync(command, ct);
        _logger.LogInformation("Command {Command} completed", typeof(TCommand).Name);
        return result;
    }
}