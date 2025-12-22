

import React from "react";
import { FaUser} from "react-icons/fa"
import { MdDashboard } from "react-icons/md";
import { MdMenuBook } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import Logo from "../components/logo-chamran.png"
import "./styles/UnitLimitSetter.css";
import { useState, useEffect } from "react";
import { FaBook } from "react-icons/fa"; 
import { MdOutlineKeyboardArrowUp, MdOutlineKeyboardArrowDown } from "react-icons/md"; // آیکون فلش

 
const UnitLimitSetter = () => {

  const navigate = useNavigate();

  const handledashboard = () => navigate("/dashboard");
  const handlemanagecourse = () => navigate("/management");

  const handlelimit = () =>{navigate('/limit')};
  //پارامتر های تایم
  const [dateTime, setDateTime] = useState(new Date());

  // برای نمایش تاریخ و زمان
  useEffect(() => {
    const timer = setInterval(() => {
      setDateTime(new Date());
    }, 1000);

    return () => clearInterval(timer); 
  }, []);


  const [minUnits, setMinUnits] = useState(12);
  const [maxUnits, setMaxUnits] = useState(18);
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(false);

  const MIN_LIMIT = 0;
  const MAX_LIMIT = 30;

  const handleMinChange = (value) => {
    const newVal = Math.max(MIN_LIMIT, Math.min(value, MAX_LIMIT));
    setMinUnits(newVal);
    if (newVal > maxUnits) setMessage("⚠ حداقل نباید بزرگتر از حداکثر باشد");
    else setMessage("");
  };

  const handleMaxChange = (value) => {
    const newVal = Math.max(MIN_LIMIT, Math.min(value, MAX_LIMIT));
    setMaxUnits(newVal);
    if (newVal < minUnits) setMessage("⚠ حداکثر نباید کمتر از حداقل باشد");
    else setMessage("");
  };

  const saveSettings = async () => {
    if (minUnits > maxUnits) {
      setMessage("⚠ حداقل نباید بزرگتر از حداکثر باشد");
      return;
    }

    setLoading(true);
    setMessage("");

    try {
      const response = await fetch("https://localhost:5127/api/unitLimits", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ minUnits, maxUnits }),
      });
      if (!response.ok) throw new Error(`خطا در ذخیره‌سازی: ${response.status}`);
      setMessage("تنظیمات با موفقیت ذخیره شد ✅");
    } catch (error) {
      setMessage(`⚠ خطا در اتصال به سرور: ${error.message}`);
    } finally {
      setLoading(false);
    }
  };

  


  return (
    <div className="container">
      <div className="frame">
        <div className="rectangle" />

        <div className="dashboard">
          <button className="btn_dash_admin_unit" onClick={handledashboard}>داشبورد</button>
          <div className="icon_doshboard">
            <MdDashboard className="icon" />
          </div>

          <div className="div" />

          <button className="btn_manage_course_unit" onClick={handlemanagecourse}>مدیریت دروس</button>
          <div className="icon_manage_course">
            <MdMenuBook className="icon" />
          </div>

          <button className="btn_limit_unit_unit" onClick={handlelimit}>تعیین حد واحد</button>
          <div className="icon_limitunit">
            <FaBook className="icon" />
          </div>
        </div>

        <img className="shahid-chamran" alt="Shahid chamran" src={Logo}/>

        <div className="rectangle-3" />

        <div className="icon_user">
            <FaUser className="icon" />
        </div>

        {/* برای نمایش تاریخ و زمان */}
        <div className="date">{dateTime.toLocaleDateString('fa-IR')}</div>
        <div className="clock">{dateTime.toLocaleTimeString('fa-IR')}</div>

           {/*  UI تعیین حد واحد */}
           <div className="unit-limit-container">
           <h2 className="unit-limit-title">تعیین حد واحدهای اخذ شده</h2>

           {/* حداقل واحد */}
           <div className="unit-input">
           <label className="labmax">حداقل واحد</label>
             <div className="input-controls">
               
               <input className="inp-max" type="number" value={minUnits} onChange={(e) => handleMinChange(Number(e.target.value))} disabled={loading} />
               
             </div>
           </div>

           {/* حداکثر واحد */}
           <div className="unit-input">
             <label className="labmin">حداکثر واحد</label>
            <div className="input-controls">
              
               <input className="inp-min" type="number" value={maxUnits} onChange={(e) => handleMaxChange(Number(e.target.value))} disabled={loading} />
              
             </div>
           </div>

           {message && <p className={`message ${message.includes("⚠") ? "error" : "success"}`}>{message}</p>}

           <button className="save-btn" onClick={saveSettings} disabled={loading}>
             {loading ? "در حال ذخیره..." : "ذخیره"}
           </button>
         </div>

      </div>
    </div>
  );
};

export default UnitLimitSetter;