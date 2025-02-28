import axios from "axios";

const axiosInstance = axios.create({
  baseURL: "http://localhost:5083", // Change to your API URL
  timeout: 10000, // 10 seconds timeout
  headers: {
    "Content-Type": "application/json",
  },
});

// Optional: Add an interceptor for requests
axiosInstance.interceptors.request.use(
  (config) => {
    // Add auth token if needed
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Optional: Add an interceptor for responses
axiosInstance.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error("API Error:", error);
    return Promise.reject(error);
  }
);

export default axiosInstance;
