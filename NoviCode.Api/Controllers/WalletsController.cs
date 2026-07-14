using Microsoft.AspNetCore.Mvc;

namespace NoviCode.Api;

[ApiController]
[Route("wallets")]
public class WalletsController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public WalletsController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    // POST /wallets — create a wallet, return 201 Created with a Location header.
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWalletRequest request, CancellationToken cancellationToken)
    {
        var wallet = await _dispatcher.Send(
            new CreateWalletCommand(request.PlayerId, request.Currency), cancellationToken);

        // Create never returns null; the ! documents that invariant to the compiler.
        return CreatedAtAction(nameof(GetById), new { id = wallet!.Id }, WalletResponse.From(wallet));
    }

    // GET /wallets/{id} — caching handled transparently by CachingQueryHandlerDecorator.
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var wallet = await _dispatcher.Ask(new GetWalletByIdQuery(id), cancellationToken);
        return wallet is null ? NotFound() : Ok(WalletResponse.From(wallet));
    }

    // POST /wallets/{id}/deposit — WalletCacheWriteThroughDecorator refreshes the cache on success.
    [HttpPost("{id:guid}/deposit")]
    public async Task<IActionResult> Deposit(Guid id, [FromBody] DepositRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var wallet = await _dispatcher.Send(new DepositCommand(id, request.Amount), cancellationToken);
            return wallet is null ? NotFound() : Ok(WalletResponse.From(wallet));
        }
        catch (WalletException ex)
        {
            // Business-rule violation (blocked wallet, non-positive amount) → 400.
            return BadRequest(new { error = ex.Message });
        }
    }
}
