import React, { useState, useEffect } from "react";
import { FaSearch } from "react-icons/fa";
import { MdDashboard, MdMenuBook } from "react-icons/md";
import { useNavigate, useParams } from "react-router-dom";
import Logo from "../components/logo-chamran.png";
import "./styles/EditCourse.css";

const EditCourse = () => {
  const { courseId } = useParams(); // گرفتن courseId از URL
  const navigate = useNavigate();

  const [course, setCourse] = useState({
    name: "",
    code: "",
    vahed: "",
    capacity: "",
    teacher: "",
    time: "",
    place: "",
  });

  const [dateTime, setDateTime] = useState(new Date());
  const [searchValue, setSearchValue] = useState("");
  const token = localStorage.getItem("token");
  // دریافت داده درس از سرور
  useEffect(() => {
    const fetchCourse = async () => {
      try {
        
        const res = await fetch(`http://localhost:5127/api/Course/${courseId}`, {
            headers: {
             "Authorization": `Bearer ${token}`
            }
        });
        if (res.ok) {
          const data = await res.json();
          setCourse({
            name: String(data.title || ""),
            code: String(data.code || ""),
            vahed: String(data.units || ""),
            capacity: String(data.capacity || ""),
            teacher: String(data.teacherName || ""),
            time: String(data.time || ""),
            place: String(data.location || ""),
          });
        } else {
          console.error("خطا در دریافت داده درس");
        }
      } catch (error) {
        console.error(error);
      }
    };
    fetchCourse();
  }, [courseId]);

  // تایمر تاریخ و ساعت
  useEffect(() => {
    const timer = setInterval(() => setDateTime(new Date()), 1000);
    return () => clearInterval(timer);
  }, []);

  // بروزرسانی فرم
  const handleChange = (e) => {
    setCourse({ ...course, [e.target.name]: e.target.value });
  };

  // ثبت تغییرات
  const handleUpdate = async () => {
    // اعتبارسنجی
    for (let key in course) {
      if (course[key].trim() === "") {
        alert(`لطفاً فیلد ${key} را پر کنید!`);
        return;
      }
    }

    // map کردن داده‌ها مطابق API
    const payload = {
      title: course.name,
      code: course.code,
      units: parseInt(course.vahed),
      capacity: parseInt(course.capacity),
      teacherName: course.teacher,
      time: course.time,
      location: course.place
    };

    try {
      const res = await fetch(`http://localhost:5127/api/Course/${courseId}`, {
        method: "PUT",
        headers: {  "Content-Type": "application/json","Authorization": `Bearer ${token}` },
        body: JSON.stringify(payload),
      });

      if (res.ok) {
        alert("ویرایش درس با موفقیت انجام شد!");
        navigate("/management");
      } else {
        const errorText = await res.text();
        alert("خطا در ویرایش درس: " + errorText);
      }
    } catch (error) {
      console.error(error);
      alert("مشکل در ارتباط با سرور");
    }
  };

  // ناوبری صفحات
  const handledashboard = () => navigate("/dashboard");
  const handlemanagecourse = () => navigate("/management");
  const handleaddnewcourse = () => navigate("/add-new-course");

  return (
    <div className="container">
      <div className="frame">
        <div className="rectangle" />

        <div className="field-name">
          <label className="label-name">نام درس</label>
          <input
            type="text"
            name="name"
            className="ipt-name"
            value={course.name}
            onChange={handleChange}
          />
        </div>

        <div className="field-code">
          <label className="label-code">کد درس</label>
          <input
            type="text"
            name="code"
            className="ipt-code"
            value={course.code}
            onChange={handleChange}
          />
        </div>

        <div className="field-vahed">
          <label className="label-vahed">واحد</label>
          <input
            type="text"
            name="vahed"
            className="ipt-vahed"
            value={course.vahed}
            onChange={handleChange}
          />
        </div>

        <div className="field-capacity">
          <label className="label-capacity">ظرفیت</label>
          <input
            type="text"
            name="capacity"
            className="ipt-capacity"
            value={course.capacity}
            onChange={handleChange}
          />
        </div>

        <div className="field-teacher">
          <label className="label-teacher">نام استاد</label>
          <input
            type="text"
            name="teacher"
            className="ipt-teacher"
            value={course.teacher}
            onChange={handleChange}
          />
        </div>

        <div className="field-time">
          <label className="label-time">زمان</label>
          <input
            type="text"
            name="time"
            className="ipt-time"
            value={course.time}
            onChange={handleChange}
          />
        </div>

        <div className="field-place">
          <label className="label-place">مکان</label>
          <input
            type="text"
            name="place"
            className="ipt-place"
            value={course.place}
            onChange={handleChange}
          />
        </div>

        <div className="dashboard">
          <button className="btn_dashdoard-edit" onClick={handledashboard}>
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
          <button
            className="btn-addcourseedit"
            alt="altbtncourse"
            onClick={handleaddnewcourse}
          >
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
            value={searchValue}
            onChange={(e) => setSearchValue(e.target.value)}
            className="search-input"
          />
        </div>

        <div className="register-course">
          <button className="btn-reg-course" onClick={handleUpdate}>
            ثبت ویرایش
          </button>
        </div>
      </div>
    </div>
  );
};

export default EditCourse;
