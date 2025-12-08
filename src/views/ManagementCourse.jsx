import { FaSearch} from "react-icons/fa"
import { MdDashboard } from "react-icons/md";
import { MdMenuBook } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import delet from "../components/delete-course.png"
import edit from "../components/edit-course.png"
import Logo from "../components/logo-chamran.png"
import "./styles/ManagementCourse.css";

import { useState, useEffect } from "react";

 
const ManagementCourse = () => {

  const [value, setValue] = useState("");
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [dateTime, setDateTime] = useState(new Date());

  const navigate = useNavigate();

  // گرفتن داده‌ها از API
  useEffect(() => {
    const fetchCourses = async () => {
      try {
        const res = await fetch("http://localhost:5000/api/courses"); // مسیر API
        if (!res.ok) throw new Error("خطا در دریافت دروس");
        const data = await res.json();
        setCourses(data);
        setLoading(false);
      } catch (err) {
        setError(err.message);
        setLoading(false);
      }
    };

    fetchCourses();
  }, []);



  // const handleaddnewcourse = () =>{
  //   navigate('/add-new-course')
  // }
  
  // const handledashboard = () =>{
  //   navigate('/dashboard')
  // }

  // const handleedit = () =>{
  //   navigate('/edit')
  // }

  // برای نمایش تاریخ و زمان
  useEffect(() => {
    const timer = setInterval(() => {
      setDateTime(new Date());
    }, 1000);
    return () => clearInterval(timer);
  }, []);

  //جستجو
  const handleChange = (e) => setValue(e.target.value);

  //رفتن به صفحات
  const handleAddNewCourse = () => navigate("/add-new-course");
  const handleDashboard = () => navigate("/dashboard");
  const handleEdit = (id) => navigate(`/edit-course/${id}`);

  //حذف درس
  const handleDelete = async (id) => {
    const confirmed = window.confirm("آیا مطمئن هستید که می‌خواهید این درس را حذف کنید؟");
    if (!confirmed) return;

    try {
      const res = await fetch(`http://localhost:5000/api/courses/${id}`, {
        method: "DELETE",
      });
      if (!res.ok) throw new Error("حذف درس موفقیت آمیز نبود");

      // بروزرسانی لیست دروس پس از حذف
      setCourses(courses.filter((c) => c.id !== id));
    } catch (err) {
      alert(err.message);
    }
  };

  // فیلتر دروس بر اساس جستجو
  const filteredCourses = courses.filter(
    (c) =>
      c.name.toLowerCase().includes(value.toLowerCase()) ||
      c.code.toLowerCase().includes(value.toLowerCase())
  );

  return (
    <div className="container">
      <div className="frame">
        <div className="rectangle" />

        <div className="dashboard">
          <button className="btn_dashdoardadmin" onClick={handleDashboard}>داشبورد</button>
          <div className="icon_doshboard">
            <MdDashboard className="icon" />
          </div>

          <div className="div" />

          <button className="btn_manage_course">مدیریت دروس</button>
          <div className="icon_manage_course">
            <MdMenuBook className="icon" />
          </div>
        </div>

        <img className="shahid-chamran" alt="Shahid chamran" src={Logo}/>

        <div className="box">
        <button className="btn-addcoursemanage" alt="altbtncourse" onClick={handleAddNewCourse}> افزودن درس جدید  +</button>
        </div>

        
        <div className="rectangle-3" />

        {/* برای نمایش تاریخ و زمان */}
        <div className="date">{dateTime.toLocaleDateString('fa-IR')}</div>
        <div className="clock">{dateTime.toLocaleTimeString('fa-IR')}</div>

        {/* جستجو */}
        <div className="search-container">
        <FaSearch className="search-icon"/>
        <input type="text" placeholder="جستجو کنید..." value={value} onChange={handleChange} className="search-input"/>
        </div>
        

        {/* نمایش لیست دروس */}
        {loading && <p>در حال بارگذاری دروس...</p>}
        {error && <p style={{ color: "red" }}>{error}</p>}
        
        {/* <button className="btn-img-delete" onClick={() => handleDelete(course.id)}>
          <img src={delet} alt="delete img" className="img_delete"/>
        </button>

        <button className="btn-img-delete" onClick={() => handleEdit(course.id)}>
          <img src={edit} alt="delete img" className="img_delete"/>
        </button>

         
        <div className="item-course">
          <div className="course-name">نام درس</div>
          <div className="course-code">کد درس</div>
          <div className="course-unit">واحد</div>
          <div className="course-capacity">ظرفیت</div>
          <div className="teacher-name">نام استاد</div>
          <div className="time-name">زمان</div>
          <div className="course-palace">مکان</div>
          <div className="course-date-exam"> تاریخ امتحان</div>
          <div className="course-delete"> حذف</div>
          <div className="course-edit">ویرایش</div>
          
        </div> */}


        {!loading && !error && filteredCourses.map((course) => (
          
          <div key={course.id} className="item-course">
            <div className="course-name">{course.name}</div>
            <div className="course-code">{course.code}</div>
            <div className="course-unit">{course.unit}</div>
            <div className="course-capacity">{course.capacity}</div>
            <div className="teacher-name">{course.teacher}</div>
            <div className="time-name">{course.time}</div>
            <div className="course-palace">{course.place}</div>
            <div className="course-date-exam">{new Date(course.examDate).toLocaleDateString("fa-IR")}</div>
            <div className="course-delete">
              <button onClick={() => handleDelete(course.id)}>
                <img src={delet} alt="delete" className="img_delete" />
              </button>
            </div>
            <div className="course-edit">
              <button onClick={() => handleEdit(course.id)}>
                <img src={edit} alt="edit" className="img_delete" />
              </button>
            </div>
          </div>
        ))}

      </div>
    </div>
  );
};

export default ManagementCourse;