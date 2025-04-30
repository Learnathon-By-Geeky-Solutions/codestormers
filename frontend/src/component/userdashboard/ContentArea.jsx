import React, { useState, useEffect } from "react";
import axiosInstance from "../../utils/api/axiosInstance";
import { FaGlobe, FaMoon, FaRing, FaLayerGroup, FaSatellite } from "react-icons/fa";
import { GiOrbit } from "react-icons/gi";
import { IoMdPlanet } from "react-icons/io";
import { BsStars } from "react-icons/bs";
import WelcomeView from "./WelcomeViewer";
import Sun from "../solar_Model/Sun";
import { Canvas } from "@react-three/fiber";
import SolarSystem from "./three_js_models/sun/SolarSytem";
//satelite images
import moonTexture from "../../assets/imgs/solar/moon.jpg"; 
import phobosTexture from "../../assets/imgs/solar/phobos.jpeg"; 
import deimosTexture from "../../assets/imgs/solar/deimos.jpeg"; 
// planets images
import EarthScene from "./three_js_models/sun/planets/EarthScene";
import MarsScene from "./three_js_models/sun/planets/MarsScene";
import NeptuneScene from "./three_js_models/sun/planets/NeptuneScene";
import VenusScene from "./three_js_models/sun/planets/VenusScene";
import UranusScene from "./three_js_models/sun/planets/UranusScene";
import JupiterScene from "./three_js_models/sun/planets/JupiterScene";
import SaturnScene from "./three_js_models/sun/planets/SaturnScene";
import MoonScene from "./three_js_models/sun/planets/satelite/MoonSceene";

