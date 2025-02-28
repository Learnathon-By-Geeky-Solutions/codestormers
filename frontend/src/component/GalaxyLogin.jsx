import React, { useState } from "react";
import { Canvas } from "@react-three/fiber";
import { OrbitControls, Stars } from "@react-three/drei";
import { motion } from "framer-motion";
import axiosInstance from "../utils/api/axiosInstance";
import { FaEye, FaEyeSlash } from "react-icons/fa";

const GalaxyLogin = () => {
  const [user, setUser] = useState({ email: "", password: "" });
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);

  const userDataChange = (e) => {
    const { name, value } = e.target;
    setUser((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleLogin = async (event) => {
    event.preventDefault();
    setLoading(true);

    const emailPattern = /^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com|hotmail\.com|outlook\.com)$/;
    if (!emailPattern.test(user.email)) {
      alert("Please enter a valid Gmail, Yahoo, Hotmail, or Outlook email address");
      setLoading(false);
      return;
    }

    const passwordPattern = /^(?=.*[!@#$%^&*])(?=.*\d).{6,}$/;
    if (!passwordPattern.test(user.password)) {
      alert("Password must be at least 6 characters long and include at least one special character and one number");
      setLoading(false);
      return;
    }

    try {
      const response = await axiosInstance.post("/api/Auth/login", user);
      console.log("Login successful:", response.data);
      alert("Login Successful!");
    } catch (error) {
      console.error("Login failed:", error.response?.data || error.message);
      alert(error.response?.data?.message || "Login failed. Please check your credentials.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="relative w-full h-screen bg-black overflow-hidden">
      <Canvas>
        <Stars radius={100} depth={50} count={5000} factor={4} fade speed={1} />
        <OrbitControls enableZoom={false} autoRotate autoRotateSpeed={0.5} />
      </Canvas>

      <motion.div
        initial={{ opacity: 0, y: -50 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 1 }}
        className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 w-96 p-8 bg-black/80 backdrop-blur-md shadow-lg rounded-xl border border-gray-600 text-white"
      >
        <h2 className="text-2xl font-bold text-center mb-6 text-blue-400">Login to Galaxy</h2>
        <form onSubmit={handleLogin}>
          <input
            type="text"
            name="email"
            placeholder="Email"
            value={user.email}
            onChange={userDataChange}
            className="w-full p-3 mb-4 rounded-md bg-gray-800 text-white border border-gray-600 focus:ring-2 focus:ring-blue-500"
          />
          
          <div className="relative">
            <input
              type={showPassword ? "text" : "password"}
              name="password"
              placeholder="Password"
              value={user.password}
              onChange={userDataChange}
              className="w-full p-3 pr-10 rounded-md bg-gray-800 text-white border border-gray-600 focus:ring-2 focus:ring-blue-500"
            />
            <span
              className="absolute top-1/2 right-3 transform -translate-y-1/2 text-gray-400 cursor-pointer"
              onClick={() => setShowPassword((prev) => !prev)}
            >
              {showPassword ? <FaEyeSlash /> : <FaEye />}
            </span>
          </div>

          <button
            type="submit"
            disabled={loading}
            className={`w-full p-3 mt-4 rounded-md transition text-white ${loading ? "bg-gray-600 cursor-not-allowed" : "bg-blue-500 hover:bg-blue-600"}`}
          >
            {loading ? "Logging in..." : "Login"}
          </button>
        </form>

        <p className="mt-4 text-center text-gray-400">
          Don't have an account? <a href="/signup" className="text-blue-400">Sign Up</a>
        </p>
      </motion.div>
    </div>
  );
};

export default GalaxyLogin;
