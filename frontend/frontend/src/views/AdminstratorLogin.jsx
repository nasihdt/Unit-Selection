

import './styles/AdminstratorLogin.css';
import { FaUser, FaLock } from "react-icons/fa";
import Adminlogin_img from "../components/Adminlogin-image.jpg";
import Logo from "../components/logo-chamran.png";
import { useNavigate } from "react-router-dom";
import { loginAdmin, loginStudent, loginProfessor } from '../services/Authoservice';
import React, { useState } from 'react';

const AdminstratorLogin = () => {
  const navigate = useNavigate();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [role, setRole] = useState('Student'); // پیش‌فرض Admin
  const [error, setError] = useState('');

  

  const handleSubmit = async (e) => {
  e.preventDefault();
  setError("");

  if (!username.trim() || !password.trim()) {
    setError("نام کاربری و رمز عبور نباید خالی باشند!");
    return;
  }

  try {
    let data;

    // فقط بر اساس radio endpoint را انتخاب کن
    if (role === "Admin") {
      data = await loginAdmin(username, password);
    } 
    else if (role === "Student") {
      data = await loginStudent(username, password);
    } 
    else if (role === "Professor") {
      data = await loginProfessor(username, password);
    }

   console.log("SEND BODY:", {
  role,
  username,
  password
});

    // ✅ ذخیره مقادیر واقعی از بک‌اند
    localStorage.setItem("accessToken", data.token);
    localStorage.setItem("refreshToken", data.refreshToken);
    localStorage.setItem("role", data.role);

    

    // ✅ هدایت بر اساس role بک‌اند
    switch (data.role) {
      case "Admin":
        navigate("/dashboard");
        break;
      case "Student":
        navigate("/dashboardstd");
        break;
      case "Professor":
        navigate("/dashboardproff");
        break;
      default:
        setError("نقش کاربر نامعتبر است");
    }

  } catch (err) {
    console.log("LOGIN ERROR:", err.response);
    setError(err.response?.data?.message || "نام کاربری یا رمز عبور اشتباه است");
  }
};

  return (
    <div className="login-container">
      <div className="login-image">
        <img src={Adminlogin_img} alt="Login" />
      </div>

      <div className="logo">
        <img src={Logo} alt="logo-chamran" />
      </div>

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
            <input type="password" placeholder="رمز عبور" value={password} onChange={(e) => setPassword(e.target.value)} />
          </div>
        </div>

        <div className="role-selector">
          <label>
            <input type="radio" value="Admin" checked={role === "Admin"} onChange={(e) => setRole(e.target.value)} />
            دانشجو
          </label>
          <label>
            <input type="radio" value="Student" checked={role === "Student"} onChange={(e) => setRole(e.target.value)} />
            استاد
          </label>
          <label>
            <input type="radio" value="Professor" checked={role === "Professor"} onChange={(e) => setRole(e.target.value)} />
            مدیر
          </label>
        </div>

        <button className="Login_btn" onClick={handleSubmit}>ورود</button>

        {error && <p style={{ color: 'red' }}>{error}</p>}
      </div>
    </div>
  );
};

export default AdminstratorLogin;
