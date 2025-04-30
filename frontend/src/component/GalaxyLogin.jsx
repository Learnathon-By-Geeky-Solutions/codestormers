import React, { useState } from "react";
import { Canvas } from "@react-three/fiber";
import { OrbitControls, Stars } from "@react-three/drei";
import { motion } from "framer-motion";
import axiosInstance from "../utils/api/axiosInstance";
import { FaEye, FaEyeSlash } from "react-icons/fa";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";

const GalaxyLogin = () => {
  const [user, setUser] = useState({ email: "", password: "" });
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();
  
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
      toast.error("Please enter a valid Gmail, Yahoo, Hotmail, or Outlook email address");
      setLoading(false);
      return;
    }

    const passwordPattern = /^(?=.*[!@#$%^&*])(?=.*\d).{6,}$/;
    if (!passwordPattern.test(user.password)) {
      toast.error("Password must be at least 6 characters long and include at least one special character and one number");
      setLoading(false);
      return;
    }

    try {
      const response = await axiosInstance.post("/api/Auth/login", user);
      toast.success("Login successful!");
      navigate("/user-dashboard");
    } catch (error) {
      console.error("Login failed:", error.response?.data || error.message);
      toast.error(error.response?.data?.message || "Login failed. Please check your credentials.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="relative flex justify-center items-center w-full h-screen bg-black overflow-hidden">
      {/* Canvas for mobile - reduced star count for performance */}
      <Canvas>
        <Stars 
          radius={50} 
          depth={30} 
          count={window.innerWidth < 768 ? 2000 : 5000} 
          factor={4} 
          fade 
          speed={1} 
        />
        <OrbitControls 
          enableZoom={false} 
          autoRotate 
          autoRotateSpeed={0.5} 
          enablePan={false}
        />
      </Canvas>

      <motion.div
        initial={{ opacity: 0, y: -50 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 1 }}
        className="absolute mx-4 w-full max-w-md p-6 md:p-8 bg-black/80 backdrop-blur-md shadow-lg rounded-xl border border-gray-600 text-white"
      >
        <h2 className="text-2xl font-bold text-center mb-6 text-blue-400">Login to Galaxy</h2>
        <form onSubmit={handleLogin}>
          <div className="mb-4">
            <input
              type="text"
              name="email"
              placeholder="Email"
              value={user.email}
              onChange={userDataChange}
              className="w-full p-3 rounded-md bg-gray-800 text-white border border-gray-600 focus:ring-2 focus:ring-blue-500"
              autoComplete="email"
            />
          </div>
          
          <div className="relative mb-4">
            <input
              type={showPassword ? "text" : "password"}
              name="password"
              placeholder="Password"
              value={user.password}
              onChange={userDataChange}
              className="w-full p-3 pr-10 rounded-md bg-gray-800 text-white border border-gray-600 focus:ring-2 focus:ring-blue-500"
              autoComplete="current-password"
            />
            <button
              type="button"
              className="absolute top-1/2 right-3 transform -translate-y-1/2 text-gray-400 focus:outline-none"
              onClick={() => setShowPassword((prev) => !prev)}
              aria-label={showPassword ? "Hide password" : "Show password"}
            >
              {showPassword ? <FaEyeSlash size={18} /> : <FaEye size={18} />}
            </button>
          </div>

          <button
            type="submit"
            disabled={loading}
            className={`w-full p-3 mt-2 rounded-md transition text-white ${
              loading ? "bg-gray-600 cursor-not-allowed" : "bg-blue-500 hover:bg-blue-600"
            }`}
          >
            {loading ? (
              <span className="flex justify-center items-center">
                <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                  <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Logging in...
              </span>
            ) : "Login"}
          </button>
        </form>

        <p className="mt-4 text-center text-gray-400 text-sm md:text-base">
          Don't have an account?{" "}
          <a href="/signup" className="text-blue-400 hover:underline">
            Sign Up
          </a>
        </p>
      </motion.div>
    </div>
  );
};

export default GalaxyLogin;