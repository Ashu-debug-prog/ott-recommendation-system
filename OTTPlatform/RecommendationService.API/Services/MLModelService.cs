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

    // 🔥 TRAIN MODEL
    public void TrainModel()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
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
                Console.WriteLine("❌ No training data available");
                return;
            }

            var trainingData = _mlContext.Data.LoadFromEnumerable(data);

            // 🔥 Convert IDs → Keys
            var dataProcessPipeline = _mlContext.Transforms.Conversion
                .MapValueToKey(
                    outputColumnName: "userIdEncoded",
                    inputColumnName: nameof(MovieRating.userId))
                .Append(_mlContext.Transforms.Conversion
                    .MapValueToKey(
                        outputColumnName: "movieIdEncoded",
                        inputColumnName: nameof(MovieRating.movieId)));

            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "movieIdEncoded",
                LabelColumnName = nameof(MovieRating.Label),
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            var estimator = dataProcessPipeline.Append(
                _mlContext.Recommendation()
                    .Trainers.MatrixFactorization(options)
            );

            _model = estimator.Fit(trainingData);

            Console.WriteLine("✅ ML Model trained successfully");
        }
    }

    // 🔥 PREDICT SCORE
    public float Predict(int userId, int movieId)
    {
        if (_model == null)
        {
            Console.WriteLine("⚠️ Model not trained yet");
            return 0;
        }

        var predictionEngine = _mlContext.Model
            .CreatePredictionEngine<MovieRating, MoviePrediction>(_model);

        var prediction = predictionEngine.Predict(new MovieRating
        {
            userId = (float)userId,
            movieId = (float)movieId
        });

        return prediction.Score;
    }
}