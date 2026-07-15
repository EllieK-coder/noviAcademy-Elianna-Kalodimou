namespace NoviCode.Commands;

// A command mutates state and returns the affected entity (used for write-through).
public interface ICommand<TResult> { }

// A query reads state and returns data. Never mutates.
public interface IQuery<TResult> { }

public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}