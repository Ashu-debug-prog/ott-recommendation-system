using System.ComponentModel.DataAnnotations;
namespace RecommendationService.API.Models

{
    public class UserRating
    {
        [Key]
        public int RatingId { get; set; }

        public int UserId { get; set; }

        public int MovieId { get; set; }

        public float Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
