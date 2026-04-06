import { useState } from "react";
import "./Auth.css";

const Login = ({ onLogin }) => {
  const [userId, setUserId] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleLogin = async () => {
    if (!userId.trim()) {
      setError("Please enter your User ID");
      return;
    }

    setError("");
    setLoading(true);

    try {
      const res = await fetch(`https://localhost:7129/api/users/${userId}`);
      if (!res.ok) throw new Error("User not found");

      const user = await res.json();
      onLogin(user.userId);
    } catch (err) {
      setError("❌ User not found. Please check your ID.");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleKeyPress = (e) => {
    if (e.key === "Enter") {
      handleLogin();
    }
  };

  return (
    <div className="auth-form">
      <h2 className="auth-title">Welcome Back</h2>
      <p className="auth-subtitle">Sign in with your User ID</p>

      <div className="form-group">
        <input
          className="form-input"
          placeholder="Enter your User ID"
          value={userId}
          onChange={(e) => {
            setUserId(e.target.value);
            setError("");
          }}
          onKeyPress={handleKeyPress}
          disabled={loading}
          type="text"
        />
      </div>

      {error && <div className="form-error">{error}</div>}

      <button 
        className="btn-submit" 
        onClick={handleLogin}
        disabled={loading}
      >
        {loading ? "Signing in..." : "Sign In"}
      </button>
    </div>
  );
};

export default Login;