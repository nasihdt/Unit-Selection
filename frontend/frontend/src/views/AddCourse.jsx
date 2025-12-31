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


        
        <div className="field-name-addcourse">
          <label className="label-name-addcourse">نام درس <span className="assign1">*</span></label>
          <input
            type="text"
            name="name"
            className="ipt-name"
            placeholder="مثال:سیستم عامل"
            value={course.name}
            onChange={handleChange}
          />
        </div>

        <div className="field-code-addcourse">
          <label className="label-code-addcourse">کد درس <span className="assign2">*</span></label>
          <input
            type="text"
            name="code"
            className="ipt-code-addcourse"
            placeholder="مثال: 7777145"
            value={course.code}
            onChange={handleChange}
          />
        </div>

        <div className="field-vahed-addcourse">
          <label className="label-vahed-addcourse">واحد <span className="assign3">*</span></label>
          <input
            type="text"
            name="vahed"
            className="ipt-vahed-addcourse"
            placeholder="مثال : 3"
            value={course.vahed}
            onChange={handleChange}
          />
        </div>

        <div className="field-capacity-addcourse">
          <label className="label-capacity-addcourse">ظرفیت <span className="assign4">*</span></label>
          <input
            type="text"
            name="capacity"
            className="ipt-capacity-addcourse"
            placeholder="مثال : 40"
            value={course.capacity}
            onChange={handleChange}
          />
        </div>

        <div className="field-teacher-addcourse">
          <label className="label-teacher-addcourse">نام استاد <span className="assign5">*</span></label>
          <input
            type="text"
            name="teacher"
            className="ipt-teacher-addcourse"
            placeholder="مثال : رضا بهرامی"
            value={course.teacher}
            onChange={handleChange}
          />
        </div>

        <div className="field-time-addcourse">
          <label className="label-time-addcourse">زمان <span className="assign6">*</span></label>
          <input
            type="text"
            name="time"
            className="ipt-time-addcourse"
            placeholder="مثال : شنبه و دوشنبه 10:00"
            value={course.time}
            onChange={handleChange}
          />
        </div>

        <div className="field-place-addcourse">
          <label className="label-place-addcourse">مکان <span className="assign7">*</span></label>
          <input
            type="text"
            name="place"
            className="ipt-place-addcourse"
            placeholder="مثال : مهندسی 103"
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
        <div className="field-description-addcourse">
          <label className="label-description-addcourse">
           پیش‌نیازها <span className="assign9">*</span>
          </label>

          <input
            type="text"
            className="ipt-description-addcourse"
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
            <div className="prereq-dropdown-addcourse">
            {allCourses
            .filter(c => c.title && c.title.includes(value))
            .map(c => (
            <label key={c.id} className="prereq-item-addcourse">
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

        <div className="field-examtime-addcourse">
          <label className="label-examtime-addcourse">تاریخ امتحان <span className="assign8">*</span></label>
          <input
            type="date"
            name="examDate"
            className="ipt-examtime-addcourse"
            value={course.examDate}
            onChange={handleChange}
          />
        </div>

        <div className="dashboard">
          <button className="btn_dashdoard-add" onClick={handledashboard}>داشبورد</button>
          <div className="icon_doshboard"><MdDashboard className="icon" /></div>

          <button className="btn_manage_course" onClick={handlemanagecourse}>مدیریت دروس</button>
          <div className="icon_manage_course"><MdMenuBook className="icon" /></div>

          <button className="btn_limitunit_inadd_course" onClick={handleLimitUnit}>تعیین حد واحد</button>
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