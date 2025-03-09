import { useEffect, useState, useRef } from "react";
import { useSearchParams } from "react-router-dom";
import axiosInstance from "../utils/api/axiosInstance";

const VerificationPage = () => {
  const [searchParams] = useSearchParams();
  const email = searchParams.get("email");
  const token = searchParams.get("token");
  const [status, setStatus] = useState("Verifying...");
  const hasRequested = useRef(false); // Prevents duplicate requests

  useEffect(() => {
    const verifyEmail = async () => {
      if (!email || !token || hasRequested.current) return; // Skip if already requested

      hasRequested.current = true; // Mark as requested

      try {
        console.log("Sending request with:", { email, token });
        const response = await axiosInstance.post(
          "/api/Auth/verify-email",
          { email, token },
          {
            headers: { "Content-Type": "application/json" },
          }
        );
        console.log("Response:", response.data);
        setStatus("Verification successful! You can now log in.");
      } catch (error) {
        console.error("Verification error:", error.response ? error.response.data : error.message);
        setStatus("Verification failed. Invalid or expired token.");
      }
    };

    verifyEmail();
  }, [email, token]);

  return (
    <div className="flex items-center justify-center h-screen bg-gray-100">
      <div className="p-6 bg-white shadow-lg rounded-md text-center w-96">
        <h1 className="text-2xl font-bold">Email Verification</h1>
        <p className="mt-4 text-gray-700">{status}</p>
      </div>
    </div>
  );
};

export default VerificationPage;
