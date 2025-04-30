import React, { useEffect, useState } from 'react';
import { FaTrash, FaUser, FaEnvelope, FaCheck, FaTimes, FaSpinner } from 'react-icons/fa';
import axiosInstance from '../../utils/api/axiosInstance';
import { toast } from 'react-toastify';

const Users = () => {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [isDeleting, setIsDeleting] = useState(null);
  const [isMobile, setIsMobile] = useState(false);

  // Check screen size
  useEffect(() => {
    const checkScreenSize = () => {
      setIsMobile(window.innerWidth < 768);
    };
    
    checkScreenSize();
    window.addEventListener('resize', checkScreenSize);
    return () => window.removeEventListener('resize', checkScreenSize);
  }, []);

  const fetchUsers = async () => {
    try {
      const response = await axiosInstance.get('/api/Admin/get-all-users');
      setUsers(response.data);
    } catch (err) {
      toast.error('Failed to fetch users');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchUsers(); }, []);

  const handleDelete = async (id) => {
    if (!window.confirm('Permanently delete this user?')) return;
    setIsDeleting(id);
    try {
      await axiosInstance.delete(`/api/Admin/delete-user/${id}`);
      setUsers(users.filter(user => user.id !== id));
      toast.success('User deleted');
    } catch (err) {
      toast.error('Deletion failed');
    } finally {
      setIsDeleting(null);
    }
  };

  if (loading) return (
    <div className="flex items-center justify-center h-64">
      <FaSpinner className="animate-spin text-2xl text-gray-400" />
    </div>
  );

  // Mobile Card View
  if (isMobile) {
    return (
      <div className="p-2 space-y-4">
        <div className="bg-gray-900 rounded-lg border border-gray-700 p-4">
          <h2 className="text-xl font-semibold text-gray-100 mb-4">User Management</h2>
        </div>

        {users.length > 0 ? (
          users.map((user) => (
            <div key={user.id} className="bg-gray-900 rounded-lg border border-gray-700 overflow-hidden shadow-md">
              <div className="p-4 flex items-start space-x-4">
                <div className="flex-shrink-0">
                  {user.profilePictureUrl ? (
                    <img 
                      src={user.profilePictureUrl} 
                      className="h-12 w-12 rounded-full object-cover border border-gray-700"
                      alt="Profile"
                    />
                  ) : (
                    <div className="h-12 w-12 rounded-full bg-gray-800 border border-gray-700 flex items-center justify-center">
                      <FaUser className="text-gray-500" />
                    </div>
                  )}
                </div>
                <div className="flex-1 min-w-0">
                  <div className=" flex flex-col sm:flex justify-between  items-start gap-y-2 sm:gap-y-0">
                    <div className=''>
                      <h3 className="text-lg font-medium text-gray-100 truncate">{user.name}</h3>
                      <p className="flex items-center text-sm text-gray-400 mt-1">
                        <FaEnvelope className="mr-2" />
                        <span className="truncate">{user.email}</span>
                      </p>
                    </div>
                    <span className={`px-2 py-1 inline-flex text-xs leading-4 font-semibold rounded-full ${
                      user.isEmailVerified 
                        ? 'bg-green-900 text-green-300' 
                        : 'bg-amber-900 text-amber-300'
                    }`}>
                      {user.isEmailVerified ? (
                        <>
                          <FaCheck className="mr-1" /> Verified
                        </>
                      ) : (
                        <>
                          <FaTimes className="mr-1" /> Unverified
                        </>
                      )}
                    </span>
                  </div>
                </div>
              </div>
              <div className="px-4 py-3 bg-gray-800 border-t border-gray-700 flex justify-end">
                <button
                  onClick={() => handleDelete(user.id)}
                  className={`text-gray-400 hover:text-red-500 p-2 rounded-full transition-colors ${
                    isDeleting === user.id ? 'opacity-50' : ''
                  }`}
                  disabled={isDeleting === user.id}
                >
                  {isDeleting === user.id ? (
                    <FaSpinner className="animate-spin" />
                  ) : (
                    <FaTrash />
                  )}
                </button>
              </div>
            </div>
          ))
        ) : (
          <div className="bg-gray-900 rounded-lg border border-gray-700 p-8 text-center">
            <p className="text-gray-400">No users found in the system</p>
          </div>
        )}
      </div>
    );
  }

  // Desktop Table View
  return (
    <div className="p-6 max-w-6xl mx-auto">
      <div className="bg-gray-900 rounded-xl shadow-sm border border-gray-700 overflow-hidden">
        <div className="px-6 py-4 border-b border-gray-700 bg-gray-800">
          <h2 className="text-xl font-semibold text-gray-100">User Management</h2>
        </div>
        
        <div className="overflow-x-auto">
          <table className="w-full divide-y divide-gray-700">
            <thead className="bg-gray-800">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-400 uppercase tracking-wider">User</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-400 uppercase tracking-wider">Email</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-400 uppercase tracking-wider">Status</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-400 uppercase tracking-wider">Profile</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-400 uppercase tracking-wider">Actions</th>
              </tr>
            </thead>
            <tbody className="bg-gray-900 divide-y divide-gray-700">
              {users.length > 0 ? (
                users.map((user) => (
                  <tr key={user.id} className="hover:bg-gray-800 transition-colors">
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="flex items-center">
                        <div className="flex-shrink-0 h-10 w-10 bg-gray-800 rounded-full flex items-center justify-center border border-gray-700">
                          <FaUser className="text-gray-400" />
                        </div>
                        <div className="ml-4">
                          <div className="text-sm font-medium text-gray-100">{user.name}</div>
                        </div>
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="flex items-center text-sm text-gray-300">
                        <FaEnvelope className="mr-2 text-gray-500" />
                        {user.email}
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
                        user.isEmailVerified 
                          ? 'bg-green-900 text-green-300' 
                          : 'bg-amber-900 text-amber-300'
                      }`}>
                        {user.isEmailVerified ? (
                          <>
                            <FaCheck className="mr-1" /> Verified
                          </>
                        ) : (
                          <>
                            <FaTimes className="mr-1" /> Unverified
                          </>
                        )}
                      </span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      {user.profilePictureUrl ? (
                        <img 
                          src={user.profilePictureUrl} 
                          className="h-10 w-10 rounded-full object-cover border border-gray-700"
                          alt="Profile"
                        />
                      ) : (
                        <div className="h-10 w-10 rounded-full bg-gray-800 border border-gray-700 flex items-center justify-center">
                          <span className="text-xs text-gray-500">No photo</span>
                        </div>
                      )}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <button
                        onClick={() => handleDelete(user.id)}
                        className={`text-gray-400 hover:text-red-500 p-2 rounded-full transition-colors ${
                          isDeleting === user.id ? 'opacity-50' : ''
                        }`}
                        disabled={isDeleting === user.id}
                      >
                        {isDeleting === user.id ? (
                          <FaSpinner className="animate-spin" />
                        ) : (
                          <FaTrash />
                        )}
                      </button>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="5" className="px-6 py-12 text-center text-gray-500">
                    No users found in the system
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default Users;