import { useState } from "react";
import { FaSpinner, FaTimes } from 'react-icons/fa';

const SatelliteForm = ({ initialData, planets, onCancel, onSubmit }) => {
  const [formData, setFormData] = useState({
    name: '',
    planetId: '',
    description: '',
    size: 0,
    distanceFromPlanet: 0,
    orbitalPeriod: 0,
    ...initialData
  });

  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: ['size', 'distanceFromPlanet', 'orbitalPeriod'].includes(name)
        ? Number(value)
        : value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);
    try {
      await onSubmit(formData);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="bg-gray-900 rounded-xl border border-gray-700 shadow-lg overflow-hidden">
      {/* Form Header */}
      <div className="bg-gray-800 px-6 py-4 border-b border-gray-700 flex justify-between items-center">
        <h3 className="text-xl font-semibold text-indigo-300">
          {initialData?.id ? `Edit ${initialData.name}` : 'Create New Satellite'}
        </h3>
        <button 
          onClick={onCancel}
          className="text-gray-400 hover:text-gray-200 p-1 rounded-full transition-colors"
          disabled={isSubmitting}
        >
          <FaTimes className="text-lg" />
        </button>
      </div>

      {/* Form Body */}
      <form onSubmit={handleSubmit} className="p-6 space-y-6">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          {/* Left Column */}
          <div className="space-y-5">
            {/* Name Field */}
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-2">
                Satellite Name <span className="text-red-500">*</span>
              </label>
              <input
                type="text"
                name="name"
                value={formData.name}
                onChange={handleChange}
                className="w-full px-4 py-2.5 bg-gray-800 border border-gray-700 rounded-lg 
                          focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none
                          text-gray-100 placeholder-gray-500 transition-all"
                placeholder="Enter satellite name"
                required
              />
            </div>

            {/* Planet Selection */}
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-2">
                Orbiting Planet <span className="text-red-500">*</span>
              </label>
              <select
                name="planetId"
                value={formData.planetId}
                onChange={handleChange}
                className="w-full px-4 py-2.5 bg-gray-800 border border-gray-700 rounded-lg 
                          focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none
                          text-gray-100 transition-all appearance-none"
                required
              >
                <option value="">Select a planet</option>
                {planets.map(planet => (
                  <option key={planet.id} value={planet.id}>
                    {planet.name}
                  </option>
                ))}
              </select>
            </div>

            {/* Description */}
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-2">
                Description
              </label>
              <textarea
                name="description"
                value={formData.description}
                onChange={handleChange}
                rows={3}
                className="w-full px-4 py-2.5 bg-gray-800 border border-gray-700 rounded-lg 
                          focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none
                          text-gray-100 placeholder-gray-500 transition-all"
                placeholder="Describe the satellite"
              />
            </div>
          </div>

          {/* Right Column */}
          <div className="space-y-5">
            {/* Size */}
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-2">
                Diameter (km) <span className="text-red-500">*</span>
              </label>
              <input
                type="number"
                name="size"
                value={formData.size}
                onChange={handleChange}
                min="0"
                step="0.1"
                className="w-full px-4 py-2.5 bg-gray-800 border border-gray-700 rounded-lg 
                          focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none
                          text-gray-100 placeholder-gray-500 transition-all"
                required
              />
            </div>

            {/* Distance */}
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-2">
                Distance from Planet (km) <span className="text-red-500">*</span>
              </label>
              <input
                type="number"
                name="distanceFromPlanet"
                value={formData.distanceFromPlanet}
                onChange={handleChange}
                min="0"
                step="0.1"
                className="w-full px-4 py-2.5 bg-gray-800 border border-gray-700 rounded-lg 
                          focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none
                          text-gray-100 placeholder-gray-500 transition-all"
                required
              />
            </div>

            {/* Orbital Period */}
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-2">
                Orbital Period (days) <span className="text-red-500">*</span>
              </label>
              <input
                type="number"
                name="orbitalPeriod"
                value={formData.orbitalPeriod}
                onChange={handleChange}
                min="0"
                step="0.1"
                className="w-full px-4 py-2.5 bg-gray-800 border border-gray-700 rounded-lg 
                          focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none
                          text-gray-100 placeholder-gray-500 transition-all"
                required
              />
            </div>
          </div>
        </div>

        {/* Form Footer */}
        <div className="flex justify-end gap-3 pt-4 border-t border-gray-800">
          <button
            type="button"
            onClick={onCancel}
            className="px-5 py-2.5 border border-gray-600 rounded-lg text-gray-300 hover:bg-gray-800 
                      transition-colors duration-200 font-medium"
            disabled={isSubmitting}
          >
            Cancel
          </button>
          <button
            type="submit"
            className="px-5 py-2.5 bg-indigo-600 hover:bg-indigo-500 text-white rounded-lg 
                      transition-colors duration-200 font-medium flex items-center gap-2 
                      disabled:bg-indigo-800 disabled:opacity-70"
            disabled={isSubmitting}
          >
            {isSubmitting ? (
              <>
                <FaSpinner className="animate-spin" />
                Processing...
              </>
            ) : (
              initialData?.id ? 'Update Satellite' : 'Create Satellite'
            )}
          </button>
        </div>
      </form>
    </div>
  );
};

export default SatelliteForm;