
// import axios from 'axios';

// const axiosInstance = axios.create({
//   baseURL: "http://localhost:5127/api",
// });


// axiosInstance.interceptors.request.use(
//   config => {
//     const token = localStorage.getItem("token");
//     if (token && !config.url.includes("login")) {
//       config.headers.Authorization = `Bearer ${token}`;
//     }
//     return config;
//   },
//   error => Promise.reject(error)
// );

// export default axiosInstance;
//--------------------------------------
// import axios from "axios";

// const axiosInstance = axios.create({
//   baseURL: "https://localhost:5127/api"
// });

// axiosInstance.interceptors.request.use(
//   config => {
//     const token = localStorage.getItem("token");
//     if (token) {
//       config.headers.Authorization = `Bearer ${token}`;
//     }
//     return config;
//   },
//   error => Promise.reject(error)
// );

// export default axiosInstance;
//-----------------------------------

import axios from "axios";

const axiosInstance = axios.create({
  baseURL: "https://localhost:5127/api", // ⬅️ HTTPS
});

// اضافه کردن خودکار Authorization
axiosInstance.interceptors.request.use(
  config => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  error => Promise.reject(error)
);

export default axiosInstance;