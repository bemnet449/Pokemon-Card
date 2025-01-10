using Microsoft.AspNetCore.Mvc;
using P.Models;
using P.Services;

namespace PokemonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        // Injected service
        private readonly IPokemonService _pokemonService;

        // Constructor injection of the service
        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        // Add a Pokemon
        [HttpPost("add")]
        public IActionResult AddPokemon(Pokemon pokemon)
        {
            try
            {
                _pokemonService.AddAsync(pokemon); // Delegates to service
                return Ok(new { Message = "Pokemon added successfully!" });
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not shown)
                return StatusCode(500, new { Message = "An error occurred while adding the Pokemon.", Error = ex.Message });
            }
        }

        // Get all Pokemon
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPokemon()
        {
            try
            {
                var pokemons = await _pokemonService.GetAllAsync();
                return Ok(pokemons);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Message = "An error occurred while retrieving the Pokemon.", Error = ex.Message });
            }
        }

        // Get Pokemon by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPokemonById(int id)
        {
            try
            {
                var pokemon = await _pokemonService.GetByIdAsync(id);
                if (pokemon == null)
                {
                    return NotFound(new { Message = "Pokemon not found." });
                }
                return Ok(pokemon);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Message = "An error occurred while retrieving the Pokemon.", Error = ex.Message });
            }
        }

        // Update Pokemon
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdatePokemon(int id, [FromBody] Pokemon updatedPokemon)
        {
            try
            {
                var existingPokemon = await _pokemonService.GetByIdAsync(id);
                if (existingPokemon == null)
                {
                    return NotFound(new { Message = "Pokemon not found." });
                }

                await _pokemonService.UpdateAsync(id, updatedPokemon);
                return Ok(new { Message = "Pokemon updated successfully!" });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Message = "An error occurred while updating the Pokemon.", Error = ex.Message });
            }
        }

        // Delete Pokemon
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePokemonAsync(int id)
        {
            try
            {
                var existingPokemon = await _pokemonService.GetByIdAsync(id);
                if (existingPokemon == null)
                {
                    return NotFound(new { Message = "Pokemon not found." });
                }

                await _pokemonService.DeleteAsync(id);
                return Ok(new { Message = "Pokemon deleted successfully!" });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Message = "An error occurred while deleting the Pokemon.", Error = ex.Message });
            }
        }
    }
}
