import React, { useState, useEffect } from "react";
import { 
  FaGlobe, 
  FaChevronDown, 
  FaSatellite, 
  FaSpaceShuttle,
  FaSatelliteDish,
  FaMeteor,
  FaAsterisk
} from "react-icons/fa";
import { GiRingedPlanet } from "react-icons/gi";
import AtomLogo from "../solar_Model/AtomLogo";
import axiosInstance from "../../utils/api/axiosInstance";

const Sidebar = ({ onSelect, activeId }) => {
  const [openCelestialMenu, setOpenCelestialMenu] = useState(true);
  const [openPlanetMenu, setOpenPlanetMenu] = useState(true);
  const [openSatelliteMenu, setOpenSatelliteMenu] = useState(true);
  const [celestialBodies, setCelestialBodies] = useState([]);
  const [planets, setPlanets] = useState([]);
  const [satellites, setSatellites] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [satellitePositions, setSatellitePositions] = useState(
    Array(3).fill().map(() => ({
      x: Math.random() * 100,
      y: Math.random() * 100,
      speed: Math.random() * 0.3 + 0.1,
      direction: Math.random() * 360
    }))
  );

  // Icons mapping
  const celestialIcons = {
    planet: <GiRingedPlanet className="mr-2 text-blue-400" />,
    star: <FaAsterisk className="mr-2 text-yellow-400" />,
    asteroid: <FaMeteor className="mr-2 text-gray-400" />,
    comet: <FaSpaceShuttle className="mr-2 text-teal-400" />,
    default: <FaGlobe className="mr-2 text-green-400" />
  };

  const planetIcon = <GiRingedPlanet className="mr-2 text-red-400" />;
  const satelliteIcon = <FaSatellite className="mr-2 text-purple-400" />;

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        
        // Fetch all data in parallel
        const [celestialResponse, planetResponse, satelliteResponse] = await Promise.all([
          axiosInstance.get('api/Celestial/get-all-celestials'),
          axiosInstance.get('api/Planet/get-all-planets'),
          axiosInstance.get('api/Satellite/get-all-satellites')
        ]);

        setCelestialBodies(celestialResponse.data);
        setPlanets(planetResponse.data);
        setSatellites(satelliteResponse.data);
        
        setLoading(false);
      } catch (err) {
        setError(err.message);
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  useEffect(() => {
    const moveSatellites = () => {
      setSatellitePositions(prev => prev.map(sat => {
        const angle = sat.direction * (Math.PI / 180);
        let newX = sat.x + Math.cos(angle) * sat.speed;
        let newY = sat.y + Math.sin(angle) * sat.speed;
        
        if (newX < 0 || newX > 100 || newY < 0 || newY > 100) {
          return {
            ...sat,
            x: Math.max(0, Math.min(100, newX)),
            y: Math.max(0, Math.min(100, newY)),
            direction: (sat.direction + 180 + (Math.random() * 90 - 45)) % 360
          };
        }
        
        if (Math.random() < 0.01) {
          return {
            ...sat,
            x: newX,
            y: newY,
            direction: (sat.direction + Math.random() * 60 - 30) % 360
          };
        }
        
        return { ...sat, x: newX, y: newY };
      }));
    };

    const interval = setInterval(moveSatellites, 100);
    return () => clearInterval(interval);
  }, []);

  const getCelestialIcon = (type) => {
    return celestialIcons[type?.toLowerCase()] || celestialIcons.default;
  };

  if (loading) return (
    <div className="w-72 h-screen bg-gray-900 text-white p-4 border-r border-gray-700 flex items-center justify-center">
      <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-blue-500"></div>
    </div>
  );

  if (error) return (
    <div className="w-72 h-screen bg-gray-900 text-white p-4 border-r border-gray-700 flex items-center justify-center">
      <div className="text-red-400">{error}</div>
    </div>
  );

  return (
    <div className=" w-64 sm:w-72 h-screen bg-gray-900 text-white p-4 border-r border-gray-700 relative overflow-hidden">
      {/* Animated Space Background */}
      <div className="absolute inset-0 pointer-events-none overflow-hidden">
        {/* Stars */}
        {Array.from({ length: 50 }).map((_, i) => (
          <div 
            key={`star-${i}`}
            className="absolute bg-white rounded-full animate-pulse"
            style={{
              width: `${Math.random() * 3}px`,
              height: `${Math.random() * 3}px`,
              left: `${Math.random() * 100}%`,
              top: `${Math.random() * 100}%`,
              opacity: Math.random() * 0.7 + 0.1,
              animationDuration: `${Math.random() * 5 + 3}s`
            }}
          />
        ))}
        
        {/* Nebula Effects */}
        <div className="absolute w-64 h-64 rounded-full bg-purple-900/20 blur-3xl -left-32 -top-32" />
        <div className="absolute w-64 h-64 rounded-full bg-blue-900/20 blur-3xl right-0 bottom-0" />
        
        {/* Animated Satellites */}
        {satellitePositions.map((pos, i) => (
          <div
            key={`sat-${i}`}
            className="absolute text-blue-300 text-xs"
            style={{
              left: `${pos.x}%`,
              top: `${pos.y}%`,
              transform: `rotate(${pos.direction + 90}deg)`
            }}
          >
            <FaSatellite className="text-blue-400/80" />
          </div>
        ))}
      </div>
      
      <div className="relative z-10 h-full flex flex-col">
        {/* Logo Header */}
        <div className="flex items-center justify-center mb-8 p-4 bg-gray-800/50 rounded-xl border border-gray-700 backdrop-blur-sm">
          <AtomLogo className="h-12 w-12 hover:rotate-180 transition-transform duration-1000" />
        </div>
        
        {/* Navigation Menu */}
        <div className="flex-1 overflow-y-auto space-y-4">
          {/* Celestial Bodies Section */}
          <div>
            <div
              className="flex items-center justify-between cursor-pointer p-3 rounded-lg hover:bg-gray-800/70 transition-all duration-300 border border-gray-700 bg-gray-900/70 backdrop-blur-sm"
              onClick={() => setOpenCelestialMenu(!openCelestialMenu)}
            >
              <div className="flex items-center gap-2">
                <FaGlobe className="text-blue-400 text-lg" />
                <span className="font-medium text-gray-200">Star</span>
              </div>
              <FaChevronDown
                className={`transition-transform duration-300 text-blue-400 ${
                  openCelestialMenu ? "rotate-180" : ""
                }`}
              />
            </div>

            {openCelestialMenu && (
              <ul className="space-y-2 pl-2 pr-2 mt-2">
                {celestialBodies.map((body) => (
                  <li
                    key={body.id}
                    onClick={() => onSelect('celestial', body.id)}
                    className={`cursor-pointer p-3 rounded-lg flex items-center transition-all duration-300 ${
                      activeId === body.id
                        ? "bg-gradient-to-r from-blue-900/30 to-transparent border-l-4 border-blue-400 shadow-lg"
                        : "hover:bg-gray-800/50 hover:border-l-4 hover:border-gray-600"
                    }`}
                  >
                    {getCelestialIcon(body.type)}
                    <span className="text-gray-200">{body.name}</span>
                    {activeId === body.id && (
                      <span className="ml-auto w-2 h-2 bg-blue-400 rounded-full animate-pulse"></span>
                    )}
                  </li>
                ))}
              </ul>
            )}
          </div>

          {/* Planets Section */}
          <div>
            <div
              className="flex items-center justify-between cursor-pointer p-3 rounded-lg hover:bg-gray-800/70 transition-all duration-300 border border-gray-700 bg-gray-900/70 backdrop-blur-sm"
              onClick={() => setOpenPlanetMenu(!openPlanetMenu)}
            >
              <div className="flex items-center gap-2">
                <GiRingedPlanet className="text-red-400 text-lg" />
                <span className="font-medium text-gray-200">Planets</span>
              </div>
              <FaChevronDown
                className={`transition-transform duration-300 text-red-400 ${
                  openPlanetMenu ? "rotate-180" : ""
                }`}
              />
            </div>

            {openPlanetMenu && (
              <ul className="space-y-2 pl-2 pr-2 mt-2">
                {planets.map((planet) => (
                  <li
                    key={planet.id}
                    onClick={() => onSelect('planet', planet.id)}
                    className={`cursor-pointer p-3 rounded-lg flex items-center transition-all duration-300 ${
                      activeId === planet.id
                        ? "bg-gradient-to-r from-red-900/30 to-transparent border-l-4 border-red-400 shadow-lg"
                        : "hover:bg-gray-800/50 hover:border-l-4 hover:border-gray-600"
                    }`}
                  >
                    {planetIcon}
                    <span className="text-gray-200">{planet.name}</span>
                    {activeId === planet.id && (
                      <span className="ml-auto w-2 h-2 bg-red-400 rounded-full animate-pulse"></span>
                    )}
                  </li>
                ))}
              </ul>
            )}
          </div>

          {/* Satellites Section */}
          <div>
            <div
              className="flex items-center justify-between cursor-pointer p-3 rounded-lg hover:bg-gray-800/70 transition-all duration-300 border border-gray-700 bg-gray-900/70 backdrop-blur-sm"
              onClick={() => setOpenSatelliteMenu(!openSatelliteMenu)}
            >
              <div className="flex items-center gap-2">
                <FaSatelliteDish className="text-purple-400 text-lg" />
                <span className="font-medium text-gray-200">Satellites</span>
              </div>
              <FaChevronDown
                className={`transition-transform duration-300 text-purple-400 ${
                  openSatelliteMenu ? "rotate-180" : ""
                }`}
              />
            </div>

            {openSatelliteMenu && (
              <ul className="space-y-2 pl-2 pr-2 mt-2">
                {satellites.map((satellite) => (
                  <li
                    key={satellite.id}
                    onClick={() => onSelect('satellite', satellite.id)}
                    className={`cursor-pointer p-3 rounded-lg flex items-center transition-all duration-300 ${
                      activeId === satellite.id
                        ? "bg-gradient-to-r from-purple-900/30 to-transparent border-l-4 border-purple-400 shadow-lg"
                        : "hover:bg-gray-800/50 hover:border-l-4 hover:border-gray-600"
                    }`}
                  >
                    {satelliteIcon}
                    <span className="text-gray-200">{satellite.name}</span>
                    {activeId === satellite.id && (
                      <span className="ml-auto w-2 h-2 bg-purple-400 rounded-full animate-pulse"></span>
                    )}
                  </li>
                ))}
              </ul>
            )}
          </div>
        </div>
        
        {/* Footer */}
        <div className="mt-auto p-3 text-xs text-gray-400 border-t border-gray-800 bg-gray-900/80 backdrop-blur-sm rounded-lg">
          <div className="flex items-center justify-between">
            <span>Tracking {celestialBodies.length + planets.length + satellites.length} objects</span>
            <div className="flex space-x-1">
              {satellitePositions.map((_, i) => (
                <div 
                  key={`signal-${i}`} 
                  className="w-1 h-1 bg-blue-400 rounded-full animate-pulse"
                  style={{ animationDelay: `${i * 0.2}s` }}
                />
              ))}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Sidebar;