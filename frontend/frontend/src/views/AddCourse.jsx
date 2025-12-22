import React, { useState, useEffect } from "react";
import { FaSearch, FaBook } from "react-icons/fa";
import { MdDashboard, MdMenuBook } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import Logo from "../components/logo-chamran.png";
import "./styles/AddCourse.css";
import axios from 'axios';
import { FiLogOut } from "react-icons/fi";

const axiosInstance = axios.create({
  baseURL: "https://localhost:7194/api",
  headers: {
    'Content-Type': 'application/json'
  }
});

axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token") || localStorage.getItem("accessToken");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);
const AddCourse = () => {
  const navigate = useNavigate();

  const [course, setCourse] = useState({
    name: "",
    code: "",
    vahed: "",
    capacity: "",
    teacher: "",
    time: "",
    place: "",
    examDate: "",
    description: "",
  });

  const [value, setValue] = useState("");
  const [dateTime, setDateTime] = useState(new Date());
  const [allCourses, setAllCourses] = useState([]);
  const [selectedPrerequisites, setSelectedPrerequisites] = useState([]);
  const [open, setOpen] = useState(false);

  const selectedTitles = allCourses
  .filter(c => selectedPrerequisites.includes(c.id))
  .map(c => c.title)
  .join("، ");
  
  // ۱. دریافت لیست دروس برای بخش پیش‌نیازها
  useEffect(() => {
    const fetchCourses = async () => {
      try {
        // استفاده از axiosInstance که پورت 7194 در آن ست شده است
        const res = await axiosInstance.get("/Course");
        setAllCourses(res.data);
      } catch (error) {
        console.error("خطا در دریافت لیست دروس:", error);
      }
    };
    fetchCourses();
  }, []);

  // بروزرسانی فرم هنگام تایپ
  const handleChange = (e) => {
    setCourse({ ...course, [e.target.name]: e.target.value });
  };

  const handlelogin = () =>{ navigate('/login')}
  const handledashboard = () => navigate("/dashboard");
  const handlemanagecourse = () => navigate("/management");
  const handleLimitUnit = () => navigate("/limit");


  const handleSubmit = async () => {
    const body = {
      title: course.name,
      code: course.code,
      units: parseInt(course.vahed),
      capacity: parseInt(course.capacity),
      teacherName: course.teacher,
      time: course.time,
      location: course.place,
      description: course.description,
      examDate: course.examDate,
      // prerequisiteIds: selectedPrerequisites 
    };

    try {
      const res = await axiosInstance.post("/Course", body);

      if (res.status === 200 || res.status === 201) {
        
        const courseId = res.data.id;   
        if (selectedPrerequisites.length > 0) {
          await axiosInstance.post(
           `/Course/${courseId}/prerequisites`,
           { prerequisiteIds: selectedPrerequisites } 
          );
        }

        alert("درس با موفقیت ثبت شد!");
        navigate("/management");
      }
    } catch (error) {
      console.error("Error details:", error.response?.data || error.message);
      alert("خطا در ثبت درس. لطفاً کنسول را چک کنید.");
    }
  };


  useEffect(() => {
    const timer = setInterval(() => setDateTime(new Date()), 1000);
    return () => clearInterval(timer);
  }, []);

  return (
    <div className="container">
      <div className="frame">
        <div className="rectangle" />

        <div className="field-name">
          <label className="label-name">نام درس <span className="assign1">*</span></label>
          <input
            type="text"
            name="name"
            className="ipt-name"
            value={course.name}
            onChange={handleChange}
          />
        </div>

        <div className="field-code">
          <label className="label-code">کد درس <span className="assign2">*</span></label>
          <input
            type="text"
            name="code"
            className="ipt-code"
            value={course.code}
            onChange={handleChange}
          />
        </div>

        <div className="field-vahed">
          <label className="label-vahed">واحد <span className="assign3">*</span></label>
          <input
            type="text"
            name="vahed"
            className="ipt-vahed"
            value={course.vahed}
            onChange={handleChange}
          />
        </div>

        <div className="field-capacity">
          <label className="label-capacity">ظرفیت <span className="assign4">*</span></label>
          <input
            type="text"
            name="capacity"
            className="ipt-capacity"
            value={course.capacity}
            onChange={handleChange}
          />
        </div>

        <div className="field-teacher">
          <label className="label-teacher">نام استاد <span className="assign5">*</span></label>
          <input
            type="text"
            name="teacher"
            className="ipt-teacher"
            value={course.teacher}
            onChange={handleChange}
          />
        </div>

        <div className="field-time">
          <label className="label-time">زمان <span className="assign6">*</span></label>
          <input
            type="text"
            name="time"
            className="ipt-time"
            value={course.time}
            onChange={handleChange}
          />
        </div>

        <div className="field-place">
          <label className="label-place">مکان <span className="assign7">*</span></label>
          <input
            type="text"
            name="place"
            className="ipt-place"
            value={course.place}
            onChange={handleChange}
          />
        </div>

        {/* <div className="field-description">
          <label className="label-description">پیش‌نیازها <span className="assign9">*</span></label>
          <input
            type="text"
            className="ipt-description"
            placeholder="جستجو در دروس..."
            onChange={(e) => setValue(e.target.value)}
          />

          <div className="prereq-list">
            {allCourses
              .filter(c => c.title && c.title.includes(value))
              .map(c => (
                <label key={c.id} className="prereq-item">
                  <input
                    type="checkbox"
                    value={c.id}
                    checked={selectedPrerequisites.includes(c.id)}
                    onChange={(e) => {
                      const id = c.id;
                      setSelectedPrerequisites(prev =>
                        prev.includes(id) ? prev.filter(x => x !== id) : [...prev, id]
                      );
                    }}
                  />
                  {c.title}
                </label>
              ))}
          </div>
        </div> */}
        <div className="field-description">
          <label className="label-description">
           پیش‌نیازها <span className="assign9">*</span>
          </label>

          <input
            type="text"
            className="ipt-description"
            placeholder="جستجو در دروس..."
            value={value}
            onFocus={() => setOpen(true)}
            onChange={(e) => {
            setValue(e.target.value);
            setOpen(true);
            }}
            onBlur={() => {
            setTimeout(() => setOpen(false), 200);
            }}
         />

          {open && (
            <div className="prereq-dropdown">
            {allCourses
            .filter(c => c.title && c.title.includes(value))
            .map(c => (
            <label key={c.id} className="prereq-item">
            <input
              type="checkbox"
              checked={selectedPrerequisites.includes(c.id)}
              onChange={() => {
                setSelectedPrerequisites(prev =>
                  prev.includes(c.id)
                    ? prev.filter(x => x !== c.id)
                    : [...prev, c.id]
                );
              }}
            />
            {c.title}
          </label>
        ))}
    </div>
          )}
</div>

        <div className="field-examtime">
          <label className="label-examtime">تاریخ امتحان <span className="assign8">*</span></label>
          <input
            type="date"
            name="examDate"
            className="ipt-examtime"
            value={course.examDate}
            onChange={handleChange}
          />
        </div>

        <div className="dashboard">
          <button className="btn_dashdoard-add" onClick={handledashboard}>داشبورد</button>
          <div className="icon_doshboard"><MdDashboard className="icon" /></div>

          <button className="btn_manage_course" onClick={handlemanagecourse}>مدیریت دروس</button>
          <div className="icon_manage_course"><MdMenuBook className="icon" /></div>

          <button className="btn_limitunit" onClick={handleLimitUnit}>تعیین حد واحد</button>
          <div className="icon_limitunit"><FaBook className="icon" /></div>
        </div>

        <button className="icon_exit_add_course" onClick={handlelogin}>
          <FiLogOut className="exit_add_course"/>     
        </button> 

        <img className="shahid-chamran" alt="Logo" src={Logo} />

        <div className="box">
          <button className="btn-addcourse-add" onClick={() => navigate("/add-new-course")}>
            افزودن درس جدید +
          </button>
        </div>

        <div className="rectangle-3" />
        <div className="date">{dateTime.toLocaleDateString("fa-IR")}</div>
        <div className="clock">{dateTime.toLocaleTimeString("fa-IR")}</div>

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

        <div className="register-course-add-new">
          <button className="btn-reg-course-add-new" onClick={handleSubmit}>
            ثبت درس
          </button>
        </div>
      </div>
    </div>
  );
};

export default AddCourse;