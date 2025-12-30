import { useState } from "react";

import Courses from "./Courses";
import Students from "./Student";
import { coursesData } from "./data";
import "./styles/TeacherDashboard.css"
import { FaChalkboardTeacher } from "react-icons/fa";

export default function TeacherDashboard() {
  const [courses, setCourses] = useState(coursesData);
  const [selectedCourseId, setSelectedCourseId] = useState(null);

  const selectedCourse = courses.find(c => c.id === selectedCourseId);

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
