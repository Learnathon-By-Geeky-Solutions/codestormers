import React, { useState } from "react";
import { X, Menu } from "lucide-react";
import { motion } from "framer-motion";
import { Link, useLocation } from "react-router-dom";
import AtomLogo from "./solar_Model/AtomLogo";

const NavBar = () => {
  const [isOpen, setIsOpen] = useState(false);
  const { pathname } = useLocation();

  console.log(pathname);

  if (pathname === "/user-dashboard" || pathname==="/admin-dashboard") {
    return null;
  }
  return (
    <>
      {/* Navbar */}
      <div className="fixed top-0 left-0 w-full flex items-center justify-between p-4 md:px-10 lg:px-20 bg-black/80 backdrop-blur-lg z-50">
        {/* Logo */}
       <Link to="/"> <div className="flex items-center text-white text-xl font-bold">
          <AtomLogo />
        </div></Link>

        {/* Desktop Menu */}
        <div className="hidden md:flex items-center gap-6">
          <Link to="/login">
            <button className="text-white px-4 py-2 border border-white rounded-md hover:bg-white hover:text-black transition">Login</button>
          </Link>
          <Link to="/signup">
            <button className="text-black px-4 py-2 bg-white rounded-md hover:bg-gray-300 transition">Signup</button>
          </Link>
        </div>

        {/* Menu Button for Mobile */}
        <button onClick={() => setIsOpen(true)} className="text-white md:hidden">
          <Menu size={32} />
        </button>
      </div>

      {/* Mobile Menu Overlay */}
      {isOpen && (
        <motion.div
          initial={{ opacity: 0, x: "100%" }}
          animate={{ opacity: 1, x: "0%" }}
          exit={{ opacity: 0, x: "100%" }}
          className="fixed inset-0 bg-black/90 text-white z-50 flex flex-col items-center py-12"
        >
          {/* Close Button */}
          <button onClick={() => setIsOpen(false)} className="absolute top-6 right-6 text-gray-400 hover:text-white">
            <X size={32} />
          </button>

          {/* Menu Content */}
          <div className="flex flex-col items-center gap-6 text-lg font-semibold">
            <Link to="/login">
              <button className="w-full text-center py-3 border border-white rounded-md hover:bg-white hover:text-black transition">Login</button>
            </Link>
            <Link to="/signup">
              <button className="w-full text-center py-3 bg-white text-black rounded-md hover:bg-gray-300 transition">Signup</button>
            </Link>
          </div>
        </motion.div>
      )}
    </>
  );
};

export default NavBar;