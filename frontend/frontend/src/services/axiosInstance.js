import axios from 'axios';

const axiosInstance = axios.create({
  // آدرس باید دقیقاً با BASE_URL در Authoservice یکی باشد
  baseURL: "https://localhost:7194/api", 
});

axiosInstance.interceptors.request.use(
  (config) => {
    // گرفتن توکن از LocalStorage که در مرحله لاگین ذخیره کردیم
    const token = localStorage.getItem("token") || localStorage.getItem("accessToken"); 
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default axiosInstance;