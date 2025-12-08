import React from "react";
import { FaSearch} from "react-icons/fa"
import { MdDashboard } from "react-icons/md";
import { MdMenuBook } from "react-icons/md";
import { useNavigate } from "react-router-dom";

import Logo from "../components/logo-chamran.png"
import "./styles/EditCourse.css";
import { useState, useEffect } from "react";

 
const EditCourse = ({ courseId }) => {

  const [course, setCourse] = useState({
    name: "",
    code: "",
    vahed: "",
    capacity: "",
    teacher: "",
    time: "",
    place: "",
    examDate: ""
  });

  // دریافت اطلاعات درس از سرور هنگام باز شدن صفحه
  useEffect(() => {
    const fetchCourse = async () => {
      try {
        const res = await fetch(`http://localhost:5000/api/courses/${courseId}`);
        if (res.ok) {
          const data = await res.json();
          setCourse(data);
        }
      } catch (error) {
        console.error(error);
      }
    };
    fetchCourse();
  }, [courseId]);

  // بروزرسانی فرم
  const handleChange = (e) => {
    setCourse({
      ...course,
      [e.target.name]: e.target.value
    });
  };

  // ثبت تغییرات
  const handleUpdate = async () => {
    // اعتبارسنجی فیلدها
    for (let key in course) {
      if (course[key].trim() === "") {
        alert(`لطفاً فیلد ${key} را پر کنید!`);
        return;
      }
    }

    try {
      const res = await fetch(`http://localhost:5000/api/courses/${courseId}`, {
        method: "PUT", // یا PATCH بسته به سرور
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(course)
      });

      if (res.ok) {
        alert("ویرایش درس با موفقیت انجام شد!");
        navigate("/management");
      } else {
        alert("خطا در ویرایش درس");
      }
    } catch (error) {
      console.error(error);
      alert("مشکل در ارتباط با سرور");
    }
  };



  const [value, setValue] = useState("");
  
  // const handleChange = (e) => {
  //   setValue(e.target.value);
  // };

  const navigate = useNavigate();
  
  const handledashboard = () =>{
    navigate('/dashboard')
  }
  const handlemanagecourse = () =>{
    navigate('/management')
  } 

  const handleaddnewcourse = () =>{
    navigate('/add-new-course')
  }
  //پارامتر های تایم
  const [dateTime, setDateTime] = useState(new Date());

  // برای نمایش تاریخ و زمان
  useEffect(() => {
    const timer = setInterval(() => {
      setDateTime(new Date());
    }, 1000);

    return () => clearInterval(timer); 
  }, []);

  return (
    <div className="container">
      <div className="frame">
        <div className="rectangle" />

       <div class="field-name">
          <label class="label-name">نام درس </label>
          <input type="text" class="ipt-name" value={course.name} onChange={handleChange}/>
       </div>
            
        <div class="field-code">
          <label class="label-code">کد درس</label>
          <input type="text" class="ipt-code" value={course.code} onChange={handleChange}/>
       </div>

        <div class="field-vahed">
          <label class="label-vahed">واحد</label>
          <input type="text" class="ipt-vahed" value={course.vahed} onChange={handleChange}/>
       </div>

        <div class="field-capacity">
          <label class="label-capacity">ظرفیت</label>
          <input type="text" class="ipt-capacity" value={course.capacity} onChange={handleChange}/>
       </div>

        <div class="field-teacher">
          <label class="label-teacher">نام استاد</label>
          <input type="text" class="ipt-teacher" value={course.teacher} onChange={handleChange}/>
       </div>

        <div class="field-time">
          <label class="label-time">زمان</label>
          <input type="date" class="ipt-time" value={course.time} onChange={handleChange}/>
       </div>

        <div class="field-place">
          <label class="label-place">مکان</label>
          <input type="text" class="ipt-place" value={course.place} onChange={handleChange}/>
       </div>

        <div class="field-examtime">
          <label class="label-examtime">تاریخ امتحان</label>
          <input type="date" class="ipt-examtime" value={course.examDate} onChange={handleChange}/>
       </div>   
         
        <div className="dashboard">
          <button className="btn_dashdoard-edit" onClick={handledashboard}>داشبورد</button>
          <div className="icon_doshboard">
            <MdDashboard className="icon" />
          </div>

          <div className="div" />

          <button className="btn_manage_course" onClick={handlemanagecourse}>مدیریت دروس</button>
          <div className="icon_manage_course">
            <MdMenuBook className="icon" />
          </div>
        </div>

        <img className="shahid-chamran" alt="Shahid chamran" src={Logo}/>

        {/* برای نمایش دکمه افزودن درس*/}
        <div className="box">
        <button className="btn-addcourseedit" alt="altbtncourse" onClick={handleaddnewcourse}> افزودن درس جدید  +</button>
        </div>

        
        <div className="rectangle-3" />

        {/* برای نمایش تاریخ و زمان */}
        <div className="date">{dateTime.toLocaleDateString('fa-IR')}</div>
        <div className="clock">{dateTime.toLocaleTimeString('fa-IR')}</div>

        {/* برای سرچ باکس*/}
        <div className="search-container">
        <FaSearch className="search-icon"/>
        <input type="text" placeholder="جستجو کنید..." value={value} onChange={handleChange} className="search-input"/>
        </div>

        {/*برای دکمه ثبت درس*/}
        <div className="register-course">
            <button className="btn-reg-course">ثبت ویرایش</button>
        </div>

      </div>
    </div>
  );
};

export default EditCourse;