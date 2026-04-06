import { useEffect, useState } from "react";
import { getNextMovie, getRecommendations, getMovieDetails } from "../services/api";
import { movies as localMovies } from "../data/movies";
import RecommendationList from "./RecommendationList";
import "./MovieDetails.css";

const MovieDetails = ({ movieId, setSelectedMovie }) => {
  const [nextMovie, setNextMovie] = useState(null);
  const [recommendations, setRecommendations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [isPlaying, setIsPlaying] = useState(false);
  const [currentMovie, setCurrentMovie] = useState(null);

  useEffect(() => {
    const fetchMovieData = async () => {
      setLoading(true);
      try {
        // 🔥 Extract ID from either movieId or movieId.movieId if it's an object with fullObject
        let actualMovieId = movieId;
        let movieData = null;
        
        if (typeof movieId === 'object' && movieId.fullObject) {
          // If passed as object with fullObject, use that directly
          movieData = movieId.fullObject;
          actualMovieId = movieId.movieId;
        } else {
          // Otherwise fetch from API
          movieData = await getMovieDetails(actualMovieId);
        }
        
        setCurrentMovie(movieData);

        // Fetch recommendations (always use actual numeric ID)
        try {
          const userId = localStorage.getItem("userId");
          const recommendationsData = await getRecommendations(userId);
          setRecommendations(recommendationsData);
        } catch (err) {
          console.warn("Could not fetch recommendations:", err);
          setRecommendations([]);
        }
      } catch (error) {
        console.error("Error fetching movie data:", error);
        // Fallback to local data if API fails
        const movieIdValue = typeof movieId === 'object' ? movieId.movieId : movieId;
        const localMovie = localMovies.find(m => m.movieId === movieIdValue);
        setCurrentMovie(localMovie || { movieId: movieIdValue, title: `Movie ${movieIdValue}` });
      } finally {
        setLoading(false);
      }
    };

    fetchMovieData();
  }, [movieId]);

  const handleWatch = async () => {
    setIsPlaying(true);
    const userId = localStorage.getItem("userId");
    const movieIdValue = typeof movieId === 'object' ? movieId.movieId : movieId;

    try {
      await fetch("https://localhost:7129/api/watchhistory", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          userId: parseInt(userId),
          movieId: movieIdValue,
          watchTime: 120
        })
      });

      getRecommendations(userId).then(setRecommendations);
    } catch (err) {
      console.error("Error adding to watch history:", err);
    }
  };

  const handleStopPlaying = () => {
    setIsPlaying(false);
  };

  if (loading) {
    return (
      <div className="movie-details-container">
        <div className="loading-skeleton">
          <div className="skeleton-hero"></div>
          <div className="skeleton-content"></div>
        </div>
      </div>
    );
  }

  return (
    <div className="movie-details-container">
      {/* Back Button */}
      <button className="btn-back" onClick={() => setSelectedMovie(null)}>
        ← Back to Movies
      </button>

      {/* Video Player - Show when playing */}
      {isPlaying && (
        <div className="video-player-container">
          <div className="video-player">
            <div className="video-content">
              <div className="play-overlay">
                <div className="play-icon-large">▶</div>
                <h2>Now Playing: {currentMovie?.title || `Movie ${movieId}`}</h2>
                <p>Enjoy your movie experience!</p>
              </div>
            </div>
            <div className="video-controls">
              <button className="btn-stop" onClick={handleStopPlaying}>
                ⏹ Stop Playing
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Hero Banner */}
      <div className="movie-hero-banner">
        <div className="hero-content">
          <div className="hero-info">
            <h1 className="movie-title">{currentMovie?.title || `Movie ${movieId}`}</h1>
            <p className="movie-desc">
              {currentMovie ? "Dive into an amazing cinematic experience with our curated selection." : "Loading movie details..."}
            </p>
            <div className="hero-buttons">
              <button className="btn-primary" onClick={handleWatch} disabled={isPlaying}>
                <span className="play-icon">▶</span> {isPlaying ? "Playing..." : "Watch Now"}
              </button>
              <button className="btn-secondary">
                <span className="add-icon">+</span> My List
              </button>
            </div>
          </div>
        </div>
        <div className="hero-overlay"></div>
      </div>

      {/* Next Episode */}
      {nextMovie && (
        <div className="next-episode">
          <div className="next-content">
            <div className="next-text">
              <h3>Next Episode</h3>
              <p>{nextMovie.title}</p>
            </div>
            <button className="btn-next">
              <span className="play-icon">▶</span> Play
            </button>
          </div>
        </div>
      )}

      {/* Recommendations */}
      <div className="recommendations-section">
        <RecommendationList movies={recommendations} setSelectedMovie={setSelectedMovie} />
      </div>
    </div>
  );
};

export default MovieDetails;