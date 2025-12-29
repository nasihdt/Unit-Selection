import { FaBook } from "react-icons/fa"; 
import { FaUsers } from "react-icons/fa";

export default function Courses({ courses, onSelect }) {
  return (
    <div className="courses">
      {courses.map(course => (
        <div
          key={course.id}
          className="course-card"
          onClick={() => onSelect(course.id)}
        >
          <h3 className="title-course-card">{course.title}</h3>
          <p className="code-course-card">کد درس: {course.id}</p>
          <span className="term-course-card">{course.term}</span>
          {/* <span className="badge">
            {course.students.length} دانشجو
          </span> */}
          <FaBook className="icon-book-card"/>
          {/* <FaUsers className="icon-users-card"/> */}

          <div className="students-info">
            <span className="badge">{course.students.length}دانشجو</span>
            <FaUsers className="icon-users-card"/>
          </div>


        </div>
      ))}
    </div>
  );
}
