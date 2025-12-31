import { useState } from "react";

import Courses from "./Courses";
import Students from "./Student";
import { coursesData } from "./data";
import "./styles/TeacherDashboard.css"
import { FaChalkboardTeacher } from "react-icons/fa";
import { FiLogOut } from "react-icons/fi";
import { useNavigate } from "react-router-dom";

export default function TeacherDashboard() {
  const [courses, setCourses] = useState(coursesData);
  const [selectedCourseId, setSelectedCourseId] = useState(null);
  const navigate = useNavigate();
  const selectedCourse = courses.find(c => c.id === selectedCourseId);
  const handlelogin = () =>{
    navigate('/login')
  }
  const removeStudent = (studentId) => {
    setCourses(prev =>
      prev.map(course =>
        course.id === selectedCourseId
          ? {
              ...course,
              students: course.students.filter(s => s.id !== studentId)
            }
          : course
      )
    );
  };

  return (
    <div className="app">

      <div className="menu-panel-teach">
        <div className="logo-subtitle">
          <div className="logo">پنل استاد</div>
          <span className="subtitle">مدیریت دروس و دانشجویان</span>
        </div>
        <button className="btn-icon-deader">
          <FaChalkboardTeacher size={35} className="icon-teaschdash"/>
        </button>
        <button className="icon_exit_pro_dashboard" onClick={handlelogin}>
          <FiLogOut className="exit_std"/>     
        </button>
      </div>

      {!selectedCourse && (
        <Courses courses={courses} onSelect={setSelectedCourseId} />
      )}

      {selectedCourse && (
        <Students
          course={selectedCourse}
          onBack={() => setSelectedCourseId(null)}
          onRemove={removeStudent}
        />
      )}
    </div>
  );
}
