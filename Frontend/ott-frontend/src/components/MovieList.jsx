import { movies } from "../data/movies";
import MovieCard from "./MovieCard";
import "./MovieList.css";

const MovieList = ({ setSelectedMovie }) => {
  return (
    <div className="movie-list-container">
      
      {/* Section Title */}
      <h2 className="section-title">🔥 Trending Now</h2>

      {/* Movie Row */}
      <div className="movie-row">
        {movies.map((m) => (
          <div
            key={m.movieId}
            className="movie-wrapper"
            onClick={() => setSelectedMovie && setSelectedMovie({ movieId: m.movieId, fullObject: m })}
          >
            <MovieCard movie={m} />
          </div>
        ))}
      </div>

    </div>
  );
};

export default MovieList;
