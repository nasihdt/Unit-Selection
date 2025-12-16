import React from "react";
import { FaUser} from "react-icons/fa"
import { MdDashboard } from "react-icons/md";
import { MdMenuBook } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import dashboard from "../components/dashboard_img.webp";
import Logo from "../components/logo-chamran.png"
import "./styles/StudentDashboard.css";
import { useState, useEffect } from "react";
 
const AdminDashboard = () => {

  const navigate = useNavigate();

  const handleManageCourse = () => {
    navigate('/courseoffere');
  };
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
          <button className="btn_dashboardadmin_Student">داشبورد</button>
          <div className="icon_doshboard">
            <MdDashboard className="icon" />
          </div>

          <div className="div" />

          <button className="btn_offercourse_student" onClick={handleManageCourse}>لیست دروس ارائه شده</button>
          <div className="icon_manage_course">
            <MdMenuBook className="icon" />
          </div>
        </div>

        <img className="shahid-chamran" alt="Shahid chamran" src={Logo}/>

        <div className="rectangle-2" />

        <p className="p">دانشجو عزیز به داشبورد خود خوش آمدید</p>

        <img className="login-page" alt="Login page" src={dashboard} />

        <div className="rectangle-3" />

        <div className="icon_user">
            <FaUser className="icon" />
        </div>

        {/* برای نمایش تاریخ و زمان */}
        <div className="date">{dateTime.toLocaleDateString('fa-IR')}</div>
        <div className="clock">{dateTime.toLocaleTimeString('fa-IR')}</div>

      </div>
    </div>
  );
};

export default AdminDashboard;