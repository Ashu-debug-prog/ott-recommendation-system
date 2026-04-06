using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecommendationService.API.Data;
using RecommendationService.API.Models;
using RecommendationService.API.Services;

namespace RecommendationService.API.Controllers
{
    [ApiController]
    [Route("api/recommendations")]
    public class RecommendationController : ControllerBase
    {
        private readonly RecommendationDbContext _db;
        private readonly MLModelService _mlService; // ✅ ADD THIS

        // ✅ UPDATED CONSTRUCTOR
        public RecommendationController(RecommendationDbContext db, MLModelService mlService)
        {
            _db = db;
            _mlService = mlService;
        }

        // 🎯 1. NEXT MOVIE (SEQUENCE-BASED)
        [HttpGet("next/{movieId}")]
        public async Task<IActionResult> GetNextMovie(int movieId)
        {
            var current = await _db.Movies
                .FirstOrDefaultAsync(m => m.MovieId == movieId);

            if (current == null)
                return NotFound("Movie not found");

            var next = await _db.Movies
                .Where(m => m.SeriesId == current.SeriesId &&
                            m.Sequence == current.Sequence + 1)
                .FirstOrDefaultAsync();

            if (next == null)
                return Ok(new { message = "No next part available" });

            return Ok(next);
        }

        // 🔥 2. HYBRID RECOMMENDATION (EXISTING - KEEP THIS)
        [HttpGet("{movieId}")]
        public async Task<IActionResult> GetRecommendations(int movieId)
        {
            var current = await _db.Movies
                .FirstOrDefaultAsync(m => m.MovieId == movieId);

            if (current == null)
                return NotFound("Movie not found");

            // SAME SERIES
            var seriesMovies = await _db.Movies
                .Where(m => m.SeriesId == current.SeriesId &&
                            m.MovieId != movieId)
                .OrderBy(m => m.Sequence)
                .Select(m => new RecommendationDto
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Score = 100
                })
                .ToListAsync();

            // COLLABORATIVE FILTERING
            var users = await _db.UserWatches
                .Where(x => x.MovieId == movieId)
                .Select(x => x.UserId)
                .Distinct()
                .ToListAsync();

            var collaborative = new List<RecommendationDto>();

            if (users.Any())
            {
                collaborative = await _db.UserWatches
                    .Where(x => users.Contains(x.UserId) && x.MovieId != movieId)
                    .GroupBy(x => x.MovieId)
                    .Select(g => new
                    {
                        MovieId = g.Key,
                        Score = g.Count()
                    })
                    .OrderByDescending(x => x.Score)
                    .Take(5)
                    .Join(_db.Movies,
                          rec => rec.MovieId,
                          movie => movie.MovieId,
                          (rec, movie) => new RecommendationDto
                          {
                              MovieId = movie.MovieId,
                              Title = movie.Title,
                              Score = rec.Score
                          })
                    .ToListAsync();
            }

            var result = seriesMovies
                .Concat(collaborative)
                .GroupBy(x => x.MovieId)
                .Select(g => g.First())
                .OrderByDescending(x => x.Score)
                .Take(6)
                .ToList();

            return Ok(result);
        }

        // 🚀 3. ML-BASED RECOMMENDATION (NEW - MOST IMPORTANT)
        [HttpGet("ml/{userId}")]
        public IActionResult GetMLRecommendations(int userId)
        {
            var watched = _db.UserWatches
                .Where(x => x.UserId == userId)
                .Select(x => x.MovieId)
                .ToList();

            var movies = _db.Movies
                .Where(m => !watched.Contains(m.MovieId) && m.SeriesId == null)
                .ToList();

            var result = new List<object>();

            foreach (var m in movies)
            {
                float score = _mlService.Predict(userId, m.MovieId);

                // 🔥 DOUBLE SAFETY (VERY IMPORTANT)
                if (float.IsNaN(score) || float.IsInfinity(score))
                    score = 0;

                result.Add(new
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Score = score
                });
            }

            return Ok(result
                .OrderByDescending(x => ((dynamic)x).Score)
                .Take(5));
        }
    }
}