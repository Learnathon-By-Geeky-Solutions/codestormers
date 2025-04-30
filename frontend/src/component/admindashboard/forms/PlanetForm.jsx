import React from 'react';
import { FaSpinner, FaTimes } from 'react-icons/fa';

const PlanetForm = ({ initialData, onCancel, onSubmit, isSubmitting, celestialSystems }) => {
  const [formData, setFormData] = React.useState(initialData || {
    name: '',
    celestialSystemId: '',
    namesake: '',
    introduction: '',
    sizeAndDistance: '',
    orbitAndRotation: '',
    moons: '',
    rings: '',
    formation: '',
    structure: '',
    surface: '',
    potentialForLife: '',
    atmosphere: '',
    magnetosphere: ''
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit(formData);
  };

  const fieldGroups = [
    {
      title: "Basic Information",
      fields: [
        { name: "name", label: "Planet Name", type: "text", required: true },
        { 
          name: "celestialSystemId", 
          label: "Celestial System", 
          type: "select",
          options: celestialSystems?.map(sys => ({ value: sys.id, label: sys.name })),
          required: true 
        },
        { name: "namesake", label: "Namesake", type: "text" }
      ]
    },
    {
      title: "Physical Characteristics",
      fields: [
        { name: "introduction", label: "Introduction", type: "textarea", required: true },
        { name: "sizeAndDistance", label: "Size and Distance", type: "textarea" },
        { name: "orbitAndRotation", label: "Orbit and Rotation", type: "textarea" }
      ]
    },
    {
      title: "Satellite Features",
      fields: [
        { name: "moons", label: "Moons", type: "textarea" },
        { name: "rings", label: "Rings", type: "textarea" }
      ]
    },
    {
      title: "Composition",
      fields: [
        { name: "formation", label: "Formation", type: "textarea" },
        { name: "structure", label: "Structure", type: "textarea" },
        { name: "surface", label: "Surface", type: "textarea" }
      ]
    },
    {
      title: "Environment",
      fields: [
        { name: "potentialForLife", label: "Potential for Life", type: "textarea" },
        { name: "atmosphere", label: "Atmosphere", type: "textarea" },
        { name: "magnetosphere", label: "Magnetosphere", type: "textarea" }
      ]
    }
  ];

  return (
    <div className="bg-gray-900 rounded-xl border border-gray-700 shadow-lg overflow-hidden">
      {/* Form Header */}
      <div className="bg-gray-800 px-6 py-4 border-b border-gray-700 flex justify-between items-center">
        <h3 className="text-xl font-semibold text-gray-100">
          {initialData?.id ? `Edit ${initialData.name}` : 'Create New Planet'}
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
        {fieldGroups.map((group, groupIndex) => (
          <div key={groupIndex} className="mb-6 last:mb-0">
            <h4 className="font-medium text-gray-300 mb-4 pb-2 border-b border-gray-700">
              {group.title}
            </h4>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
              {group.fields.map((field, fieldIndex) => (
                <div 
                  key={fieldIndex} 
                  className={field.type === "textarea" ? "md:col-span-2" : ""}
                >
                  <label className="block text-sm font-medium text-gray-300 mb-2">
                    {field.label}
                    {field.required && <span className="text-red-500 ml-1">*</span>}
                  </label>
                  
                  {field.type === "select" ? (
                    <select
                      name={field.name}
                      value={formData[field.name]}
                      onChange={handleChange}
                      className="w-full px-4 py-2.5 bg-gray-800 border border-gray-700 rounded-lg 
                                focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none
                                text-gray-100 transition-all"
                      required={field.required}
                    >
                      <option value="">Select a system</option>
                      {field.options?.map(option => (
                        <option key={option.value} value={option.value}>
                          {option.label}
                        </option>
                      ))}
                    </select>
                  ) : field.type === "textarea" ? (
                    <textarea
                      name={field.name}
                      value={formData[field.name]}
                      onChange={handleChange}
                      className="w-full px-4 py-2.5 bg-gray-800 border border-gray-700 rounded-lg 
                                focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none
                                text-gray-100 placeholder-gray-500 transition-all"
                      rows="4"
                      required={field.required}
                      placeholder={`Enter ${field.label.toLowerCase()}`}
                    />
                  ) : (
                    <input
                      type={field.type}
                      name={field.name}
                      value={formData[field.name]}
                      onChange={handleChange}
                      className="w-full px-4 py-2.5 bg-gray-800 border border-gray-700 rounded-lg 
                                focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none
                                text-gray-100 placeholder-gray-500 transition-all"
                      required={field.required}
                      placeholder={`Enter ${field.label.toLowerCase()}`}
                    />
                  )}
                </div>
              ))}
            </div>
          </div>
        ))}

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
            className="px-5 py-2.5 bg-blue-600 hover:bg-blue-700 text-white rounded-lg 
                      transition-colors duration-200 font-medium flex items-center gap-2 
                      disabled:bg-blue-800 disabled:opacity-70"
            disabled={isSubmitting}
          >
            {isSubmitting ? (
              <>
                <FaSpinner className="animate-spin" />
                Processing...
              </>
            ) : (
              initialData?.id ? 'Update Planet' : 'Create Planet'
            )}
          </button>
        </div>
      </form>
    </div>
  );
};

export default PlanetForm;