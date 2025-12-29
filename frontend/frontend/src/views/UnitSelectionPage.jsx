import { useEffect, useState } from "react";
import jsPDF from "jspdf";
import "./styles/UnitSelectionPage.css";
import VazirTTF from "../fonts/Vazirmatn-Light.ttf?url";
import axiosInstance from "../services/axiosInstance";
import { FiLogOut } from "react-icons/fi";
import { useNavigate } from "react-router-dom";
import autoTable from "jspdf-autotable";

export default function UnitSelectionPage() {


    const parseCourses = (data) =>
    data.map((c) => ({
      id: c.courseId ?? c.Id ?? c.id,
      title: c.Title ?? c.title,
      code: c.Code ?? c.code,
      units: c.Units ?? c.units,
      capacity: c.Capacity ?? c.capacity,
      teacherName: c.TeacherName ?? c.teacherName,
      time: c.Time ?? c.time,
      location: c.Location ?? c.location,
      credits: c.Units ?? c.units,
      description: c.description,
    }));

    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [vazirBase64, setVazirBase64] = useState("");

useEffect(() => {
  fetch(VazirTTF)
    .then(res => res.arrayBuffer())
    .then(buffer => {
      let binary = '';
      const bytes = new Uint8Array(buffer);
      const len = bytes.byteLength;
      for (let i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
      }
      const base64 = window.btoa(binary);
      setVazirBase64(base64); // ذخیره base64 فونت
    });
}, []);

const [courses, setCourses] = useState([]);
useEffect(() => {
  const fetchCourses = async () => {
    try {
      setLoading(true);

      const res = await axiosInstance.get("/course");
      const formattedData = parseCourses(res.data);

      setCourses(formattedData);
      localStorage.setItem("courses", JSON.stringify(formattedData));
    } catch (err) {
      console.error(err);
      setError("خطا در دریافت دروس");
    } finally {
      setLoading(false);
    }
  };

  fetchCourses();
}, []);

    const [selectedUnits, setSelectedUnits] = useState([]);
    const [showSchedule, setShowSchedule] = useState(false);


  useEffect(() => {
    const saved = JSON.parse(localStorage.getItem("selectedUnits"));
    if (saved) setSelectedUnits(saved);
  }, []);

  useEffect(() => {
    localStorage.setItem("selectedUnits", JSON.stringify(selectedUnits));
  }, [selectedUnits]);

 
  const hasTimeConflict = (course) =>
    selectedUnits.some(
      (c) => c.day === course.day && c.time === course.time
    );

  const hasPrerequisite = (course) => {
    if (!course.prerequisite) return true;
    return selectedUnits.some((c) => c.code === course.prerequisite);
  };

 
  const removeWithDependents = (course, list) => {
    let updated = list.filter((c) => c.id !== course.id);
    const dependents = updated.filter(
      (c) => c.prerequisite === course.code
    );

    dependents.forEach((d) => {
      updated = removeWithDependents(d, updated);
    });

    return updated;
  };

  const handlelogout = () =>{
    navigate('/dashboardstd')
  } 

  const toggleUnit = (course) => {
    const exists = selectedUnits.find((c) => c.id === course.id);

    if (exists) {
      const updated = removeWithDependents(course, selectedUnits);
      if (updated.length !== selectedUnits.length) {
        alert("⚠️ دروس وابسته به این درس نیز حذف شدند");
      }
      setSelectedUnits(updated);
      return;
    }

    if (hasTimeConflict(course)) {
      alert("⛔ تداخل زمانی");
      return;
    }

    if (!hasPrerequisite(course)) {
      alert(`⛔ پیش‌نیاز ${course.prerequisite} رعایت نشده`);
      return;
    }

    setSelectedUnits([...selectedUnits, course]);
  };

  const totalCredits = selectedUnits.reduce(
  (sum, c) => sum + Number(c.units || 0), 
  0
);
 const isValid = totalCredits >= 12 && totalCredits <= 18;

  const generatePDF = () => {
  if (!vazirBase64) {
    alert("فونت هنوز بارگذاری نشده، کمی صبر کنید");
    return;
  }

  const doc = new jsPDF();

  doc.addFileToVFS("Vazir.ttf", vazirBase64);
  doc.addFont("Vazir.ttf", "Vazir", "normal");
  doc.setFont("Vazir");


   doc.text("برنامه هفتگی دانشجو", doc.internal.pageSize.getWidth() - 10, 20, { align: "right" });


  
  // جدول برنامه هفتگی
  doc.setFontSize(12);
  const startY = 40;
    let y = startY;
    
    doc.setFontSize(14);
    const tableColumn = ["Time" ,"Teacher name" ,"code", "Title"];
      const tableRows = selectedUnits.map(course => [
        course.time || "",
        course.teacherName || "",
        course.code || "",
        course.title || ""
    ]);

    autoTable(doc, { 
      head: [tableColumn],
      body: tableRows,
      startY: 40,
      styles: { font: "Vazir", halign: "right" },
      headStyles: { fillColor: [41, 128, 185], textColor: 255, fontStyle: "bold" },
    });

    doc.save("schedule.pdf");


  };

  
  return (
    <div className="select-unit-page">
      <h1 className="title">🎓 انتخاب واحد دانشجو</h1>

      <div className="layout">
        <div className="courses">
          <h2>📚 دروس ارائه‌شده</h2>


          <button className="btn_exit_Unit_selection" onClick={handlelogout}>
            <FiLogOut className="icon_exit"/>
          </button>
          {courses.map(course => {
            const selected = selectedUnits.some(c => c.id === course.id);
            const remainingCapacity = course.capacity - selectedUnits.filter(c => c.id === course.id).length;

            return (
              <div className="course-card" key={course.id}>
                <div>
                  <h3>{course.title}</h3>
                  <p>کد درس : 
                    {course.code} 
                  </p>
                  <p>واحد :
                     {course.units} 
                  </p>
                  <p className="meta">
                    {course.data}
                  </p>
                  {course.prerequisite && (
                    <p className="pre">
                      پیش‌نیاز: {course.prerequisite}
                    </p>
                  )}
                </div>

                <p className="capacity">ظرفیت: {remainingCapacity} / {course.capacity}
                   {/* ظرفیت: {course.capacity - selectedUnits.filter(c => c.id === course.id).length} / {course.capacity} */}
                </p>

                <p className="name-teach">نام استاد : {course.teacherName}</p>

                <button
                  className={selected ? "btn remove" : "btn add"}
                  onClick={() => toggleUnit(course)}
                >
                  {selected ? "حذف" : "انتخاب"}
                </button>
              </div>
            );
          })}
        </div>

        <div className="summary">
          <h2>🛒 واحدهای انتخاب‌شده</h2>

          {selectedUnits.length === 0 ? (
            <p className="empty">هیچ درسی انتخاب نشده</p>
          ) : (
            <ul>
              {selectedUnits.map((c) => (
                <li key={c.id}>
                  {c.name}
                  <span>{c.credits} واحد</span>
                </li>
              ))}
            </ul>
          )}

          <div className="credits">
            مجموع واحدها: <strong>{totalCredits}</strong>
          </div>

          <div className={`status ${isValid ? "ok" : "error"}`}>
            {isValid
              ? "انتخاب واحد معتبر است ✅"
              : "حداقل ۱۲ و حداکثر ۱۸ واحد"}
          </div>

          <button
            className="final-btn"
            disabled={!isValid}
            onClick={() => setShowSchedule(true)}
          >
            ثبت نهایی
          </button>
        </div>
      </div>

      {showSchedule && (
        <div className="schedule">
          <h2>📅 برنامه هفتگی</h2>

          <table>
            <thead>
              <tr>
                <th>نام درس</th>
                <th>کد درس</th>
                <th>ساعت</th>
                <th>نام استاد</th>
                <th>مکان</th>
              </tr>
            </thead>
            <tbody>
              {selectedUnits.map((c) => (
                <tr key={c.id}>
                  <td>{c.title}</td>
                  <td>{c.code}</td>
                  <td>{c.time}</td>
                  <td>{c.teacherName}</td>
                  <td>{c.location}</td>
                </tr>
              ))}
            </tbody>
          </table>

          <button className="pdf-btn" onClick={generatePDF}>
            📄 دانلود PDF برنامه هفتگی
          </button>
        </div>
      )}
    </div>
  );
}
