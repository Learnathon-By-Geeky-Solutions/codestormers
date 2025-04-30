import React, { useEffect, useState } from 'react';
import { FaPlus, FaEdit, FaTrash, FaSpinner } from 'react-icons/fa';
import axiosInstance from '../../utils/api/axiosInstance';
import { toast } from 'react-toastify';
import SatelliteForm from './forms/SatelliteForm';

const Satellites = () => {
  const [satellites, setSatellites] = useState([]);
  const [planets, setPlanets] = useState([]);
  const [isCreating, setIsCreating] = useState(false);
  const [editingItem, setEditingItem] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isDeleting, setIsDeleting] = useState(false);

  const fetchData = async () => {
    try {
      setLoading(true);
      const [satellitesRes, planetsRes] = await Promise.all([
        axiosInstance.get('/api/Satellite/get-all-satellites'),
        axiosInstance.get('/api/Planet/get-all-planets')
      ]);
      setSatellites(satellitesRes.data);
      setPlanets(planetsRes.data);
    } catch (err) {
      toast.error('Failed to fetch data');
      console.error('Error fetching data:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleCreate = () => {
    setIsCreating(true);
    setEditingItem(null);
  };

  const handleEdit = async (satelliteId) => {
    try {
      const response = await axiosInstance.get(`/api/Satellite/${satelliteId}`);
      setEditingItem(response.data);
      setIsCreating(false);
    } catch (err) {
      toast.error('Failed to fetch satellite details');
      console.error('Error fetching satellite:', err);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this satellite?')) return;
    
    setIsDeleting(id);
    try {
      await axiosInstance.delete(`/api/Admin/delete-satellite/${id}`);
      setSatellites(satellites.filter(s => s.id !== id));
      toast.success('Satellite deleted successfully');
    } catch (err) {
      toast.error('Failed to delete satellite');
      console.error('Error deleting satellite:', err);
    } finally {
      setIsDeleting(false);
    }
  };

  const handleSubmit = async (data) => {
    try {
      if (editingItem) {
        await axiosInstance.put(`/api/Admin/update-satellite/${editingItem.id}`, data);
        toast.success('Satellite updated successfully');
      } else {
        await axiosInstance.post('/api/Admin/create-satellite', data);
        toast.success('Satellite created successfully');
      }
      await fetchData();
      setIsCreating(false);
      setEditingItem(null);
    } catch (err) {
      toast.error('Failed to save satellite');
      console.error('Error saving satellite:', err);
    }
  };

  const getPlanetName = (planetId) => {
    const planet = planets.find(p => p.id === planetId);
    return planet ? planet.name : 'Unknown Planet';
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <FaSpinner className="animate-spin text-2xl text-indigo-500" />
      </div>
    );
  }

  return (
    <div className="p-6 max-w-7xl mx-auto bg-gray-900 min-h-screen">
      {/* Header Section */}
      <div className="bg-gray-800 rounded-xl border border-gray-700 shadow-sm overflow-hidden mb-8">
        <div className="px-6 py-4 border-b border-gray-700 bg-gray-700">
          <div className="flex justify-between items-center">
            <h2 className="text-2xl font-bold text-indigo-300">Satellite Database</h2>
            <button
              onClick={handleCreate}
              className="bg-indigo-600 hover:bg-indigo-500 text-white px-5 py-3 rounded-lg flex items-center gap-2 
                        transition-all duration-200 shadow-md hover:shadow-lg"
              disabled={isCreating}
            >
              <FaPlus className="text-lg" />
              <span className="font-medium">New Satellite</span>
            </button>
          </div>
        </div>

        {/* Form Section */}
        {(isCreating || editingItem) && (
          <div className="p-6">
            <SatelliteForm
              initialData={editingItem}
              planets={planets}
              onCancel={() => {
                setIsCreating(false);
                setEditingItem(null);
              }}
              onSubmit={handleSubmit}
            />
          </div>
        )}
      </div>

      {/* Satellites Grid */}
      {satellites.length === 0 ? (
        <div className="bg-gray-800 p-8 rounded-xl border border-gray-700 shadow-sm text-center">
          <div className="text-gray-500 mb-2">
            <FaSpinner className="text-4xl mx-auto" />
          </div>
          <h3 className="text-xl font-medium text-gray-300 mb-1">
            {isCreating ? 'Create your first satellite' : 'No satellites found'}
          </h3>
          <p className="text-gray-400">
            {isCreating ? 'Fill out the form above' : 'Click "New Satellite" to get started'}
          </p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-5">
          {satellites.map((satellite) => (
            <div key={satellite.id} className="bg-gray-800 rounded-xl border border-gray-700 overflow-hidden 
                                          hover:shadow-lg hover:shadow-indigo-900/20 transition-all duration-300 hover:border-indigo-400">
              <div className="p-5">
                <div className="flex justify-between items-start mb-3">
                  <h3 className="text-xl font-bold text-indigo-300 truncate">{satellite.name}</h3>
                  <span className="bg-gray-700 text-amber-300 text-xs px-2 py-1 rounded-full">
                    {getPlanetName(satellite.planetId)}
                  </span>
                </div>
                
                <p className="text-gray-300 text-sm mb-4 line-clamp-3">
                  {satellite.description || 'No description available'}
                </p>
                
                <div className="grid grid-cols-3 gap-2 text-xs mb-4">
                  <div className="bg-gray-700/50 p-2 rounded">
                    <span className="block text-gray-400">Size</span>
                    <span className="font-medium text-gray-200">
                      {satellite.size} km
                    </span>
                  </div>
                  <div className="bg-gray-700/50 p-2 rounded">
                    <span className="block text-gray-400">Distance</span>
                    <span className="font-medium text-gray-200">
                      {satellite.distanceFromPlanet} km
                    </span>
                  </div>
                  <div className="bg-gray-700/50 p-2 rounded">
                    <span className="block text-gray-400">Period</span>
                    <span className="font-medium text-gray-200">
                      {satellite.orbitalPeriod} days
                    </span>
                  </div>
                </div>
              </div>
              
              <div className="bg-gray-700/40 px-5 py-3 border-t border-gray-700 flex justify-end gap-2">
                <button
                  onClick={() => handleEdit(satellite.id)}
                  className="text-indigo-300 hover:text-white p-2 rounded-lg hover:bg-indigo-900/50 
                            transition-colors duration-200"
                  title="Edit"
                  disabled={isDeleting}
                >
                  <FaEdit />
                </button>
                <button
                  onClick={() => handleDelete(satellite.id)}
                  className="text-rose-400 hover:text-rose-300 p-2 rounded-lg hover:bg-rose-900/30 
                            transition-colors duration-200"
                  title="Delete"
                  disabled={isDeleting === satellite.id}
                >
                  {isDeleting === satellite.id ? (
                    <FaSpinner className="animate-spin" />
                  ) : (
                    <FaTrash />
                  )}
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Satellites;