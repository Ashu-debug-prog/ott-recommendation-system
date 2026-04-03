namespace RecommendationService.API.Models;
public class Movie
{
    public int MovieId { get; set; }
    public string Title { get; set; }


    public int? SeriesId { get; set; }   // ✅ nullable
    public int? Sequence { get; set; }   // ✅ nullable
}
