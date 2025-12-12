import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import AdminstratorLogin from './views/AdminstratorLogin'
import Dashboard from './views/AdminDashboard'  
import Managementcourse from './views/ManagementCourse'
import Addcourse from './views/AddCourse'
import Editcourse from './views/EditCourse'

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<AdminstratorLogin />} />
        <Route path="/login" element={<AdminstratorLogin/>} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/management" element={<Managementcourse />} />
        <Route path="/add-new-course" element={<Addcourse />} />
        <Route path="/edit/:courseId" element={<Editcourse />} />
      </Routes>
    </Router>
  );
}

export default App;
