import React, { useState, useEffect } from 'react';
import { FaPlus, FaEdit, FaTrash, FaSearch, FaSpinner } from 'react-icons/fa';
import { toast } from 'react-toastify';
import axiosInstance from '../../utils/api/axiosInstance';
import PlanetForm from './forms/PlanetForm';

const Planets = () => {
  const [isCreating, setIsCreating] = useState(false);
  const [editingPlanet, setEditingPlanet] = useState(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [planets, setPlanets] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isDeleting, setIsDeleting] = useState(false);
  const [isFetchingPlanet, setIsFetchingPlanet] = useState(false);

  const [celestialSystems, setCelestialSystems] = useState([]);
  
  const defaultPlanet = {
    name: '',
    introduction: '',
    namesake: '',
    potentialForLife: '',
    sizeAndDistance: '',
    orbitAndRotation: '',
    moons: '',
    rings: '',
    formation: '',
    structure: '',
    surface: '',
    atmosphere: '',
    magnetosphere: '',
    satellites: []
  };
  // Complete planet data structure with default values
  

  // Fetch planets and celestial systems from API
  const fetchData = async () => {
    setIsLoading(true);
    try {
      const [planetsResponse, celestialsResponse] = await Promise.all([
        axiosInstance.get('/api/Planet/get-all-planets'),
        axiosInstance.get('/api/Celestial/get-all-celestials')
      ]);
      setPlanets(planetsResponse.data);
      setCelestialSystems(celestialsResponse.data);
    } catch (error) {
      toast.error('Failed to fetch data');
      console.error('Error fetching data:', error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  // Fetch single planet details for editing
  const fetchPlanetDetails = async (id) => {
    setIsFetchingPlanet(true);
    try {
      const response = await axiosInstance.get(`/api/Planet/${id}`);
      console.log(response.data,"re")
     
      setEditingPlanet(response.data[0]);
    } catch (error) {
      toast.error('Failed to fetch planet details');
      console.error('Error fetching planet:', error);
    } finally {
      setIsFetchingPlanet(false);
    }
  };

  // Handle edit button click
  const handleEdit = async (planet) => {
    const res =await fetchPlanetDetails(planet.id);
    console.log(res)
    setIsCreating(false);
  };

  // Create a new planet
  const handleCreate = async (planetData) => {
    setIsSubmitting(true);
    try {
      await axiosInstance.post('/api/Admin/create-planet', planetData);
      toast.success('Planet created successfully');
      await fetchData();
      setIsCreating(false);
    } catch (error) {
      toast.error(error.response?.data?.message || 'Failed to create planet');
      console.error('Error creating planet:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  // Update an existing planet
  const handleUpdate = async (planetData) => {
    setIsSubmitting(true);
    try {
      await axiosInstance.put(
        `/api/Admin/update-planet/${editingPlanet.id}`,
        planetData
      );
      toast.success('Planet updated successfully');
      await fetchData();
      setEditingPlanet(null);
    } catch (error) {
      toast.error(error.response?.data?.message || 'Failed to update planet');
      console.error('Error updating planet:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  // Delete a planet
  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this planet?')) {
      setIsDeleting(true);
      try {
        await axiosInstance.delete(`/api/Admin/delete-planet/${id}`);
        toast.success('Planet deleted successfully');
        await fetchData();
      } catch (error) {
        toast.error(error.response?.data?.message || 'Failed to delete planet');
        console.error('Error deleting planet:', error);
      } finally {
        setIsDeleting(false);
      }
    }
  };

  const handleSubmit = (data) => {
    if (editingPlanet) {
      handleUpdate(data);
    } else {
      handleCreate(data);
    }
  };

  const filteredPlanets = planets.filter(planet =>
    planet.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    planet.introduction.toLowerCase().includes(searchTerm.toLowerCase())
  );

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <FaSpinner className="animate-spin text-4xl text-blue-500" />
      </div>
    );
  }

  console.log(filteredPlanets)

  return (
    <div className="p-6 bg-gray-900 min-h-screen">
    {/* Header Section */}
    <div className="flex justify-between items-center mb-8">
      <h2 className="text-3xl font-bold text-indigo-300">Planetary Database</h2>
      <button 
        onClick={() => {
          setIsCreating(true);
          setEditingPlanet(null);
        }}
        className="bg-indigo-600 hover:bg-indigo-500 text-white px-5 py-3 rounded-lg flex items-center gap-2 
                  transition-all duration-200 shadow-md hover:shadow-lg"
        disabled={isCreating}
      >
        <FaPlus className="text-lg" />
        <span className="font-medium">New Planet</span>
      </button>
    </div>
  
    {/* Search Bar */}
    <div className="mb-8 relative">
      <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
        <FaSearch className="text-gray-400" />
      </div>
      <input
        type="text"
        placeholder="Search planets by name or description..."
        className="w-full pl-10 pr-4 py-3 bg-gray-800 border border-gray-700 rounded-lg focus:ring-2 
                  focus:ring-indigo-500 focus:border-indigo-500 outline-none text-gray-200 placeholder-gray-500"
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
      />
    </div>
    
    {/* Forms */}
    {isCreating && (
      <div className="mb-8 animate-fadeIn">
        <PlanetForm 
          initialData={defaultPlanet}
          onCancel={() => setIsCreating(false)}
          onSubmit={handleSubmit}
          isSubmitting={isSubmitting}
          celestialSystems={celestialSystems}
        />
      </div>
    )}
    
    {editingPlanet && (
      <div className="mb-8 animate-fadeIn">
        <PlanetForm 
          initialData={editingPlanet}
          onCancel={() => setEditingPlanet(null)}
          onSubmit={handleSubmit}
          isSubmitting={isSubmitting}
          celestialSystems={celestialSystems}
        />
      </div>
    )}
  
    {/* Loading Overlay */}
    {isFetchingPlanet && (
      <div className="fixed inset-0 bg-black bg-opacity-70 flex items-center justify-center z-50 backdrop-blur-sm">
        <div className="bg-gray-800 p-8 rounded-xl shadow-2xl max-w-md w-full border border-gray-700">
          <div className="flex flex-col items-center">
            <FaSpinner className="animate-spin text-3xl text-indigo-400 mb-4" />
            <h3 className="text-xl font-semibold text-gray-200 mb-2">Loading Planet</h3>
            <p className="text-gray-400">Fetching planetary data...</p>
          </div>
        </div>
      </div>
    )}
    
    {/* Planets Grid */}
    {filteredPlanets.length === 0 ? (
      <div className="bg-gray-800 p-8 rounded-xl border border-gray-700 shadow-sm text-center">
        <div className="text-gray-500 mb-2">
          <FaSearch className="text-4xl mx-auto" />
        </div>
        <h3 className="text-xl font-medium text-gray-300 mb-1">
          {searchTerm ? 'No matching planets found' : 'No planets in database'}
        </h3>
        <p className="text-gray-400">
          {searchTerm ? 'Try a different search term' : 'Create your first planet'}
        </p>
      </div>
    ) : (
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-5">
        {filteredPlanets.map(planet => (
          <div key={planet.id} className="bg-gray-800 rounded-xl border border-gray-700 overflow-hidden 
                                    hover:shadow-lg hover:shadow-indigo-900/20 transition-all duration-300 hover:border-indigo-400">
            <div className="p-5">
              <div className="flex justify-between items-start mb-3">
                <h3 className="text-xl font-bold text-indigo-300 truncate">{planet.name}</h3>
                <span className="bg-gray-700 text-amber-300 text-xs px-2 py-1 rounded-full">
                  {celestialSystems.find(sys => sys.id === planet.celestialSystemId)?.name || 'Unknown System'}
                </span>
              </div>
              
              <p className="text-gray-300 text-sm mb-4 line-clamp-3">
                {planet.introduction || 'No introduction available'}
              </p>
              
              <div className="grid grid-cols-2 gap-2 text-xs mb-4">
                <div className="bg-gray-700/50 p-2 rounded">
                  <span className="block text-gray-400">Moons</span>
                  <span className="font-medium text-gray-200">
                    {planet.moons || 'None'}
                  </span>
                </div>
                <div className="bg-gray-700/50 p-2 rounded">
                  <span className="block text-gray-400">Rings</span>
                  <span className="font-medium text-gray-200">
                    {planet.rings || 'None'}
                  </span>
                </div>
                <div className="bg-gray-700/50 p-2 rounded">
                  <span className="block text-gray-400">Atmosphere</span>
                  <span className="font-medium text-gray-200">
                    {planet.atmosphere ? 'Present' : 'None'}
                  </span>
                </div>
              </div>
            </div>
            
            <div className="bg-gray-700/40 px-5 py-3 border-t border-gray-700 flex justify-end gap-2">
              <button 
                onClick={() => handleEdit(planet)}
                className="text-indigo-300 hover:text-white p-2 rounded-lg hover:bg-indigo-900/50 
                          transition-colors duration-200"
                title="Edit"
                disabled={isSubmitting || isDeleting}
              >
                <FaEdit />
              </button>
              <button 
                onClick={() => handleDelete(planet.id)}
                className="text-rose-400 hover:text-rose-300 p-2 rounded-lg hover:bg-rose-900/30 
                          transition-colors duration-200"
                title="Delete"
                disabled={isDeleting}
              >
                {isDeleting ? <FaSpinner className="animate-spin" /> : <FaTrash />}
              </button>
            </div>
          </div>
        ))}
      </div>
    )}
  </div>
  );
};

export default Planets;