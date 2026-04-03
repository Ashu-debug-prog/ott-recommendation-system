import { useState } from "react";

const Login = ({ onLogin }) => {
  const [userId, setUserId] = useState("");

  const handleLogin = async () => {
    try {
      const res = await fetch(`https://localhost:7129/api/users/${userId}`);
      if (!res.ok) throw new Error();

      const user = await res.json();
      onLogin(user.userId);
    } catch {
      alert("User not found");
    }
  };

  return (
    <div>
      <h3>Login</h3>
      <input
        placeholder="User ID"
        value={userId}
        onChange={(e) => setUserId(e.target.value)}
      />
      <button onClick={handleLogin}>Login</button>
    </div>
  );
};

export default Login;