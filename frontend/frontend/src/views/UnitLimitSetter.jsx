import React from "react";
import { FaUser} from "react-icons/fa"
import { MdDashboard } from "react-icons/md";
import { MdMenuBook } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import Logo from "../components/logo-chamran.png"
import "./styles/UnitLimitSetter.css";
import { useState, useEffect } from "react";
import { FaBook } from "react-icons/fa"; 
import { FiLogOut } from "react-icons/fi";
import axiosInstance from "../services/axiosInstance";

 
const UnitLimitSetter = () => {

  const navigate = useNavigate();

  const handledashboard = () => navigate("/dashboard");
  const handlemanagecourse = () => navigate("/management");
  const handlelogin = () =>{
    navigate('/login')
  }
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
  const MAX_LIMIT = 24;

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
  // چک فرانت برای جلوگیری از درخواست اشتباه
  if (minUnits > maxUnits) {
    setMessage("⚠ حداقل نباید بزرگتر از حداکثر باشد");
    return;
  }

  setLoading(true);
  setMessage("");

  try {
    // ارسال درخواست به بک‌اند با axiosInstance
    await axiosInstance.put("/admin/settings/units", { minUnits, maxUnits });

    setMessage("تنظیمات با موفقیت ذخیره شد ✅");
  } catch (error) {
    // گرفتن پیام خطا از بک‌اند (مثلاً 400 BadRequest)
    const backendMsg =
      error?.response?.data?.message ||
      error?.response?.data ||
      "⚠ مقدار وارد شده نامعتبر است";

    setMessage(backendMsg);
    console.error("Save settings error:", error);
  } finally {
    setLoading(false);
  }
};


  return (
    <div className="container">
  {/* سایدبار */}
  <aside className="sidebar">
  <img className="logo" src={Logo} alt="Shahid Chamran Logo" />

  <nav className="sidebar-menu">
    <button onClick={handledashboard} className="side-btn">
      <MdDashboard className="icon" />
      داشبورد
    </button>

    <button onClick={handlemanagecourse} className="side-btn">
      <MdMenuBook className="icon" />
      مدیریت دروس
    </button>

    <button onClick={handlelimit} className="side-btn active">
      <FaBook className="icon" />
      تعیین حد واحد
    </button>
  </nav>
</aside>

  {/* محتوای اصلی */}
  <main className="main-content">
    <header className="top-bar">
      <FaUser />
      <span>{dateTime.toLocaleDateString('fa-IR')}</span>
      <span>{dateTime.toLocaleTimeString('fa-IR')}</span>

      <button className="logout" onClick={handlelogin}>
        <FiLogOut />
      </button>
    </header>

    <section className="unit-limit-container">
      <h2>تعیین حد واحدهای اخذ شده</h2>

      <div className="unit-input">
        <label>حداقل واحد</label>
        <input
          type="number"
          value={minUnits}
          onChange={(e) => handleMinChange(+e.target.value)}
        />
      </div>

      <div className="unit-input">
        <label>حداکثر واحد</label>
        <input
          type="number"
          value={maxUnits}
          onChange={(e) => handleMaxChange(+e.target.value)}
        />
      </div>

      {message && (
        <p className={`message ${message.includes("⚠") ? "error" : "success"}`}>
          {message}
        </p>
      )}

      <button className="save-btn" onClick={saveSettings} disabled={loading}>
        {loading ? "در حال ذخیره..." : "ذخیره"}
      </button>
    </section>
  </main>
</div>
  );
};

export default UnitLimitSetter;