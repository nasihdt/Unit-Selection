import React from "react";
import { FaUser} from "react-icons/fa"
import { MdDashboard } from "react-icons/md";
import { MdMenuBook } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import dashboard from "../components/dashboard_img.webp";
import Logo from "../components/logo-chamran.png"
import "./styles/TeacherDashboard.css";
import { useState, useEffect } from "react";
import { FiLogOut } from "react-icons/fi";
 
const AdminDashboard = () => {

  const navigate = useNavigate();

  const handlelogin = () =>{
    navigate('/login')
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
          <button className="btn_dashboardadmin">داشبورد</button>
          <div className="icon_doshboard">
            <MdDashboard className="icon" />
          </div>

          <div className="div" />

        </div>
        <button className="exit-icon-thr" onClick={handlelogin}>
         <FiLogOut className="icon-thr-exit"/>   
        </button> 

        <img className="shahid-chamran" alt="Shahid chamran" src={Logo}/>

        <div className="rectangle-2" />

        <p className="p">استاد عزیز به داشبورد خود خوش آمدید</p>

        <img className="login-page" alt="Login page" src={dashboard} />

        <div className="rectangle-3" />

        <div className="icon_user_thr">
            <FaUser className="icon_thr" />
        </div>

        {/* برای نمایش تاریخ و زمان */}
        <div className="date">{dateTime.toLocaleDateString('fa-IR')}</div>
        <div className="clock">{dateTime.toLocaleTimeString('fa-IR')}</div>

      </div>
    </div>
  );
};

export default AdminDashboard;