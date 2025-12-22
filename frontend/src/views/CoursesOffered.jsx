import { FaSearch } from "react-icons/fa";
import { MdDashboard, MdMenuBook } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import delet from "../components/delete-course.png";
import edit from "../components/edit-course.png";
import Logo from "../components/logo-chamran.png";
import "./styles/ManagementCourse.css";

import { useState, useEffect } from "react";

const ManagementCourse = () => {
  const [value, setValue] = useState("");
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [dateTime, setDateTime] = useState(new Date());

  const API_URL = "https://localhost:5127/api/Course";
  const navigate = useNavigate();

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
    }));

  useEffect(() => {
 
    const storedCourses = localStorage.getItem("courses");
    if (storedCourses) {
      setCourses(parseCourses(JSON.parse(storedCourses)));
      setLoading(false);
    }

    //  بارگذاری از API
    const fetchCourses = async () => {
      try {
        const token = localStorage.getItem("token");
        const res = await fetch(API_URL, {
          // method: "GET",
          headers: {
            Authorization: `Bearer ${token}`,
            Accept: "application/json",
          },
        });

        if (!res.ok) throw new Error("خطا در دریافت دروس");

        const data = await res.json();
        console.log("COURSE RAW DATA:", data);
        const formattedData = parseCourses(data);

        setCourses(formattedData);
        localStorage.setItem("courses", JSON.stringify(formattedData));
        setLoading(false);
      } catch (err) {
        setError(err.message);
        setLoading(false);
      }
    };

    fetchCourses();
  }, []);

  // بروزرسانی زمان
  useEffect(() => {
    const timer = setInterval(() => setDateTime(new Date()), 1000);
    return () => clearInterval(timer);
  }, []);

  // جستجو
  const handleChange = (e) => setValue(e.target.value);

  // روتینگ
  const handleAddNewCourse = () => navigate("/add-new-course");
  const handleDashboard = () => navigate("/dashboard");
  const handleEdit = (id) => {console.log("Editing courseId:", id);
    navigate(`/edit/${id}`)};

  // حذف درس
  const handleDelete = async (id) => {
  const confirmed = window.confirm("آیا مطمئن هستید؟");

  if (!confirmed) return;

  try {
    const token = localStorage.getItem("token");

    const res = await fetch(`${API_URL}/${id}`, {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${token}`,
        Accept: "application/json",
      },
    });

    if (!res.ok) throw new Error("حذف درس موفقیت‌آمیز نبود");

    const updatedCourses = courses.filter((c) => c.id !== id);
    setCourses(updatedCourses);
    localStorage.setItem("courses", JSON.stringify(updatedCourses));

  } catch (err) {
    alert(err.message);
  }
};

  // فیلتر جستجو
  const displayedCourses =
    value.trim() === ""
      ? courses
      : courses.filter(
          (c) =>
            c.title.toLowerCase().includes(value.toLowerCase()) ||
            c.code.toLowerCase().includes(value.toLowerCase())
        );

  return (
    <div className="container">
      <div className="frame">
        <div className="rectangle" />

        <div className="dashboard">
          <button className="btn_dashdoardadmin" onClick={handleDashboard}>
            داشبورد
          </button>
          <div className="icon_doshboard">
            <MdDashboard className="icon" />
          </div>

          <div className="div" />

          <button className="btn_manage_course">لیست دروس ارائه شده</button>
          <div className="icon_manage_course">
            <MdMenuBook className="icon" />
          </div>
        </div>

        <img className="shahid-chamran" alt="Shahid chamran" src={Logo} />

       

        <div className="rectangle-3" />

        {/* تاریخ و زمان */}
        <div className="date">{dateTime.toLocaleDateString("fa-IR")}</div>
        <div className="clock">{dateTime.toLocaleTimeString("fa-IR")}</div>

        {/* جستجو */}
        <div className="search-container">
          <FaSearch className="search-icon" />
          <input
            type="text"
            placeholder="جستجو کنید..."
            value={value}
            onChange={handleChange}
            className="search-input"
          />
        </div>

        <div className="list-of-course">
          <div className="name">نام درس</div>
          <div className="code"> کد درس</div>
          <div className="space-bet3"></div>
          <div className="unit"> واحد</div>
          <div className="capacity">ظرفیت</div>
          <div className="teachname">نام استاد</div>
          <div className="space-bet"></div>
          <div className="time">زمان</div>
          <div className="space-bet"></div>
          <div className="palace">مکان</div>
          <div className="space-bet1"></div>
          <div className="exam-date">تاریخ امتحان</div>
          
          <div className="space-bet1"></div>
          <div className="delete">حذف</div>
          <div className="space-bet1"></div>
          <div className="edit">ویرایش</div>
        </div> 

        {/* نمایش دروس */}
        {loading && <p>در حال بارگذاری دروس...</p>}
        {error && <p style={{ color: "red" }}>{error}</p>}

        {!loading &&
          displayedCourses.map((course) => (
            <div key={course.id} className="item-course">
              <div className="course-name">{course.title}</div>
              <div className="course-code">{course.code}</div>
              <div className="course-vahed">{course.units}</div>
              <div className="course-capacity">{course.capacity}</div>
              <div className="teacher-name">{course.teacherName}</div>
              <div className="time-name">{course.time}</div>
              <div className="course-palace">{course.location}</div>


              <div className="course-delete">
                <button onClick={() => handleDelete(course.id)} className="btn-delete">
                <img src={delet} alt="delete" className="img-delete"/>
                </button>
              </div>

              <div className="course-edit">
                <button onClick={() => handleEdit(course.id)}>
                  <img src={edit} alt="edit" className="img_edit" />
                </button>
              </div>
            </div>
          ))}
      </div>
    </div>
  );
};

export default ManagementCourse;

