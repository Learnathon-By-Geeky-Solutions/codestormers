import React, { useState, useEffect } from "react";
import { FaSpinner, FaTimes } from "react-icons/fa";

const CelestialSystemForm = ({ initialData, onCancel, onSubmit }) => {
  const [formData, setFormData] = useState({
    name: "",
    type: "",
    description: "",
    structure: "",
  });
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (initialData) {
      setFormData({
        name: initialData.name || "",
        type: initialData.type || "",
        description: initialData.description || "",
        structure: initialData.structure || "",
      });
    }
  }, [initialData]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      await onSubmit(formData);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="bg-gray-900 rounded-xl border border-gray-700 shadow-lg overflow-hidden">
      {/* Form Header */}
      <div className="bg-gray-800 px-6 py-4 border-b border-gray-700 flex justify-between items-center">
        <h3 className="text-xl font-semibold text-gray-100">
          {initialData ? "Edit Celestial System" : "Create New System"}
        </h3>
        <button
          onClick={onCancel}
          className="text-gray-400 hover:text-gray-200 p-1 rounded-full transition-colors"
          disabled={loading}
        >
          <FaTimes className="text-lg" />
        </button>
      </div>

      {/* Form Body */}
      <form onSubmit={handleSubmit} className="p-6 space-y-6">
        <div className="space-y-4">
          {/* Name Field */}
          <div>
            <label className="block text-sm font-medium text-gray-300 mb-2">
              System Name <span className="text-red-500">*</span>
            </label>
            <input
              type="text"
              name="name"
              value={formData.name}
              onChange={handleChange}
              className="w-full px-4 py-2.5 bg-gray-800 border border-gray-700 rounded-lg 
                        focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none
                        text-gray-100 placeholder-gray-500 transition-all"
              placeholder="Enter system name"
              required
            />
          </div>

          {/* Type Field */}
          <div>
            <label className="block text-sm font-medium text-gray-300 mb-2">
              Type <span className="text-red-500">*</span>
            </label>
            <input
              name="type"
              value={formData.type}
              onChange={handleChange}
              className="w-full px-4 py-2.5 bg-gray-800 border border-gray-700 rounded-lg 
                        focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none
                        text-gray-100 transition-all appearance-none"
              required
            />
          </div>

          {/* Description Field */}
          <div>
            <label className="block text-sm font-medium text-gray-300 mb-2">
              Description
            </label>
            <textarea
              name="description"
              value={formData.description}
              onChange={handleChange}
              className="w-full px-4 py-2.5 bg-gray-800 border border-gray-700 rounded-lg 
                        focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none
                        text-gray-100 placeholder-gray-500 transition-all"
              rows="3"
              placeholder="Describe the celestial system"
            />
          </div>

          {/* Structure Field */}
          <div>
            <label className="block text-sm font-medium text-gray-300 mb-2">
              Structure
            </label>
            <textarea
              name="structure"
              value={formData.structure}
              onChange={handleChange}
              className="w-full px-4 py-2.5 bg-gray-800 border border-gray-700 rounded-lg 
                        focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none
                        text-gray-100 placeholder-gray-500 transition-all"
              rows="3"
              placeholder="Describe the system structure and composition"
            />
          </div>
        </div>

        {/* Form Footer */}
        <div className="flex justify-end gap-3 pt-4 border-t border-gray-800">
          <button
            type="button"
            onClick={onCancel}
            className="px-5 py-2.5 border border-gray-600 rounded-lg text-gray-300 hover:bg-gray-800 
                      transition-colors duration-200 font-medium"
            disabled={loading}
          >
            Cancel
          </button>
          <button
            type="submit"
            className="px-5 py-2.5 bg-blue-600 hover:bg-blue-700 text-white rounded-lg 
                      transition-colors duration-200 font-medium flex items-center gap-2 
                      disabled:bg-blue-800 disabled:opacity-70"
            disabled={loading}
          >
            {loading ? (
              <>
                <FaSpinner className="animate-spin" />
                {initialData ? "Updating..." : "Creating..."}
              </>
            ) : initialData ? (
              "Update System"
            ) : (
              "Create System"
            )}
          </button>
        </div>
      </form>
    </div>
  );
};

export default CelestialSystemForm;
