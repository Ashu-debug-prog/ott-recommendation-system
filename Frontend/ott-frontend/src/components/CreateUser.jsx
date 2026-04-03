import { useState } from "react";

const CreateUser = ({ onUserCreated }) => {
  const [name, setName] = useState("");
  const [age, setAge] = useState("");
  const [language, setLanguage] = useState("");

  const handleCreateUser = async () => {
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

    const userId = await res.json();
    onUserCreated(userId);
  };

  return (
    <div>
      <h3>Create User</h3>
      <input placeholder="Name" onChange={(e) => setName(e.target.value)} />
      <input placeholder="Age" onChange={(e) => setAge(e.target.value)} />
      <input placeholder="Language" onChange={(e) => setLanguage(e.target.value)} />
      <button onClick={handleCreateUser}>Sign Up</button>
    </div>
  );
};

export default CreateUser;