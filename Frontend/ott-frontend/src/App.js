import { useState } from "react";
import Login from "./components/Login";
import CreateUser from "./components/CreateUser";
import MovieList from "./components/MovieList";
import MovieDetails from "./components/MovieDetails";
import SearchBar from "./components/SearchBar";
import "./App.css"; // 👈 ADD THIS

function App() {
  const [selectedMovie, setSelectedMovie] = useState(null);
  const [userId, setUserId] = useState(localStorage.getItem("userId"));

  const handleAuth = (id) => {
    localStorage.setItem("userId", id);
    setUserId(id);
  };

  const handleLogout = () => {
    localStorage.removeItem("userId");
    setUserId(null);
  };

  // 🔐 LOGIN SCREEN
  if (!userId) {
    return (
      <div className="auth-container">
        <h1 className="logo">🎬 Movie Clone</h1>

        <div className="auth-box">
          <Login onLogin={handleAuth} />
          <CreateUser onUserCreated={handleAuth} />
        </div>
      </div>
    );
  }

  // 🎬 MAIN APP
  return (
    <div className="app-container">

      {/* 🔥 Navbar */}
<nav className="navbar">
  <h2 className="logo">MyOTT</h2>
  <button className="logout-btn" onClick={handleLogout}>
    Logout
  </button>
</nav>

      {/* 🔍 Search */}
<div className="search-container">
  <SearchBar setSelectedMovie={setSelectedMovie} />
</div>

      {/* 🎥 Content */}
      <div className="content">
        {!selectedMovie ? (
          <>
            <h3 className="section-title">Recommended Movies</h3>
            <MovieList setSelectedMovie={setSelectedMovie} />
          </>
        ) : (
          <MovieDetails movieId={selectedMovie} />
        )}
      </div>
    </div>
    
  );
}

export default App;