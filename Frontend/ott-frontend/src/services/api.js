import { movies as localMovies } from "../data/movies";

const BASE_URL = "https://localhost:7042/api/recommendations";
const MOVIES_BASE_URL = "https://localhost:7042/api/movies";

// ✅ Get movie details with safe fallback
export const getMovieDetails = async (movieId) => {
  try {
    const res = await fetch(`${MOVIES_BASE_URL}/${movieId}`);

    if (!res.ok) {
      throw new Error("Movie not found");
    }

    return await res.json();
  } catch (error) {
    console.warn("API not available, using local data:", error);

    const localMovie = localMovies.find(
      (m) => m.movieId === parseInt(movieId)
    );

    return (
      localMovie || {
        movieId: parseInt(movieId),
        title: `Movie ${movieId}`,
        videoUrl: "/videos/default.mp4", // 🔥 fallback video
        score: 7.5,
      }
    );
  }
};

// ✅ Get recommendations (ML)
export const getRecommendations = async (userId) => {
  try {
    const res = await fetch(`${BASE_URL}/ml/${userId}`);

    if (!res.ok) throw new Error("API failed");

    const data = await res.json();
    console.log("✅ Recommendations from API:", data);

    // Normalize scores so the UI always gets a number
    return data.map((rec) => ({
      ...rec,
      score: Number(rec.score),
    }));
  } catch (err) {
    console.warn("Using local fallback recommendations");

    // 🔥 fallback recommendations with all fields
    return localMovies.map((m) => ({
      movieId: m.movieId,
      title: m.title,
      score: Number(m.score) || (Math.random() * 5 + 5),
      image: m.image
    }));
  }
};

// ✅ Combine recommendation + details (ensures scores are preserved)
export const getRecommendationsWithDetails = async (userId) => {
  try {
    const recs = await getRecommendations(userId);
    console.log("📊 Raw recommendations from getRecommendations:", recs);

    // Just return the recommendations as-is since they already have titles and scores
    return recs;
  } catch (err) {
    console.error("Error in getRecommendationsWithDetails:", err);
    
    // Fallback to local movies with scores
    return localMovies.map((m) => ({
      movieId: m.movieId,
      title: m.title,
      score: m.score,
      image: m.image
    }));
  }
};