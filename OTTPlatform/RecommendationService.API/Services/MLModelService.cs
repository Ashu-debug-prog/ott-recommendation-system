using Microsoft.ML;
using Microsoft.ML.Trainers;
using Microsoft.Extensions.DependencyInjection;
using RecommendationService.API.Data;
using RecommendationService.API.Models;

namespace RecommendationService.API.Services;

public class MLModelService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly MLContext _mlContext;
    private ITransformer _model;

    public MLModelService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _mlContext = new MLContext();
    }

    public void TrainModel()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<RecommendationDbContext>();

        var data = db.UserRatings
            .Select(x => new MovieRating
            {
                userId = (float)x.UserId,
                movieId = (float)x.MovieId,
                Label = x.Rating
            })
            .ToList();

        if (!data.Any())
        {
            Console.WriteLine("❌ No training data");
            return;
        }

        var trainingData = _mlContext.Data.LoadFromEnumerable(data);

        var pipeline = _mlContext.Transforms.Conversion
            .MapValueToKey("userIdEncoded", nameof(MovieRating.userId))
            .Append(_mlContext.Transforms.Conversion
                .MapValueToKey("movieIdEncoded", nameof(MovieRating.movieId)))
            .Append(_mlContext.Recommendation()
                .Trainers.MatrixFactorization(new MatrixFactorizationTrainer.Options
                {
                    MatrixColumnIndexColumnName = "userIdEncoded",
                    MatrixRowIndexColumnName = "movieIdEncoded",
                    LabelColumnName = nameof(MovieRating.Label),
                    NumberOfIterations = 20,
                    ApproximationRank = 100
                }));

        _model = pipeline.Fit(trainingData);

        Console.WriteLine("✅ Model trained");
    }

    // 🔥 FINAL SAFE PREDICT
    public float Predict(int userId, int movieId)
    {
        if (_model == null) return 0;

        try
        {
            var engine = _mlContext.Model
                .CreatePredictionEngine<MovieRating, MoviePrediction>(_model);

            var prediction = engine.Predict(new MovieRating
            {
                userId = userId,
                movieId = movieId
            });

            float score = prediction.Score;

            // ✅ GUARANTEE VALID JSON VALUE
            if (float.IsNaN(score) || float.IsInfinity(score))
                return 0;

            return score;
        }
        catch
        {
            return 0;
        }
    }
}