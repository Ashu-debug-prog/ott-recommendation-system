import { useState } from "react";
import "./Auth.css";

const CreateUser = ({ onUserCreated }) => {
  const [name, setName] = useState("");
  const [age, setAge] = useState("");
  const [language, setLanguage] = useState("English");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleCreateUser = async () => {
    if (!name.trim() || !age.trim() || !language.trim()) {
      setError("Please fill in all fields");
      return;
    }

    setError("");
    setLoading(true);

    try {
      const res = await fetch("https://localhost:7129/api/users", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({
          name,
          age: parseInt(age),
          preferredLanguage: language
        })
      });

      if (!res.ok) throw new Error("Failed to create user");

      const userId = await res.json();
      onUserCreated(userId);
    } catch (err) {
      setError("❌ Failed to create account. Please try again.");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleKeyPress = (e) => {
    if (e.key === "Enter") {
      handleCreateUser();
    }
  };

  return (
    <div className="auth-form">
      <h2 className="auth-title">Create Account</h2>
      <p className="auth-subtitle">Start watching amazing movies</p>

      <div className="form-group">
        <input
          className="form-input"
          placeholder="Full Name"
          value={name}
          onChange={(e) => {
            setName(e.target.value);
            setError("");
          }}
          onKeyPress={handleKeyPress}
          disabled={loading}
          type="text"
        />
      </div>

      <div className="form-group">
        <input
          className="form-input"
          placeholder="Age"
          value={age}
          onChange={(e) => {
            setAge(e.target.value);
            setError("");
          }}
          onKeyPress={handleKeyPress}
          disabled={loading}
          type="number"
          min="1"
          max="120"
        />
      </div>

      <div className="form-group">
        <select
          className="form-input"
          value={language}
          onChange={(e) => {
            setLanguage(e.target.value);
            setError("");
          }}
          disabled={loading}
        >
          <option value="English">English</option>
          <option value="Hindi">Hindi</option>
          <option value="Spanish">Spanish</option>
          <option value="French">French</option>
          <option value="German">German</option>
        </select>
      </div>

      {error && <div className="form-error">{error}</div>}

      <button 
        className="btn-submit" 
        onClick={handleCreateUser}
        disabled={loading}
      >
        {loading ? "Creating Account..." : "Sign Up"}
      </button>
    </div>
  );
};

export default CreateUser;