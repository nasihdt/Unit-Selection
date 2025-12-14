import React, { useState, useEffect } from "react";
import { FaSearch } from "react-icons/fa";
import { MdDashboard, MdMenuBook } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import Logo from "../components/logo-chamran.png";
import "./styles/AddCourse.css";

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

  // بروزرسانی فرم
  const handleChange = (e) => {
    setCourse({ ...course, [e.target.name]: e.target.value });
  };

  const handledashboard = () => navigate("/dashboard");
  const handlemanagecourse = () => navigate("/management");

  const handleSubmit = async () => {
    const token = localStorage.getItem("token");

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
    };

    try {
      const res = await fetch("http://localhost:5127/api/Course", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(body),
      });

      if (res.ok) {
        alert("درس با موفقیت ثبت شد!");
        navigate("/management");
      } else {
        const errorText = await res.text();
        console.log("Error:", errorText);
        alert("خطا در ثبت درس");
      }
    } catch (error) {
      console.error(error);
      alert("مشکل در ارتباط با سرور");
    }
  };

  // نمایش تاریخ و زمان
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

        <div className="field-description">
          <label className="label-description">پیش نیاز <span className="assign9">*</span></label>
          <input
            type="text"
            name="description"
            className="ipt-description"
            value={course.description}
            onChange={handleChange}
          />
        </div>

        <div className="dashboard">
          <button className="btn_dashdoard-add" onClick={handledashboard}>
            داشبورد
          </button>
          <div className="icon_doshboard">
            <MdDashboard className="icon" />
          </div>

          <button className="btn_manage_course" onClick={handlemanagecourse}>
            مدیریت دروس
          </button>
          <div className="icon_manage_course">
            <MdMenuBook className="icon" />
          </div>
        </div>

        <img className="shahid-chamran" alt="Shahid chamran" src={Logo} />

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

        <div className="register-course">
          <button className="btn-reg-course" onClick={handleSubmit}>
            ثبت درس
          </button>
        </div>
      </div>
    </div>
  );
};

export default AddCourse;