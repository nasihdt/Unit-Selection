import React from "react";
import { FaSearch} from "react-icons/fa"
import { MdDashboard } from "react-icons/md";
import { MdMenuBook } from "react-icons/md";
import { useNavigate } from "react-router-dom";

import Logo from "../components/logo-chamran.png"
import "./styles/AddCourse.css";
import { useState, useEffect } from "react";

 
const AddCourse = () => {

  
  const [value, setValue] = useState("");
  
  const handleChange = (e) => {
    setValue(e.target.value);
  };
  const navigate = useNavigate();
  
  const handledashboard = () =>{
    navigate('/dashboard')
  }
  const handlemanagecourse = () =>{
    navigate('/management')
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

       {/* لیستی که برای اضافه کردن درس باید پر کنیم*/}
       <div class="field-name">
          <label class="label-name">نام درس <span class="assign1">*</span></label>
          <input type="text" class="ipt-name"/>
       </div>
            
        <div class="field-code">
          <label class="label-code">کد درس<span class="assign2">*</span></label>
          <input type="text" class="ipt-code"/>
       </div>

        <div class="field-vahed">
          <label class="label-vahed">واحد<span class="assign3">*</span></label>
          <input type="text" class="ipt-vahed"/>
       </div>

        <div class="field-capacity">
          <label class="label-capacity">ظرفیت<span class="assign4">*</span></label>
          <input type="text" class="ipt-capacity"/>
       </div>

        <div class="field-teacher">
          <label class="label-teacher">نام استاد<span class="assign5">*</span></label>
          <input type="text" class="ipt-teacher"/>
       </div>

        <div class="field-time">
          <label class="label-time">زمان<span class="assign6">*</span></label>
          <input type="date" class="ipt-time"/>
       </div>

        <div class="field-place">
          <label class="label-place">مکان<span class="assign7">*</span></label>
          <input type="text" class="ipt-place"/>
       </div>

        <div class="field-examtime">
          <label class="label-examtime">تاریخ امتحان<span class="assign8">*</span></label>
          <input type="date" class="ipt-examtime"/>
       </div>   
         
         
        <div className="dashboard">
          <button className="btn_dashdoard-add" onClick={handledashboard}>داشبورد</button>
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

        <div className="box">
        <button className="btn-addcourse-add" alt="altbtncourse" > افزودن درس جدید  +</button>
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
            <button className="btn-reg-course">ثبت درس</button>
        </div>

      </div>
    </div>
  );
};

export default AddCourse;