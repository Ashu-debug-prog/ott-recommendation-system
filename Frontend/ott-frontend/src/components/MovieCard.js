import React from "react";
import "./MovieCard.css";

const MovieCard = ({ movie }) => {
  const gradients = [
    "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
    "linear-gradient(135deg, #f093fb 0%, #f5576c 100%)",
    "linear-gradient(135deg, #4facfe 0%, #00f2fe 100%)",
    "linear-gradient(135deg, #43e97b 0%, #38f9d7 100%)",
    "linear-gradient(135deg, #fa709a 0%, #fee140 100%)",
    "linear-gradient(135deg, #30cfd0 0%, #330867 100%)",
    "linear-gradient(135deg, #a8edea 0%, #fed6e3 100%)",
    "linear-gradient(135deg, #ff9a56 0%, #ff6a88 100%)",
  ];

  const gradientIndex = movie.movieId % gradients.length;

  const backgroundStyle = {
    background: movie.posterUrl || gradients[gradientIndex]
  };

  const scoreValue = Number(movie.score);
  const displayScore = Number.isFinite(scoreValue)
    ? scoreValue.toFixed(2)
    : "N/A";

  return (
    <div 
      className="movie-card"
      style={backgroundStyle}
    >
      <div className="play-btn">▶</div>

      <div className="movie-content">
        <h4>{movie.title}</h4>
        <p>⭐ {displayScore}</p>
      </div>
    </div>
  );
};

export default MovieCard;