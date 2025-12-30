// components/Students.jsx
export default function Students({ course, onBack, onRemove }) {
  return (
    <div>
      <button className="back-btn" onClick={onBack}>بازگشت</button>

      <h2>{course.title}</h2>
      <p>{course.id} - {course.term}</p>

      <h4>لیست دانشجویان ({course.students.length} نفر)</h4>

      {course.students.map(student => (
        <div className="student-card" key={student.id}>
          <div>
            <strong>{student.name}</strong>
            <p>شماره دانشجویی: {student.id}</p>
          </div>

          <div className="student-actions">
            <span className="score">نمره {student.score}</span>
            <button
              className="delete-btn"
              onClick={() => onRemove(student.id)}
            >
              حذف
            </button>
          </div>
        </div>
      ))}
    </div>
  );
}
