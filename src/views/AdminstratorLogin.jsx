import React from 'react'
import './styles/AdminstratorLogin.css'
import {FaUser, FaLock} from "react-icons/fa"
import Adminlogin_img from "../components/Adminlogin-image.jpg"
import Logo from "../components/logo-chamran.png"

const AdminstratorLogin = () =>{
    return(
  
     <div className="login-container">
      
      <div className="login-image">
        <img src={Adminlogin_img} alt="Login" />
      </div>
      <div className="logo">
        <img src={Logo} alt="logo-chamran"/>
      </div>

      {/* بخش فرم */}
      <div className="login-form">
        <div className="header">
          <div className="title_page">ورود به سامانه</div>
          <div className="underline"></div>
        </div>
        <div className="inputs">
          <div className="input-wrapper">
            <FaUser className="icon" />
            <input type="text" placeholder="نام کاربری" />
          </div>
          <div className="input-wrapper">
            <FaLock className="icon" />
            <input type="password" placeholder="رمز عبور" />
          </div>
        </div>

        <button className="Login_btn">ورود</button>
      </div>
    </div>
    )
}



export default AdminstratorLogin

