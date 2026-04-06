import { useState } from "react";
import "./SearchBar.css";

const SearchBar = ({ setSelectedMovie }) => {
  const [query, setQuery] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSearch = async () => {
    if (!query.trim()) {
      alert("Enter movie name");
      return;
    }

    setLoading(true);
    try {
      const res = await fetch(
        `https://localhost:7042/api/movies/search?query=${query}`
      );

      const data = await res.json();

      console.log("Search result:", data);

      if (data.length > 0) {
        const foundMovie = data[0];
        console.log("Found movie:", foundMovie);
        alert(`🎬 Found "${foundMovie.title || `Movie ${foundMovie.movieId}`}" - Opening now!`);
        setSelectedMovie({ movieId: foundMovie.movieId, fullObject: foundMovie }); // Pass full object
        setQuery("");
      } else {
        alert("❌ Movie not found");
      }
    } catch (err) {
      console.error(err);
      alert("Error calling API");
    } finally {
      setLoading(false);
    }
  };

  const handleKeyPress = (e) => {
    if (e.key === "Enter") {
      handleSearch();
    }
  };

  return (
    <div className="search-bar-wrapper">
      <div className="search-bar">
        <input
          className="search-input"
          placeholder="🔍 Search for movies..."
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          onKeyPress={handleKeyPress}
          disabled={loading}
        />
        <button 
          className="search-btn" 
          onClick={handleSearch}
          disabled={loading}
        >
          {loading ? "Searching..." : "Search"}
        </button>
      </div>
    </div>
  );
};

export default SearchBar;