import React, { useState, useEffect } from 'react';
import { FaBars, FaTimes } from 'react-icons/fa';
import CelestialSystems from '../component/admindashboard/CelestialSystems';
import Planets from '../component/admindashboard/Planets';
import Users from '../component/admindashboard/Users';
import Satellites from '../component/admindashboard/Satellites';

const AdminDashboard = () => {
  const [activeTab, setActiveTab] = useState('celestial-systems');
  const [isCreating, setIsCreating] = useState(false);
  const [editingItem, setEditingItem] = useState(null);
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);
  const [isMobile, setIsMobile] = useState(false);

  // Check if mobile view
  useEffect(() => {
    const handleResize = () => {
      setIsMobile(window.innerWidth < 1024);
      if (window.innerWidth >= 1024) {
        setIsSidebarOpen(true);
      }
    };

    handleResize(); // Initial check
    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);

  const renderContent = () => {
    switch (activeTab) {
      case 'celestial-systems':
        return <CelestialSystems 
                 isCreating={isCreating}
                 setIsCreating={setIsCreating}
                 editingItem={editingItem}
                 setEditingItem={setEditingItem}
               />;
      case 'planets':
        return <Planets 
                 isCreating={isCreating}
                 setIsCreating={setIsCreating}
                 editingItem={editingItem}
                 setEditingItem={setEditingItem}
               />;
      case 'satellites':
        return <Satellites 
                 isCreating={isCreating}
                 setIsCreating={setIsCreating}
                 editingItem={editingItem}
                 setEditingItem={setEditingItem}
               />;
      case 'users':
        return <Users />;
      default:
        return <div className="p-6">Select a category from the sidebar</div>;
    }
  };

  const toggleSidebar = () => {
    setIsSidebarOpen(!isSidebarOpen);
  };

  const handleTabChange = (tab) => {
    setActiveTab(tab);
    if (isMobile) {
      setIsSidebarOpen(false);
    }
  };

  return (
    <div className="flex h-screen bg-slate-950 overflow-hidden">
      {/* Mobile Header */}
      {isMobile && (
        <div className="fixed top-0 left-0 right-0 bg-slate-900 shadow-sm z-20 lg:hidden">
          <div className="flex items-center justify-between p-4">
            <button 
              onClick={toggleSidebar}
              className="text-gray-100 focus:outline-none"
            >
              {isSidebarOpen ? (
                <FaTimes className="h-6 w-6" />
              ) : (
                <FaBars className="h-6 w-6" />
              )}
            </button>
            <h1 className="text-xl font-semibold text-slate-200">
              {activeTab.split('-').map(word => word.charAt(0).toUpperCase() + word.slice(1)).join(' ')}
            </h1>
            <div className="w-6"></div> {/* Spacer for balance */}
          </div>
        </div>
      )}

      {/* Sidebar */}
      <div 
        className={`fixed lg:static z-10 w-64 h-full bg-gray-800 text-white transition-all duration-300 ease-in-out transform 
                   ${isSidebarOpen ? 'translate-x-0' : '-translate-x-full'} lg:translate-x-0`}
      >
        <div className="p-4 h-full flex flex-col">
          <div className="mb-8 pt-4 px-2">
            <h2 className="text-2xl font-bold">Admin Dashboard</h2>
          </div>
          
          <nav className="flex-1">
            <ul className="space-y-2">
              <li>
                <button
                  onClick={() => handleTabChange('celestial-systems')}
                  className={`w-full text-left px-4 py-3 rounded-lg transition-colors ${activeTab === 'celestial-systems' ? 'bg-blue-600' : 'hover:bg-gray-700'}`}
                >
                  Celestial Systems
                </button>
              </li>
              <li>
                <button
                  onClick={() => handleTabChange('planets')}
                  className={`w-full text-left px-4 py-3 rounded-lg transition-colors ${activeTab === 'planets' ? 'bg-blue-600' : 'hover:bg-gray-700'}`}
                >
                  Planets
                </button>
              </li>
              <li>
                <button
                  onClick={() => handleTabChange('satellites')}
                  className={`w-full text-left px-4 py-3 rounded-lg transition-colors ${activeTab === 'satellites' ? 'bg-blue-600' : 'hover:bg-gray-700'}`}
                >
                  Satellites
                </button>
              </li>
              <li>
                <button
                  onClick={() => handleTabChange('users')}
                  className={`w-full text-left px-4 py-3 rounded-lg transition-colors ${activeTab === 'users' ? 'bg-blue-600' : 'hover:bg-gray-700'}`}
                >
                  Users
                </button>
              </li>
            </ul>
          </nav>

          <div className="mt-auto p-4 text-gray-400 text-sm">
            <p>© {new Date().getFullYear()} Space Admin</p>
          </div>
        </div>
      </div>

      {/* Overlay for mobile sidebar */}
      {isSidebarOpen && isMobile && (
        <div 
          className="fixed inset-0 bg-black bg-opacity-50 z-0 lg:hidden"
          onClick={toggleSidebar}
        ></div>
      )}

      {/* Main Content */}
      <div className={`flex-1 overflow-auto transition-all duration-200 ${isMobile ? 'pt-16' : ''}`}>
        {renderContent()}
      </div>
    </div>
  );
};

export default AdminDashboard;