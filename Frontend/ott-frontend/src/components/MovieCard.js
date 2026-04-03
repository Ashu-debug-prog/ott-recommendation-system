import React from "react";
import "./MovieCard.css";

const MovieCard = ({ movie }) => {
  return (
    <div className="movie-card">
      <div className="play-btn">▶</div>

      <div className="movie-content">
        <h4>{movie.title}</h4>
        <p>⭐ Score: {movie.score?.toFixed(2)}</p>
      </div>
    </div>
  );
};

export default MovieCard;