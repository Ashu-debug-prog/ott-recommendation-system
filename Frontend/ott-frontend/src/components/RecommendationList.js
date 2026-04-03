import MovieCard from "./MovieCard";

const RecommendationList = ({ movies, setSelectedMovie }) => {
  return (
    <div>
      <h3>Recommended</h3>
      <div style={{ display: "flex" }}>
        {movies.map((m) => (
          <MovieCard key={m.movieId} movie={m} onClick={setSelectedMovie} />
        ))}
      </div>
    </div>
  );
};

export default RecommendationList;