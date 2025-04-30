import React from "react";
import { BsStars } from "react-icons/bs";
import { FaRocket, FaSatellite } from "react-icons/fa";
import { GiOrbital, GiRingedPlanet } from "react-icons/gi";

const WelcomeView = () => {
  return (
    <div className="flex-1 relative h-full overflow-hidden bg-gray-900 text-gray-300 flex items-center justify-center px-4 sm:px-6 lg:px-8">
      {/* Galaxy Background Elements */}
      <div className="absolute inset-0 overflow-hidden">
        {/* Stars - Reduced number on mobile */}
        {[...Array(window.innerWidth < 640 ? 50 : 100)].map((_, i) => (
          <div
            key={i}
            className="absolute rounded-full bg-white animate-pulse"
            style={{
              width: `${Math.random() * 3}px`,
              height: `${Math.random() * 3}px`,
              left: `${Math.random() * 100}%`,
              top: `${Math.random() * 100}%`,
              opacity: Math.random() * 0.7 + 0.1,
              animationDuration: `${Math.random() * 5 + 3}s`,
              animationDelay: `${Math.random() * 2}s`
            }}
          />
        ))}

        {/* Nebula Effects - Smaller on mobile */}
        <div className="absolute w-64 h-64 sm:w-96 sm:h-96 rounded-full bg-purple-900/20 blur-3xl -left-32 sm:-left-64 -top-32 sm:-top-64 animate-pulse duration-[15s]" />
        <div className="absolute w-64 h-64 sm:w-96 sm:h-96 rounded-full bg-blue-900/20 blur-3xl right-0 bottom-0 animate-pulse duration-[20s] delay-[5s]" />

        {/* Animated Planets - Adjusted positioning for mobile */}
        <div className="absolute left-[15%] sm:left-1/4 top-[30%] sm:top-1/3 animate-float duration-[25s]">
          <GiRingedPlanet className="text-4xl sm:text-6xl text-blue-400" />
        </div>
        
        <div className="absolute right-[15%] sm:right-1/4 bottom-[30%] sm:bottom-1/3 animate-float duration-[30s] delay-[3s]">
          <GiRingedPlanet className="text-3xl sm:text-4xl text-orange-400" />
        </div>

        {/* Animated Spacecraft - Adjusted for mobile */}
        <div className="absolute left-[20%] sm:left-1/3 top-[20%] sm:top-1/4 animate-float-fast duration-[15s] delay-[2s]">
          <FaRocket className="text-xl sm:text-2xl text-yellow-400 transform rotate-45" />
        </div>
        
        <div className="absolute right-[20%] sm:right-1/3 bottom-[20%] sm:bottom-1/4 animate-float-fast duration-[18s]">
          <FaSatellite className="text-lg sm:text-xl text-purple-300" />
        </div>
      </div>

      {/* Welcome Content - Responsive adjustments */}
      <div className="relative z-10 text-center w-full max-w-xs sm:max-w-sm md:max-w-2xl px-4 backdrop-blur-sm bg-gray-900/50 rounded-xl py-6 sm:p-8 border border-gray-700 mx-2 sm:mx-0">
        <BsStars className="mx-auto text-4xl sm:text-5xl md:text-6xl text-purple-400 mb-4 sm:mb-6 animate-pulse duration-[3s]" />
        
        {/* Responsive title with character gradients */}
        <h2 className="text-2xl sm:text-3xl md:text-4xl font-bold mb-3 sm:mb-4">
          <span className="inline-block tracking-wider">
            <span className="bg-gradient-to-r from-purple-400 to-blue-400 bg-clip-text text-transparent">
              {' Welcome to'}
            </span>
            {'CosmoVerse'.split('').map((char, i) => {
              const colors = [
                'from-purple-500 to-pink-500',
                'from-blue-400 to-cyan-400',
                'from-green-400 to-teal-400',
                'from-yellow-400 to-orange-400',
                'from-red-400 to-pink-500'
              ];
              return (
                <span 
                  key={i}
                  className={`inline-block mx-0.5 bg-gradient-to-r ${colors[i % colors.length]} bg-clip-text text-transparent`}
                >
                  {char === ' ' ? '\u00A0' : char}
                </span>
              );
            })}
          </span>
          <span className="bg-gradient-to-r from-purple-400 to-blue-400 bg-clip-text text-transparent">
            {' Space Explorer'}
          </span>
        </h2>
        
        <p className="text-base sm:text-lg md:text-xl text-gray-300 mb-6 sm:mb-8 px-2 sm:px-0">
          Select a celestial object from the sidebar to begin your journey
        </p>
        
        {/* Responsive icon grid */}
        <div className="grid grid-cols-3 gap-2 sm:flex sm:justify-center sm:space-x-4">
          <div className="flex flex-col sm:flex-row items-center text-blue-300 text-sm sm:text-base">
            <GiOrbital className="mb-1 sm:mb-0 sm:mr-2 text-lg sm:text-xl" />
            <span>Planets</span>
          </div>
          <div className="flex flex-col sm:flex-row items-center text-yellow-300 text-sm sm:text-base">
            <BsStars className="mb-1 sm:mb-0 sm:mr-2 text-lg sm:text-xl" />
            <span>Stars</span>
          </div>
          <div className="flex flex-col sm:flex-row items-center text-purple-300 text-sm sm:text-base">
            <FaSatellite className="mb-1 sm:mb-0 sm:mr-2 text-lg sm:text-xl" />
            <span>Satellites</span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default WelcomeView;