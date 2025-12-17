// src/services/axiosInstance.js
import axios from "axios";

const axiosInstance = axios.create({
  baseURL: "https://localhost:7194/api", // ✅ HTTPS صحیح
  headers: {
    "Content-Type": "application/json",
  },
});

// اضافه کردن خودکار توکن
axiosInstance.interceptors.request.use(
  (config) => {
    const token =
      localStorage.getItem("accessToken") ||
      localStorage.getItem("token");

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => Promise.reject(error)
);

export default axiosInstance;
