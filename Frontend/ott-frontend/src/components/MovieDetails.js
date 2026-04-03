import { useEffect, useState } from "react";
import { getNextMovie, getRecommendations } from "../services/api";
import RecommendationList from "./RecommendationList";

const MovieDetails = ({ movieId }) => {fetch("/api/movies")
  const [nextMovie, setNextMovie] = useState(null);
  const [recommendations, setRecommendations] = useState([]);

  useEffect(() => {
    getNextMovie(movieId).then(setNextMovie);
    getRecommendations(movieId).then(setRecommendations);
  }, [movieId]);

  const handleWatch = async () => {
    const userId = localStorage.getItem("userId");

    await fetch("https://localhost:7129/api/watchhistory", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        userId: parseInt(userId),
        movieId,
        watchTime: 120
      })
    });

    getRecommendations(movieId).then(setRecommendations);
  };

  return (
    <div>
      <button onClick={handleWatch}>▶ Play</button>
      {nextMovie && <p>Next: {nextMovie.title}</p>}
      <RecommendationList movies={recommendations} />
    </div>
  );
};

export default MovieDetails;