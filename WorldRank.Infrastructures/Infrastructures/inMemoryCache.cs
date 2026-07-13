using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using WorldRank.Application.Interfaces;
using WorldRank.Domains.Entities;

namespace WorldRank.Infrastructure.Caching;

public class CachedPlayerRepository : IPlayerRepository
{
    private readonly IPlayerRepository _players;   // the real repo it wraps
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachedPlayerRepository> _logger;
    private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(5);

    private const string CacheKey = "players";

    public CachedPlayerRepository(IPlayerRepository players, IMemoryCache cache, ILogger<CachedPlayerRepository> logger)
    {
        _players = players;
        _cache = cache;
        _logger = logger;
    }

    // Optional async helper used by other consumers in this assembly.
    public Task<IReadOnlyList<Player>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = _cache.GetOrCreate(CacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = Ttl;
            return _players.GetAllPlayers().ToList().AsReadOnly();
        });

        // Ensure return type is IReadOnlyList<Player>
        if (list is IReadOnlyList<Player> roList)
            return Task.FromResult(roList);

        // If cache held a List<Player> due to previous value, convert.
        var converted = (list as IEnumerable<Player>)?.ToList().AsReadOnly() ?? Array.Empty<Player>().ToList().AsReadOnly();
        _cache.Set(CacheKey, converted, Ttl);
        return Task.FromResult((IReadOnlyList<Player>)converted);
    }

    // IPlayerRepository implementation - delegate to wrapped repo and keep cache coherent
    public void AddPlayer(Player player)
    {
        _players.AddPlayer(player);
        _cache.Remove(CacheKey);
        _logger.LogInformation("Player {PlayerId} cached repo: invalidated cache after add", player.ID);
    }

    public IEnumerable<Player> GetAllPlayers()
    {
        var list = _cache.GetOrCreate(CacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = Ttl;
            return _players.GetAllPlayers().ToList();
        });

        return (IEnumerable<Player>)list;
    }

    public void DeletePlayer(int playerId)
    {
        _players.DeletePlayer(playerId);
        _cache.Remove(CacheKey);
        _logger.LogInformation("Player {PlayerId} deleted: invalidated cache", playerId);
    }

    public Player? FindPlayer(int playerId)
    {
        // Try to find in cache first
        var cached = _cache.Get<IEnumerable<Player>>(CacheKey);
        if (cached != null)
        {
            return cached.FirstOrDefault(p => p.ID == playerId);
        }

        return _players.FindPlayer(playerId);
    }

    public IEnumerable<IGrouping<int, Player>> GroupPlayersByScore()
    {
        // Delegate to underlying repository; grouping is cheap and likely dynamic.
        return _players.GroupPlayersByScore();
    }
}