const ContentArea = ({ type, id }) => {
  const [content, setContent] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  
  useEffect(() => {
    const fetchContent = async () => {
      if (!id) return;
      
      try {
        setLoading(true);
        setError(null);
        setContent(null);
        
        let endpoint = '';
        if (type === 'planet') {
          endpoint = `api/Planet/${id}`;
        } else if (type === 'celestial') {
          endpoint = `api/Celestial/${id}`;
        } else if (type === 'satellite') {
          endpoint = `api/Satellite/${id}`;
        }

        const response = await axiosInstance.get(endpoint);
        const responseData = Array.isArray(response.data) ? response.data[0] : response.data;
        setContent(responseData);
      } catch (err) {
        setError(err.message || 'Failed to fetch data');
      } finally {
        setLoading(false);
      }
    };

    fetchContent();
  }, [type, id]);

  if (!id) {
    return <WelcomeView />;
  }

  if (loading) {
    return (
      <div className="flex-1 flex items-center justify-center bg-gradient-to-br from-gray-900 to-gray-800">
        <div className="flex flex-col items-center">
          <div className="relative w-24 h-24 mb-4">
            <div className="absolute inset-0 rounded-full border-4 border-purple-500 border-t-transparent animate-spin"></div>
            <div className="absolute inset-2 rounded-full border-4 border-blue-500 border-b-transparent animate-spin animation-delay-200"></div>
          </div>
          <p className="text-purple-300 font-medium">Exploring the cosmos...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex-1 flex items-center justify-center bg-gradient-to-br from-gray-900 to-gray-800">
        <div className="bg-gray-800/80 p-8 rounded-xl border border-red-500/30 backdrop-blur-sm text-center max-w-md">
          <div className="text-red-400 text-5xl mb-4">⚠️</div>
          <h3 className="text-xl font-bold text-red-400 mb-2">Cosmic Interference</h3>
          <p className="text-gray-300 mb-4">{error}</p>
          <button 
            onClick={() => window.location.reload()}
            className="px-4 py-2 bg-purple-600 hover:bg-purple-700 rounded-lg transition-colors"
          >
            Retry Transmission
          </button>
        </div>
      </div>
    );
  }

  if (!content) {
    return (
      <div className="flex-1 flex items-center justify-center bg-gradient-to-br from-gray-900 to-gray-800">
        <div className="text-center">
          <div className="text-6xl mb-4">🌌</div>
          <h3 className="text-xl font-bold text-purple-400">No Celestial Data Found</h3>
          <p className="text-gray-400">This cosmic object remains a mystery</p>
        </div>
      </div>
    );
  }

  const renderPlanetContent = () => {
    const planetName = content.name.toLowerCase();
    
    return (
      <>
        {planetName === 'earth' && <EarthScene />}
        {planetName === 'mars' && <MarsScene />}
        {planetName === 'venus' && <VenusScene />}
        {planetName === 'neptune' && <NeptuneScene />}
        {planetName === 'uranus' && <UranusScene />}
        {planetName === 'jupiter' && <JupiterScene />}
        {planetName === 'saturn' && <SaturnScene />}
        
        <div className="bg-gradient-to-br from-gray-900 to-gray-800 p-4 sm:p-8 rounded-xl">
          <div className="flex items-center mb-8 animate-fade-in">
            <IoMdPlanet className="text-5xl sm:text-6xl text-blue-400 mr-4" />
            <div>
              <h1 className="text-3xl sm:text-4xl font-bold bg-gradient-to-r from-blue-400 to-purple-400 bg-clip-text text-transparent">
                {content.name}
              </h1>
              <div className="flex space-x-2 mt-2">
                <span className="px-2 py-1 bg-blue-900/50 text-blue-300 rounded-full text-xs">Planet</span>
                {content.potentialForLife && (
                  <span className="px-2 py-1 bg-green-900/50 text-green-300 rounded-full text-xs">Potential Life</span>
                )}
              </div>
            </div>
          </div>
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-8">
            <div className="space-y-6">
              <div className="bg-gray-800/50 backdrop-blur-sm p-6 rounded-xl border border-gray-700 hover:border-purple-500 transition-all duration-300">
                <h2 className="text-xl font-semibold mb-3 flex items-center text-purple-400">
                  <FaGlobe className="mr-2" /> Overview
                </h2>
                {content.introduction && (
                  <p className="text-gray-300 mb-4">{content.introduction}</p>
                )}
                {content.namesake && (
                  <div className="mt-4">
                    <h3 className="text-sm font-medium text-gray-400 mb-1">Namesake</h3>
                    <p className="text-gray-200">{content.namesake}</p>
                  </div>
                )}
              </div>
  
              <div className="bg-gray-800/50 backdrop-blur-sm p-6 rounded-xl border border-gray-700 hover:border-blue-500 transition-all duration-300">
                <h2 className="text-xl font-semibold mb-3 flex items-center text-blue-400">
                  <GiOrbit className="mr-2" /> Orbital Dynamics
                </h2>
                {content.orbitAndRotation && (
                  <p className="text-gray-300 mb-4">{content.orbitAndRotation}</p>
                )}
                {content.sizeAndDistance && (
                  <div className="mt-4">
                    <h3 className="text-sm font-medium text-gray-400 mb-1">Size & Distance</h3>
                    <p className="text-gray-200">{content.sizeAndDistance}</p>
                  </div>
                )}
              </div>
            </div>
  
            <div className="space-y-6">
              <div className="bg-gray-800/50 backdrop-blur-sm p-6 rounded-xl border border-gray-700 hover:border-pink-500 transition-all duration-300">
                <h2 className="text-xl font-semibold mb-3 flex items-center text-pink-400">
                  <FaMoon className="mr-2" /> Satellite System
                </h2>
                {content.moons && (
                  <p className="text-gray-300 mb-4">{content.moons}</p>
                )}
                {content.rings && (
                  <div className="mt-4">
                    <h3 className="text-sm font-medium text-gray-400 mb-1">Ring System</h3>
                    <p className="text-gray-200">{content.rings}</p>
                  </div>
                )}
              </div>
  
              <div className="bg-gray-800/50 backdrop-blur-sm p-6 rounded-xl border border-gray-700 hover:border-teal-500 transition-all duration-300">
                <h2 className="text-xl font-semibold mb-3 flex items-center text-teal-400">
                  <FaLayerGroup className="mr-2" /> Formation
                </h2>
                {content.formation && (
                  <p className="text-gray-300">{content.formation}</p>
                )}
              </div>
            </div>
          </div>
  
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            {content.structure && (
              <div className="bg-gray-800/50 backdrop-blur-sm p-6 rounded-xl border border-gray-700 hover:border-yellow-500 transition-all duration-300">
                <h2 className="text-xl font-semibold mb-3 flex items-center text-yellow-400">
                  <FaLayerGroup className="mr-2" /> Structure
                </h2>
                <p className="text-gray-300">{content.structure}</p>
              </div>
            )}
  
            {content.surface && (
              <div className="bg-gray-800/50 backdrop-blur-sm p-6 rounded-xl border border-gray-700 hover:border-orange-500 transition-all duration-300">
                <h2 className="text-xl font-semibold mb-3 flex items-center text-orange-400">
                  <FaGlobe className="mr-2" /> Surface
                </h2>
                <p className="text-gray-300">{content.surface}</p>
              </div>
            )}
  
            {content.atmosphere && (
              <div className="bg-gray-800/50 backdrop-blur-sm p-6 rounded-xl border border-gray-700 hover:border-red-500 transition-all duration-300">
                <h2 className="text-xl font-semibold mb-3 flex items-center text-red-400">
                  <FaGlobe className="mr-2" /> Atmosphere
                </h2>
                <p className="text-gray-300">{content.atmosphere}</p>
              </div>
            )}
          </div>
        </div>
      </>
    );
  };

  const renderCelestialContent = () => (
    <>
 <div className="relative h-96 w-full mb-8 rounded-xl overflow-hidden">
          
          <SolarSystem />
        <div className="absolute inset-0 pointer-events-none bg-gradient-to-t from-gray-900/80 to-transparent" />
      </div>

    <div className="bg-gradient-to-br from-gray-900 to-gray-800 p-4 sm:p-8 rounded-xl">
      <div className="flex items-center mb-8 animate-fade-in">
        <BsStars className="text-5xl sm:text-6xl text-yellow-400 mr-4" />
        <div>
          <h1 className="text-3xl sm:text-4xl font-bold bg-gradient-to-r from-yellow-400 to-orange-400 bg-clip-text text-transparent">
            {content.name}
          </h1>
          <div className="flex space-x-2 mt-2">
            <span className="px-2 py-1 bg-yellow-900/50 text-yellow-300 rounded-full text-xs">Star</span>
          </div>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-8">
        <div className="bg-gray-800/50 backdrop-blur-sm p-6 rounded-xl border border-gray-700 hover:border-yellow-500 transition-all duration-300">
          <h2 className="text-xl font-semibold mb-3 flex items-center text-yellow-400">
            <BsStars className="mr-2" /> Stellar Profile
          </h2>
          {content.description && (
            <p className="text-gray-300 whitespace-pre-line">{content.description}</p>
          )}
        </div>

        <div className="bg-gray-800/50 backdrop-blur-sm p-6 rounded-xl border border-gray-700 hover:border-orange-500 transition-all duration-300">
          <h2 className="text-xl font-semibold mb-3 flex items-center text-orange-400">
            <FaLayerGroup className="mr-2" /> Internal Structure
          </h2>
          {content.structure && (
            <p className="text-gray-300 whitespace-pre-line">{content.structure}</p>
          )}
        </div>
      </div>

      {content.planets?.length > 0 && (
        <div className="bg-gray-800/50 backdrop-blur-sm p-6 rounded-xl border border-gray-700">
          <h2 className="text-xl font-semibold mb-4 text-purple-400">Orbiting Planets</h2>
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4">
            {content.planets.map(planet => (
              <div 
                key={planet.id} 
                className="bg-gray-700/50 hover:bg-gray-700 p-4 rounded-lg border border-gray-600 hover:border-purple-500 transition-all duration-300 flex items-center"
              >
                <IoMdPlanet className="text-blue-400 mr-3 flex-shrink-0" />
                <span className="truncate">{planet.name}</span>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
    </>
  );

  const renderSatelliteContent = () => {
    const satelliteName = content.name.toLowerCase();
    let sceneComponent = null;
    let texture = null;
    let title = '';
  
    // Determine which scene and texture to use based on the satellite name
    switch(satelliteName) {
      case 'moon':
        texture = moonTexture;
        title = "Earth's Moon";
        sceneComponent = <MoonScene name={content.name} title={title} texture={texture} />;
        break;
      case 'phobos':
        texture = phobosTexture;
        title = "Mars' Moon Phobos";
        sceneComponent = <MoonScene name={content.name} title={title} texture={texture} />;
        break;
      case 'deimos':
        texture = deimosTexture;
        title = "Mars' Moon Deimos";
        sceneComponent = <MoonScene name={content.name} title={title} texture={texture} />;
        break;
      default:
        // Default case for unknown satellites
        texture = moonTexture; // fallback texture
        title = `${content.name} Satellite`;
        sceneComponent = <MoonScene name={content.name} title={title} texture={texture} />;
    }
  
    return (
      <>
        {sceneComponent}
        <div className="bg-gradient-to-br from-gray-900 to-gray-800 p-4 sm:p-8 rounded-xl">
          <div className="flex items-center mb-8 animate-fade-in">
            <FaSatellite className="text-5xl sm:text-6xl text-purple-400 mr-4" />
            <div>
              <h1 className="text-3xl sm:text-4xl font-bold bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text text-transparent">
                {content.name}
              </h1>
              <div className="flex space-x-2 mt-2">
                <span className="px-2 py-1 bg-purple-900/50 text-purple-300 rounded-full text-xs">Satellite</span>
                {satelliteName === 'moon' && (
                  <span className="px-2 py-1 bg-blue-900/50 text-blue-300 rounded-full text-xs">Earth's Moon</span>
                )}
              </div>
            </div>
          </div>
  
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-8">
            <div className="bg-gray-800/50 backdrop-blur-sm p-6 rounded-xl border border-gray-700 hover:border-purple-500 transition-all duration-300">
              <h2 className="text-xl font-semibold mb-3 flex items-center text-purple-400">
                <FaSatellite className="mr-2" /> Satellite Profile
              </h2>
              {content.description && (
                <p className="text-gray-300">{content.description}</p>
              )}
            </div>
  
            <div className="bg-gray-800/50 backdrop-blur-sm p-6 rounded-xl border border-gray-700 hover:border-blue-500 transition-all duration-300">
              <h2 className="text-xl font-semibold mb-3 flex items-center text-blue-400">
                <GiOrbit className="mr-2" /> Orbital Characteristics
              </h2>
              <div className="space-y-4">
                <div>
                  <h3 className="text-sm font-medium text-gray-400 mb-1">Distance from Planet</h3>
                  <p className="text-gray-200">{content.distanceFromPlanet} km</p>
                </div>
                <div>
                  <h3 className="text-sm font-medium text-gray-400 mb-1">Orbital Period</h3>
                  <p className="text-gray-200">{content.orbitalPeriod} days</p>
                </div>
                {satelliteName === 'moon' && (
                  <div>
                    <h3 className="text-sm font-medium text-gray-400 mb-1">Gravity</h3>
                    <p className="text-gray-200">1.62 m/s² (16.6% of Earth's)</p>
                  </div>
                )}
              </div>
            </div>
          </div>
  
          <div className="bg-gray-800/50 backdrop-blur-sm p-6 rounded-xl border border-gray-700 hover:border-pink-500 transition-all duration-300">
            <h2 className="text-xl font-semibold mb-3 flex items-center text-pink-400">
              <FaLayerGroup className="mr-2" /> Physical Characteristics
            </h2>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div>
                <h3 className="text-sm font-medium text-gray-400 mb-1">Size</h3>
                <p className="text-gray-200">{content.size} km diameter</p>
              </div>
              <div>
                <h3 className="text-sm font-medium text-gray-400 mb-1">Surface Temperature</h3>
                <p className="text-gray-200">{content.temperature || 'N/A'}</p>
              </div>
              {satelliteName === 'moon' && (
                <div>
                  <h3 className="text-sm font-medium text-gray-400 mb-1">Surface Composition</h3>
                  <p className="text-gray-200">Regolith (rock and dust)</p>
                </div>
              )}
            </div>
          </div>
        </div>
      </>
    );
  };

  return (
    <div className="flex-1 p-4 sm:p-6 overflow-auto bg-gradient-to-br from-gray-900 to-gray-800">
      {type === 'planet' && renderPlanetContent()}
      {type === 'celestial' && renderCelestialContent()}
      {type === 'satellite' && renderSatelliteContent()}
    </div>
  );
};

export default ContentArea;