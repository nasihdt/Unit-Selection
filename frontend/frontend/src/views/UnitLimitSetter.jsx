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
    const token = localStorage.getItem("token");

const response = await fetch(
  "http://localhost:5127/api/admin/settings/units",
  {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${token}`
    },
    body: JSON.stringify({ minUnits, maxUnits }),
  }
);

    if (!response.ok) {
      throw new Error(`خطای سرور: ${response.status}`);
    }

    setMessage("تنظیمات با موفقیت ذخیره شد ✅");
  } catch (error) {
    setMessage("⚠ خطا در ارتباط با سرور");
    console.error(error);
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