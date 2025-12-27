import React from "react";
import { FaUser} from "react-icons/fa"
import { MdDashboard } from "react-icons/md";
import { MdMenuBook } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import dashboard from "../components/dashboard_img.webp";
import Logo from "../components/logo-chamran.png"
import "./styles/StudentDashboard.css";
import { useState, useEffect } from "react";
import { FiLogOut } from "react-icons/fi";
import { FaClipboardList } from "react-icons/fa"
 
const AdminDashboard = () => {

  const navigate = useNavigate();


  const handlelogin = () =>{
    navigate('/login')
  }
  const handleOfferCourse = () => {
    navigate('/courseoffere');
  };

  const handleselectionunit = () =>{
    navigate('/selectunit')
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
          <button className="btn_dashboardadmin_Student">داشبورد</button>
          <div className="icon_doshboard">
            <MdDashboard className="icon" />
          </div>

          <div className="div" />

          <button className="btn_offercourse_student" onClick={handleOfferCourse}>لیست دروس ارائه شده</button>
          <div className="icon_manage_course">
            <MdMenuBook className="icon" />
          </div>

          <button className="btn_choose_unit_dash" onClick={handleselectionunit}>انتخاب واحد</button>
          <div className="icon_selection_unit_dash">
            <FaClipboardList className="icon"/>
          </div> 
        </div>

        <button className="icon_exit_Std_dashboard" onClick={handlelogin}>
          <FiLogOut className="exit_std"/>     
        </button>

        <img className="shahid-chamran" alt="Shahid chamran" src={Logo}/>

        <div className="rectangle-2" />

        <p className="p">دانشجو عزیز به داشبورد خود خوش آمدید</p>

        <img className="login-page" alt="Login page" src={dashboard} />

        <div className="rectangle-3" />

        <div className="icon_user_std">
            <FaUser className="icon_std" />
        </div>

        {/* برای نمایش تاریخ و زمان */}
        <div className="date">{dateTime.toLocaleDateString('fa-IR')}</div>
        <div className="clock">{dateTime.toLocaleTimeString('fa-IR')}</div>

      </div>
    </div>
  );
};

export default AdminDashboard;