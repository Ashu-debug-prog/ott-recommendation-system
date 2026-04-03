import { useState } from "react";

const SearchBar = ({ setSelectedMovie }) => {
  const [query, setQuery] = useState("");

  const handleSearch = async () => {
    if (!query) {
      alert("Enter movie name");
      return;
    }

    try {
      const res = await fetch(
        `https://localhost:7042/api/movies/search?query=${query}`
      );

      const data = await res.json();

      console.log("Search result:", data);

      if (data.length > 0) {
        setSelectedMovie(data[0].movieId); // open first match
      } else {
        alert("❌ Movie not found");
      }
    } catch (err) {
      console.error(err);
      alert("Error calling API");
    }
  };

  return (
    <div>
      <input
        placeholder="Search movie..."
        value={query}
        onChange={(e) => setQuery(e.target.value)}
      />

      <button onClick={handleSearch}>Search</button>
    </div>
  );
};

export default SearchBar;