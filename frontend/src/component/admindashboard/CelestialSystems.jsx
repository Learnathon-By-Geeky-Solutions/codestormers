import React, { useState, useEffect } from 'react';
import { FaPlus, FaEdit, FaTrash, FaSpinner, FaSearch } from 'react-icons/fa';
import CelestialSystemForm from './forms/CelestialSystemForm';
import { toast } from 'react-toastify';
import axiosInstance from '../../utils/api/axiosInstance';

const CelestialSystems = ({ isCreating, setIsCreating, editingItem, setEditingItem }) => {
  const [celestialSystems, setCelestialSystems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [deleteLoading, setDeleteLoading] = useState(false);

  // Fetch all celestial systems
  useEffect(() => {
    const fetchCelestialSystems = async () => {
      try {
        const response = await axiosInstance.get('/api/Celestial/get-all-celestials'); // Assuming this endpoint exists
        console.log(response.data)
        setCelestialSystems(response.data);
      } catch (error) {
        toast.error('Failed to fetch celestial systems');
        console.error('Error fetching celestial systems:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchCelestialSystems();
  }, []);

  const handleCreate = () => {
    setIsCreating(true);
    setEditingItem(null);
  };

  const handleEdit = (system) => {
    setEditingItem(system);
    setIsCreating(false);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this celestial system?')) {
      setDeleteLoading(true);
      try {
        await axiosInstance.delete(`/api/Admin/delete-celestial-system/${id}`);
        setCelestialSystems(celestialSystems.filter(system => system.id !== id));
        toast.success('Celestial system deleted successfully');
      } catch (error) {
        toast.error('Failed to delete celestial system');
        console.error('Error deleting celestial system:', error);
      } finally {
        setDeleteLoading(false);
      }
    }
  };

  const handleSubmit = async (formData) => {
    try {
      if (editingItem) {
        // Update existing system
        const response = await axiosInstance.put('/api/Admin/update-celestial-system', formData, {
          params: { Id: editingItem.id }
        });
        
        // Fetch the updated system data
        const updatedSystemResponse = await axiosInstance.get(`/api/Celestial/${editingItem.id}`);
        console.log(updatedSystemResponse)
        
        setCelestialSystems(celestialSystems.map(sys => 
          sys.id === editingItem.id ? updatedSystemResponse.data[0] : sys
        ));
        
        toast.success('Celestial system updated successfully');
      } else {
        // Create new system
        const response = await axiosInstance.post('/api/Admin/create-celestial-system', formData);
        setCelestialSystems([...celestialSystems, response.data]);
        toast.success('Celestial system created successfully');
      }
      setIsCreating(false);
      setEditingItem(null);
    } catch (error) {
      const errorMessage = editingItem 
        ? 'Failed to update celestial system' 
        : 'Failed to create celestial system';
      toast.error(errorMessage);
      console.error('Error:', error);
    }
  };
  if (loading) {
    return (
      <div className="p-6 flex justify-center items-center h-64">
        <FaSpinner className="animate-spin text-2xl text-blue-600" />
      </div>
    );
  }

  return (
    <div className="p-6  min-h-screen">
    {/* Header Section */}
    <div className="flex justify-between items-center mb-8">
      <h2 className="text-3xl font-bold text-slate-100">Celestial Systems</h2>
      <button 
        onClick={handleCreate}
        className="bg-slate-700 hover:bg-slate-600 text-white px-5 py-3 rounded-lg flex items-center gap-2 
                  transition-all duration-200 shadow-md hover:shadow-lg"
        disabled={isCreating}
      >
        <FaPlus className="text-lg" />
        <span className="font-medium">Create System</span>
      </button>
    </div>
    
    {/* Forms */}
    {isCreating && (
      <div className="mb-8 animate-fadeIn">
        <CelestialSystemForm 
          onCancel={() => setIsCreating(false)}
          onSubmit={handleSubmit}
          isSubmitting={isCreating}
        />
      </div>
    )}
    
    {editingItem && (
      <div className="mb-8 animate-fadeIn">
        <CelestialSystemForm 
          initialData={editingItem}
          onCancel={() => setEditingItem(null)}
          onSubmit={handleSubmit}
          isSubmitting={isCreating}
        />
      </div>
    )}
  
    {/* Systems Grid */}
    {celestialSystems.length === 0 ? (
      <div className="bg-slate-800 p-8 rounded-xl border border-slate-200 shadow-sm text-center">
        <div className="text-slate-400 mb-2">
          <FaSearch className="text-4xl mx-auto" />
        </div>
        <h3 className="text-xl font-medium text-slate-700 mb-1">
          No celestial systems found
        </h3>
        <p className="text-slate-500">
          Create your first system to get started
        </p>
      </div>
    ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-5">
        {celestialSystems.map(system => (
          <div key={system.id} className="bg-slate-800 rounded-xl border border-slate-700 overflow-hidden 
                                    hover:shadow-lg hover:shadow-slate-900/30 transition-all duration-300 hover:border-indigo-400">
            <div className="p-5">
              <div className="flex justify-between items-start mb-3">
                <h3 className="text-xl font-bold text-indigo-300 truncate">{system.name}</h3>
                <span className="bg-slate-700/50 text-amber-300 text-xs px-3 py-1 rounded-full border border-slate-600">
                  {system.type}
                </span>
              </div>
              
              <p className="text-slate-300 text-sm mb-4 line-clamp-3">
                {system.description || 'No description available'}
              </p>
            </div>
            
            <div className="bg-slate-700/40 px-5 py-3 border-t border-slate-700 flex justify-end gap-2">
              <button 
                onClick={() => handleEdit(system)}
                className="text-indigo-300 hover:text-white p-2 rounded-lg hover:bg-indigo-900/50 
                          transition-colors duration-200 border border-indigo-900/20"
                title="Edit"
              >
                <FaEdit />
              </button>
              <button 
                onClick={() => handleDelete(system.id)}
                className="text-rose-400 hover:text-rose-300 p-2 rounded-lg hover:bg-rose-900/30 
                          transition-colors duration-200 border border-rose-900/20"
                title="Delete"
                disabled={deleteLoading}
              >
                {deleteLoading ? <FaSpinner className="animate-spin" /> : <FaTrash />}
              </button>
            </div>
          </div>
        ))}
      </div>
    )}
  </div>
  );
};

export default CelestialSystems;