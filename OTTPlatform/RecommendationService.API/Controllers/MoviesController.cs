using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecommendationService.API.Data;
using RecommendationService.API.Models;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly RecommendationDbContext _context;

    public MoviesController(RecommendationDbContext context)
    {
        _context = context;
    }

    // ✅ GET ALL MOVIES
    [HttpGet]
    public async Task<IActionResult> GetAllMovies()
    {
        var movies = await _context.Movies.ToListAsync();
        return Ok(movies);
    }

    // ✅ SEARCH BY NAME
    [HttpGet("search")]
    public async Task<IActionResult> SearchMovies([FromQuery] string query)
    {
        var movies = await _context.Movies
            .Where(m => m.Title.ToLower().Contains(query.ToLower()))
            .ToListAsync();

        return Ok(movies);
    }
}