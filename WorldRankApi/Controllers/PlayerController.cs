using Microsoft.AspNetCore.Mvc;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayersController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var result = _playerRepository
                    .GetAllPlayers()
                    .ToList();

                if (result.Count == 0)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{playerId:int}")]
        public IActionResult GetPlayerById(int playerId)
        {
            try
            {
                var result = _playerRepository.FindPlayer(playerId);

                if (result is null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}