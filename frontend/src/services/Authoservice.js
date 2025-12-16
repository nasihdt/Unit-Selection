
// import axiosInstance from './axiosInstance';
// import axios from 'axios';

// // LOGIN
// export const loginAdmin = (username, password) =>
//   axiosInstance.post("/admin/login", { username, password }).then(res => res.data);

// export const loginStudent = (username, password) => {
//   return axiosInstance.post("/student/login", {
//     username,    
//     password
//   }).then(res => res.data);
// };

// // 🔑 Professor
// export const loginProfessor = (username, password) => {
//   return axiosInstance.post("/professor/login", {
//     username,     
//     password
//   }).then(res => res.data);
// };
// // REFRESH TOKEN (بدون interceptor)
// export const refreshAccessToken = async () => {
//   const refreshToken = localStorage.getItem("refreshToken");
//   if (!refreshToken) return null;

//   try {
//     const res = await axios.post(
//       "http://localhost:5127/api/auth/refresh",
//       { token: refreshToken }
//     );

//     localStorage.setItem("token", res.data.accessToken);
//     return res.data.accessToken;
//   } catch {
//     return null;
//   }
// };

import axiosInstance from "./axiosInstance";
import axios from "axios";

// =======================
// LOGIN
// =======================
export const loginAdmin = async (username, password) => {
  const res = await axiosInstance.post("/admin/login", { username, password });

  localStorage.setItem("token", res.data.accessToken);
  localStorage.setItem("refreshToken", res.data.refreshToken);

  return res.data;
};

export const loginStudent = async (username, password) => {
  const res = await axiosInstance.post("/student/login", { username, password });

  localStorage.setItem("token", res.data.accessToken);
  localStorage.setItem("refreshToken", res.data.refreshToken);

  return res.data;
};

export const loginProfessor = async (username, password) => {
  const res = await axiosInstance.post("/professor/login", { username, password });

  localStorage.setItem("token", res.data.accessToken);
  localStorage.setItem("refreshToken", res.data.refreshToken);

  return res.data;
};

// =======================
// REFRESH TOKEN
// =======================
export const refreshAccessToken = async () => {
  const refreshToken = localStorage.getItem("refreshToken");
  if (!refreshToken) return null;

  try {
    const res = await axios.post(
      "https://localhost:5127/api/auth/refresh",
      { refreshToken }
    );

    localStorage.setItem("token", res.data.accessToken);
    localStorage.setItem("refreshToken", res.data.refreshToken);

    return res.data.accessToken;
  } catch {
    localStorage.clear();
    return null;
  }
};