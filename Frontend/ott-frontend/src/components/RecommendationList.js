import MovieCard from "./MovieCard";
import "./RecommendationList.css";

const RecommendationList = ({ movies, setSelectedMovie }) => {
  if (!movies || movies.length === 0) {
    return null;
  }

  return (
    <div className="recommendation-list">
      <h3 className="recommendation-title">You Might Like</h3>
      <div className="recommendation-grid">
        {movies.map((m) => (
          <div
            key={m.movieId}
            className="recommendation-item"
            onClick={() => setSelectedMovie && setSelectedMovie({ movieId: m.movieId, fullObject: m })}
            style={{ cursor: 'pointer' }}
          >
            <MovieCard movie={m} />
          </div>
        ))}
      </div>
    </div>
  );
};

export default RecommendationList;