// this is new file

import { FaSearch, FaBook } from "react-icons/fa";
import { MdDashboard, MdMenuBook } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";

import axiosInstance from "../services/axiosInstance";

import delet from "../components/delete-course.png";
import edit from "../components/edit-course.png";
import Logo from "../components/logo-chamran.png";

import "./styles/ManagementCourse.css";

const ManagementCourse = () => {
  const navigate = useNavigate();

  const [value, setValue] = useState("");
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [dateTime, setDateTime] = useState(new Date());

  // ===============================
  // Helpers
  // ===============================
  const parseCourses = (data) =>
    data.map((c) => ({
      id: c.id ?? c.Id,
      title: c.title ?? c.Title,
      code: c.code ?? c.Code,
      units: c.units ?? c.Units,
      capacity: c.capacity ?? c.Capacity,
      teacherName: c.teacherName ?? c.TeacherName,
      time: c.time ?? c.Time,
      location: c.location ?? c.Location,
      description: c.description ?? "",
    }));

  // ===============================
  // Fetch Courses
  // ===============================
  useEffect(() => {
    const fetchCourses = async () => {
      try {
        setLoading(true);
        const res = await axiosInstance.get("/course");
        const formatted = parseCourses(res.data);
        setCourses(formatted);
      } catch (err) {
        console.error("FETCH COURSES ERROR:", err);
        setError("خطا در دریافت دروس");
      } finally {
        setLoading(false);
      }
    };

    fetchCourses();
  }, []);

  // ===============================
  // Clock
  // ===============================
  useEffect(() => {
    const timer = setInterval(() => setDateTime(new Date()), 1000);
    return () => clearInterval(timer);
  }, []);

  // ===============================
  // Actions
  // ===============================
  const handleDelete = async (id) => {
    if (!window.confirm("آیا مطمئن هستید؟")) return;

    try {
      await axiosInstance.delete(`/course/${id}`);
      setCourses((prev) => prev.filter((c) => c.id !== id));
    } catch (err) {
      console.error(err);
      alert("حذف درس انجام نشد");
    }
  };

  const handleEdit = (id) => navigate(`/edit/${id}`);
  const handleAddNewCourse = () => navigate("/add-new-course");
  const handleDashboard = () => navigate("/dashboard");
  const handleLimitUnit = () => navigate("/limit");

  // ===============================
  // Search
  // ===============================
  const displayedCourses =
    value.trim() === ""
      ? courses
      : courses.filter(
          (c) =>
            c.title.toLowerCase().includes(value.toLowerCase()) ||
            c.code.toLowerCase().includes(value.toLowerCase())
        );

  // ===============================
  // Render
  // ===============================
  return (
    <div className="container">
      <div className="frame">
        <div className="rectangle" />

        {/* Sidebar */}
        <div className="dashboard">
          <button className="btn_dashdoard_admin" onClick={handleDashboard}>
            داشبورد
          </button>
          <div className="icon_doshboard">
            <MdDashboard className="icon" />
          </div>

          <button className="btn_mng_course">مدیریت دروس</button>
          <div className="icon_manage_course">
            <MdMenuBook className="icon" />
          </div>

          <button className="botton_limitunit" onClick={handleLimitUnit}>
            تعیین حد واحد
          </button>
          <div className="icon_limitunit">
            <FaBook className="icon" />
          </div>
        </div>

        <img className="shahid-chamran" src={Logo} alt="logo" />

        <button className="btn-addcoursemanage" onClick={handleAddNewCourse}>
          افزودن درس جدید +
        </button>

        <div className="rectangle-3" />

        {/* Date & Time */}
        <div className="date">{dateTime.toLocaleDateString("fa-IR")}</div>
        <div className="clock">{dateTime.toLocaleTimeString("fa-IR")}</div>

        {/* Search */}
        <div className="search-container">
          <FaSearch className="search-icon" />
          <input
            type="text"
            placeholder="جستجو کنید..."
            value={value}
            onChange={(e) => setValue(e.target.value)}
            className="search-input"
          />
        </div>

        {/* Header */}
        <div className="list-of-course">
          <div>نام درس</div>
          <div>کد درس</div>
          <div>واحد</div>
          <div>ظرفیت</div>
          <div>نام استاد</div>
          <div>زمان</div>
          <div>مکان</div>
          <div>پیش‌نیاز</div>
          <div>حذف</div>
          <div>ویرایش</div>
        </div>

        {/* Content */}
        {loading && <p>در حال بارگذاری...</p>}
        {error && <p style={{ color: "red" }}>{error}</p>}

        {!loading &&
          displayedCourses.map((course) => (
            <div key={course.id} className="item-course">
              <div>{course.title}</div>
              <div>{course.code}</div>
              <div>{course.units}</div>
              <div>{course.capacity}</div>
              <div>{course.teacherName}</div>
              <div>{course.time}</div>
              <div>{course.location}</div>
              <div>{course.description}</div>

              <button onClick={() => handleDelete(course.id)}>
                <img src={delet} alt="delete" className="img-delete" />
              </button>

              <button onClick={() => handleEdit(course.id)}>
                <img src={edit} alt="edit" className="img_edit" />
              </button>
            </div>
          ))}
      </div>
    </div>
  );
};

export default ManagementCourse;
