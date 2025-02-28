import React, { useRef, useState } from "react";
import { Canvas, useFrame } from "@react-three/fiber";
import { OrbitControls, Stars } from "@react-three/drei";
import { motion } from "framer-motion";
import axios from "axios";
import axiosInstance from "../utils/api/axiosInstance";
import { useNavigate } from "react-router-dom";

const TwinklingStars = () => {
  const groupRef = useRef();
  useFrame(() => {
    groupRef.current.children.forEach((star, index) => {
      star.material.opacity = 0.5 + Math.sin(Date.now() * 0.001 + index) * 0.5;
    });
  });

  return (
    <group ref={groupRef}>
      {Array.from({ length: 500 }).map((_, i) => (
        <mesh
          key={i}
          position={[
            (Math.random() - 0.5) * 20,
            (Math.random() - 0.5) * 20,
            (Math.random() - 0.5) * 20,
          ]}
        >
          <sphereGeometry args={[0.1, 8, 8]} />
          <meshBasicMaterial
            color={"white"}
            transparent
            opacity={Math.random()}
          />
        </mesh>
      ))}
    </group>
  );
};

const GalaxyBackground = () => {
  return (
    <Canvas
      style={{
        position: "absolute",
        top: 0,
        left: 0,
        width: "100%",
        height: "100%",
      }}
    >
      <Stars count={2000} factor={4} fade speed={1} />
      <TwinklingStars />
      <OrbitControls enableZoom={false} autoRotate autoRotateSpeed={0.5} />
    </Canvas>
  );
};

const Signup = () => {
  const [user, setUser] = useState({});
  const navigate=useNavigate();

  const onUserDataChange = (e) => {
    const { name, value } = e.target;
    setUser((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async(e) => {
    e.preventDefault();
    if (
      !user.name ||
      !user.email ||
      !user.password ||
      !user.confirmpassword
    ) {
      alert("Please fill all fields");
      return;
    }

    // Validate email (only Gmail, Yahoo, Hotmail, Outlook)
    const emailPattern =
      /^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com|hotmail\.com|outlook\.com)$/;
    if (!emailPattern.test(user.email)) {
      alert(
        "Please enter a valid Gmail, Yahoo, Hotmail, or Outlook email address"
      );
      return;
    }

    // Validate password (at least 1 special character, 1 number, min length 6)
    const passwordPattern = /^(?=.*[!@#$%^&*])(?=.*\d).{8,}$/;
    if (!passwordPattern.test(user.password)) {
      alert(
        "Password must be at least 8 characters long and include at least one special character and one number"
      );
      return;
    }

    if (user.password !== user.confirmpassword) {
      alert("Password and Confirm Password do not match");
      return;
    }

    try {
      const res =await axiosInstance.post("/api/Auth/register", user);
      if(res.status===201){
        navigate("/login")
      }
    } catch (error) {
      console.log("something went wrong", error);
    }
  };

  return (
    <div className="relative flex items-center justify-center min-h-screen bg-black text-white overflow-hidden">
      <GalaxyBackground />
      <motion.div
        initial={{ opacity: 0, scale: 0.8 }}
        animate={{ opacity: 1, scale: 1 }}
        transition={{ duration: 1 }}
        className="relative z-10 bg-black/80 backdrop-blur-md p-8 rounded-lg shadow-xl max-w-md w-full"
      >
        <h2 className="text-3xl font-bold text-center mb-6">Signup</h2>
        <form className="space-y-4">
          <input
            name="name"
            type="text"
            placeholder="Name"
            className="w-full px-4 py-2 bg-gray-800 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            onChange={onUserDataChange}
          />
          <input
            name="email"
            type="email"
            placeholder="Email"
            className="w-full px-4 py-2 bg-gray-800 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            onChange={onUserDataChange}
          />
          <input
            name="password"
            type="password"
            placeholder="Password"
            className="w-full px-4 py-2 bg-gray-800 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            onChange={onUserDataChange}
          />
          <input
            name="confirmpassword"
            type="password"
            placeholder="Confirm Password"
            className="w-full px-4 py-2 bg-gray-800 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            onChange={onUserDataChange}
          />
          <button
            onClick={handleSubmit}
            className="w-full py-2 bg-blue-600 hover:bg-blue-700 rounded-md text-white font-semibold transition"
          >
            Sign Up
          </button>
        </form>
      </motion.div>
    </div>
  );
};

export default Signup;
