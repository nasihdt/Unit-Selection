import { FaSearch } from "react-icons/fa";
import { MdDashboard, MdMenuBook } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import Logo from "../components/logo-chamran.png";
import "./styles/CoursesOffered.css";
import { useState, useEffect } from "react";
import axiosInstance from "../services/axiosInstance";
import { FiLogOut } from "react-icons/fi";
import { FaClipboardList } from "react-icons/fa"

const CoursesOffered = () => {
  const [value, setValue] = useState("");
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [dateTime, setDateTime] = useState(new Date());

  const API_URL = "http://localhost:5127/api/course";
  
  
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
      description: c.description,
    }));

  

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


  // بروزرسانی زمان
  useEffect(() => {
    const timer = setInterval(() => setDateTime(new Date()), 1000);
    return () => clearInterval(timer);
  }, []);

  // جستجو
  const handleChange = (e) => setValue(e.target.value);

  // روتینگ
  const handlelogin = () =>{
    navigate('/login')
  }
 
  const handleDashboard = () => navigate("/dashboardstd");
  

  const handleSelect = () => navigate("/selectunit");  
 
 // فیلتر جستجو
  const displayedCourses =
    value.trim() === ""
      ? courses
      : courses.filter(
          (c) =>
            c.title.toLowerCase().includes(value.toLowerCase()) ||
            c.teacherName.toLowerCase().includes(value.toLowerCase())
        );

  return (

    <div className="container">
  <div className="frame">
    <div className="rectangle" />

    <div className="dashboard">
      <button className="btn_dashdoard_student" onClick={handleDashboard}>
        داشبورد
      </button>
      <div className="icon_doshboard">
        <MdDashboard className="icon" />
      </div>

      <div className="div" />

      <button className="btn_offer_course">لیست دروس ارائه شده</button>
      <div className="icon_offer_course">
        <MdMenuBook className="icon" />
      </div>

      <button className="btn_choose_unit" onClick={handleSelect}>انتخاب واحد</button>
      <div className="icon_selection_unit">
         <FaClipboardList className="icon"/>
      </div>
    </div>

    <button className="exit-icon-offer" onClick={handlelogin}>
      <FiLogOut className="icon-offer-exit"/>   
    </button>  
    

    <img className="shahid-chamran" alt="Shahid chamran" src={Logo} />

    <div className="rectangle-3" />

    {/* تاریخ و زمان */}
    <div className="date">{dateTime.toLocaleDateString("fa-IR")}</div>
    <div className="clock">{dateTime.toLocaleTimeString("fa-IR")}</div>

    {/* جستجو */}
    <div className="search-container-courseoffer">
      <FaSearch className="search-icon" />
      <input
        type="text"
        placeholder="جستجو کنید..."
        value={value}
        onChange={handleChange}
        className="search-input"
      />
    </div>

    {/* جدول دروس */}
    <div className="courses-table">
      <table>
        <thead>
          <tr className="column1">
            <th>نام درس</th>
            <th>کد درس</th>
            <th>واحد</th>
            <th>ظرفیت</th>
            <th>نام استاد</th>
            <th>زمان</th>
            <th>مکان</th>
            <th>تاریخ امتحان</th>
            <th>پیش نیاز</th>
          </tr>
        </thead>
        <tbody>
          {loading && (
            <tr>
              <td colSpan="11" style={{ textAlign: "center" }}>
                در حال بارگذاری دروس...
              </td>
            </tr>
          )}
          {error && (
            <tr>
              <td colSpan="11" style={{ color: "red", textAlign: "center" }}>
                {error}
              </td>
            </tr>
          )}
          {!loading &&
            displayedCourses.map((course) => (
              <tr key={course.id}>
                <td>{course.title}</td>
                <td>{course.code}</td>
                <td>{course.units}</td>
                <td>{course.capacity}</td>
                <td>{course.teacherName}</td>
                <td>{course.time}</td>
                <td>{course.location}</td>
                <td>{course.examDate}</td>
                <td>{course.description}</td>
                <tr></tr>
              </tr>
            ))}
        </tbody>
      </table>
    </div>
  </div>
</div>

  );
};

export default CoursesOffered;

