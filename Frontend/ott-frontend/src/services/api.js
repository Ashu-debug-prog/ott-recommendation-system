const BASE_URL = "https://localhost:7042/api/recommendations";

export const getNextMovie = async (movieId) => {
  const res = await fetch(`${BASE_URL}/next/${movieId}`);
  return res.json();
};

export const getRecommendations = async (movieId) => {
  const res = await fetch(`${BASE_URL}/${movieId}`);
  return res.json();
};