import React, { useState, useEffect } from "react";
import Sidebar from "../component/userdashboard/Sidebar";
import ContentArea from "../component/userdashboard/ContentArea";
import { FiMenu } from "react-icons/fi";

const UserDashboard = () => {
  const [activeSelection, setActiveSelection] = useState({
    type: null,
    id: null
  });
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);
  const [windowWidth, setWindowWidth] = useState(window.innerWidth);

  useEffect(() => {
    const handleResize = () => {
      setWindowWidth(window.innerWidth);
      if (window.innerWidth >= 1024) {
        setIsSidebarOpen(true);
      } else {
        setIsSidebarOpen(false);
      }
    };

    window.addEventListener("resize", handleResize);
    return () => window.removeEventListener("resize", handleResize);
  }, []);

  const handleSidebarSelect = (type, id) => {
    setActiveSelection({ type, id });
    if (windowWidth < 1024) {
      setIsSidebarOpen(false);
    }
  };

  const toggleSidebar = () => {
    setIsSidebarOpen(!isSidebarOpen);
  };

  return (
    <div className="flex h-screen bg-gray-900 relative overflow-hidden">
      {/* Mobile Menu Button */}
      <button
        onClick={toggleSidebar}
        className="lg:hidden fixed top-4 right-4 z-50 p-2 rounded-md bg-gray-800 text-gray-300 hover:bg-gray-700 transition-all duration-300"
      >
        <FiMenu className="w-6 h-6" />
      </button>

      {/* Sidebar with Transition */}
      <div
        className={`fixed lg:static z-40 h-full transform transition-all duration-500 ease-in-out ${
          isSidebarOpen ? "translate-x-0" : "-translate-x-full"
        } lg:translate-x-0`}
      >
        <Sidebar
          onSelect={handleSidebarSelect}
          activeId={activeSelection.id}
        />
      </div>

      {/* Overlay for mobile */}
      {isSidebarOpen && windowWidth < 1024 && (
        <div
          className="fixed inset-0 bg-black bg-opacity-50 z-30 lg:hidden"
          onClick={toggleSidebar}
        />
      )}

      {/* Content Area */}
      <div className="flex-1 overflow-auto transition-all duration-300">
        <ContentArea
          type={activeSelection.type}
          id={activeSelection.id}
        />
      </div>
    </div>
  );
};

export default UserDashboard;