import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./component/NavBar";
import SolarSystem from "./component/SolarSystem";
import Signup from "./component/Signup";
import GalaxyLogin from "./component/GalaxyLogin";

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
        </Routes>
      </div>
    </Router>
  );
}

export default App;