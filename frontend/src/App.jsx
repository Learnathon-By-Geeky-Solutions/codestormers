import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./component/NavBar";
import SolarSystem from "./component/SolarSystem";
import Signup from "./component/Signup";
import GalaxyLogin from "./component/GalaxyLogin";
import VerificationPage from "./pages/VerificationPage ";
import UserDashboard from "./pages/UserDashboard";
import AdminDashboard from "./pages/AdminDashboard";

function App() {
  return (
    <Router>
      <div>
        {/* Navbar (visible on all pages) */}
        <Navbar />

        {/* Routes */}
        <Routes>
          <Route path="/" element={<SolarSystem />} />
          <Route path="/login" element={<GalaxyLogin />} />
          <Route path="/signup" element={<Signup />} />
          <Route path="/verification-page" element={<VerificationPage />} />
          <Route path="/user-dashboard" element={<UserDashboard />} />
          <Route path="/admin-dashboard" element={<AdminDashboard />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;