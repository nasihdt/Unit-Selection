import React from "react";
import { FaSearch} from "react-icons/fa"
import { MdDashboard } from "react-icons/md";
import { MdMenuBook } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import delet from "../components/delete-course.png"
import edit from "../components/edit-course.png"
import Logo from "../components/logo-chamran.png"
import "./styles/ManagementCourse.css";
import flash from "../components/flash.png"
import { useState, useEffect } from "react";

 
const ManagementCourse = () => {

  
  const [value, setValue] = useState("");

   const handleChange = (e) => {
  setValue(e.target.value);
  };
  const navigate = useNavigate();

  const handleaddnewcourse = () =>{
    navigate('/add-new-course')
  }
  
  const handledashboard = () =>{
    navigate('/dashboard')
  }

  const handleedit = () =>{
    navigate('/edit')
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

        <div className="dashboard">
          <button className="btn_dashdoardadmin" onClick={handledashboard}>داشبورد</button>
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
        <button className="btn-addcoursemanage" alt="altbtncourse" onClick={handleaddnewcourse}> افزودن درس جدید  +</button>
        </div>

        
        <div className="rectangle-3" />

        {/* برای نمایش تاریخ و زمان */}
        <div className="date">{dateTime.toLocaleDateString('fa-IR')}</div>
        <div className="clock">{dateTime.toLocaleTimeString('fa-IR')}</div>

        <div className="search-container">
        <FaSearch className="search-icon"/>
        <input type="text" placeholder="جستجو کنید..." value={value} onChange={handleChange} className="search-input"/>
        </div>
        
        <button className="btn-img-delete">
          <img src={delet} alt="delete img" className="img_delete"/>
        </button>

        <button className="btn-img-delete" onClick={handleedit}>
          <img src={edit} alt="delete img" className="img_delete"/>
        </button>

        {/* <button className="btn-flash-bask">
          <img src={flash} alt="flash back" className="img-flash-back"/>
        </button> */}

      </div>
    </div>
  );
};

export default ManagementCourse;