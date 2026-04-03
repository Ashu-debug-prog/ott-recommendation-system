import React, { useEffect, useState } from "react";
import MovieCard from "../components/MovieCard";

const Home = () => {
  const [movies, setMovies] = useState([]);

  useEffect(() => {
    // Dummy data (replace with API later)
    const data = [
      { movieId: 1, title: "Inception", score: 5 },
      { movieId: 2, title: "Interstellar", score: 4 },
      { movieId: 3, title: "Dark Knight", score: 5 },
      { movieId: 4, title: "Tenet", score: 3 },
    ];

    setMovies(data);
  }, []);

  return (
    <div style={{ padding: "20px" }}>
      <h2 style={{ color: "white" }}>Recommended Movies</h2>

      <div style={{
        display: "flex",
        flexWrap: "wrap",
        gap: "15px",
        marginTop: "20px"
      }}>
        {movies.map((m) => (
          <MovieCard key={m.movieId} movie={m} />
        ))}
      </div>
    </div>
  );
};

export default Home;