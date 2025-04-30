import React from 'react';
import { 
  FaGlobe, 
  
  FaSatellite, 
  FaUsers
} from 'react-icons/fa';
import { GiRingedPlanet } from 'react-icons/gi';


const Sidebar = ({ activeTab, setActiveTab }) => {
  return (
    <div className="w-64 bg-gray-800 text-white">
      <div className="p-4 border-b border-gray-700">
        <h1 className="text-xl font-bold">Space Admin</h1>
      </div>
      
      <nav className="p-4">
        <ul className="space-y-2">
          <li>
            <button
              onClick={() => setActiveTab('celestial-systems')}
              className={`w-full flex items-center p-3 rounded-lg transition-colors ${activeTab === 'celestial-systems' ? 'bg-blue-700' : 'hover:bg-gray-700'}`}
            >
              <FaGlobe className="mr-3" />
              Celestial Systems
            </button>
          </li>
          <li>
            <button
              onClick={() => setActiveTab('planets')}
              className={`w-full flex items-center p-3 rounded-lg transition-colors ${activeTab === 'planets' ? 'bg-blue-700' : 'hover:bg-gray-700'}`}
            >
              <GiRingedPlanet className="mr-3" />
              Planets
            </button>
          </li>
          <li>
            <button
              onClick={() => setActiveTab('satellites')}
              className={`w-full flex items-center p-3 rounded-lg transition-colors ${activeTab === 'satellites' ? 'bg-blue-700' : 'hover:bg-gray-700'}`}
            >
              <FaSatellite className="mr-3" />
              Satellites
            </button>
          </li>
          <li>
            <button
              onClick={() => setActiveTab('users')}
              className={`w-full flex items-center p-3 rounded-lg transition-colors ${activeTab === 'users' ? 'bg-blue-700' : 'hover:bg-gray-700'}`}
            >
              <FaUsers className="mr-3" />
              Users
            </button>
          </li>
        </ul>
      </nav>
    </div>
  );
};

export default Sidebar;