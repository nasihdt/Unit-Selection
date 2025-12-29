import axios from "axios";

// حتماً چک کن که پورت بک‌اِندت در ویژوال استودیو همین 7194 باشد
const BASE_URL = "https://localhost:7194/api";

// =======================
// LOGIN ADMIN
// =======================
export const loginAdmin = async (username, password) => {
  const res = await axios.post(`${BASE_URL}/Admin/login`, { username, password });
  const token = res.data.token || res.data.accessToken;
  
  if (token) {
    localStorage.setItem("token", token);
    localStorage.setItem("accessToken", token); // برای هماهنگی با بقیه کامپوننت‌ها
    localStorage.setItem("refreshToken", res.data.refreshToken);
  }
  return res.data;
};

// =======================
// LOGIN STUDENT
// =======================
export const loginStudent = async (username, password) => {
  const res = await axios.post(`${BASE_URL}/Student/login`, { username, password });
  const token = res.data.token || res.data.accessToken;
  
  if (token) {
    localStorage.setItem("token", token);
    localStorage.setItem("accessToken", token);
    localStorage.setItem("refreshToken", res.data.refreshToken);
  }
  return res.data;
};

// =======================
// LOGIN PROFESSOR
// =======================
export const loginProfessor = async (username, password) => {
  const res = await axios.post(`${BASE_URL}/Professor/login`, { username, password });
  const token = res.data.token || res.data.accessToken;
  
  if (token) {
    localStorage.setItem("token", token);
    localStorage.setItem("accessToken", token);
    localStorage.setItem("refreshToken", res.data.refreshToken);
  }
  return res.data;
};

// =======================
// REFRESH TOKEN
// =======================
export const refreshAccessToken = async () => {
  const refreshToken = localStorage.getItem("refreshToken");
  if (!refreshToken) return null;
  try {
    const res = await axios.post(`${BASE_URL}/Admin/refresh`, { refreshToken });
    const newToken = res.data.token || res.data.accessToken;
    
    localStorage.setItem("token", newToken);
    localStorage.setItem("accessToken", newToken);
    return newToken;
  } catch (error) {
    localStorage.clear();
    return null;
  }
};