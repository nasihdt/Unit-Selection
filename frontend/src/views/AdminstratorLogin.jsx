
import './styles/AdminstratorLogin.css'
import {FaUser, FaLock} from "react-icons/fa"
import Adminlogin_img from "../components/Adminlogin-image.jpg"
import Logo from "../components/logo-chamran.png"
import { useNavigate } from "react-router-dom";
import { loginAdmin } from '../services/Authoservice';
import React, { useState } from 'react';

const AdminstratorLogin = () =>{
  
  const navigate = useNavigate();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
     if (!username.trim() || !password.trim()) {
    setError("نام کاربری و رمز عبور نباید خالی باشند!");
    return;
  }

  try {
    const data = await loginAdmin(username, password);

    localStorage.setItem('token', data.token);
    console.log("LOGIN RESPONSE:", data);

    
    // if(username === "student" && password === '1234'){
    //   navigate('/dashboardstd')
    // }
    
    navigate("/dashboard"); 
    // alert('Login successful!');
  } catch (err) {
    setError(err.message || "نام کاربری یا رمز عبور اشتباه است");
  }
  };

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
            <input type="text" placeholder="نام کاربری" value={username} onChange={(e) => setUsername(e.target.value)} />
          </div>

          <div className="input-wrapper">
            <FaLock className="icon" />
            <input type="password" placeholder="رمز عبور" value={password} onChange={(e) => setPassword(e.target.value)}/>
          </div>
        </div>

        <button className="Login_btn" onClick={handleSubmit}>ورود</button>

       {error && <p style={{color:'red'}}>{error}</p>}
      </div>
    </div>
    )
}



export default AdminstratorLogin

