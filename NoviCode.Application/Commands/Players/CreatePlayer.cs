using NoviCode.Commands;
namespace NoviCode.Commands.Players;

// --- Queries ---
public sealed record GetPlayerByIdQuery(Guid Id) : IQuery<Player?>, ICacheableQuery
{
    public string CacheKey => CacheKeys.Player(Id);
    public TimeSpan CacheTtl => TimeSpan.FromSeconds(60);
}

public sealed record GetAllPlayersQuery() : IQuery<IReadOnlyList<Player>>, ICacheableQuery
{
    public string CacheKey => CacheKeys.AllPlayers;
    public TimeSpan CacheTtl => TimeSpan.FromSeconds(60);
}

// --- Command ---
public sealed record CreatePlayerCommand(string Name, int Score) : ICommand<Player?>;

// --- Query handlers ---
public sealed class GetPlayerByIdHandler : IQueryHandler<GetPlayerByIdQuery, Player?>
{
    private readonly IPlayerRepository _players;
    public GetPlayerByIdHandler(IPlayerRepository players) => _players = players;

    public Task<Player?> HandleAsync(GetPlayerByIdQuery query, CancellationToken ct = default)
        => _players.GetByIdAsync(query.Id, ct);
}

public sealed class GetAllPlayersHandler : IQueryHandler<GetAllPlayersQuery, IReadOnlyList<Player>>
{
    private readonly IPlayerRepository _players;
    public GetAllPlayersHandler(IPlayerRepository players) => _players = players;

    public Task<IReadOnlyList<Player>> HandleAsync(GetAllPlayersQuery query, CancellationToken ct = default)
        => _players.GetAllAsync(ct);
}

// --- Command handler ---
public sealed class CreatePlayerHandler : ICommandHandler<CreatePlayerCommand, Player?>
{
    private readonly IPlayerRepository _players;
    public CreatePlayerHandler(IPlayerRepository players) => _players = players;

    public async Task<Player?> HandleAsync(CreatePlayerCommand command, CancellationToken ct = default)
    {
        var player = new Player(command.Name); // throws on empty name
        player.UpdateScore(command.Score);      // throws on negative score
        await _players.AddAsync(player, ct);
        return player;
    }
}