
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

/*import axiosInstance from "./axiosInstance";
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
};*/





// import axiosInstance from "./axiosInstance";
// import axios from "axios";

// // =======================
// // LOGIN - ADMIN
// // =======================
// export const loginAdmin = async (username, password) => {
//   const res = await axiosInstance.post("/admin/login", { username, password });

//   localStorage.setItem("accessToken", res.data.token);
//   localStorage.setItem("refreshToken", res.data.refreshToken);

//   return res.data;
// };

// // =======================
// // LOGIN - STUDENT
// // =======================
// export const loginStudent = async (username, password) => {
//   const res = await axiosInstance.post("/student/login", { username, password });

//   localStorage.setItem("accessToken", res.data.token);
//   localStorage.setItem("refreshToken", res.data.refreshToken);

//   return res.data;
// };

// // =======================
// // LOGIN - PROFESSOR
// // =======================
// export const loginProfessor = async (username, password) => {
//   const res = await axiosInstance.post("/professor/login", { username, password });

//   localStorage.setItem("accessToken", res.data.token);
//   localStorage.setItem("refreshToken", res.data.refreshToken);

//   return res.data;
// };

// // =======================
// // REFRESH TOKEN (ADMIN)
// // =======================
// export const refreshAccessToken = async () => {
//   const refreshToken = localStorage.getItem("refreshToken");
//   if (!refreshToken) return null;

//   try {
//     const res = await axios.post(
//       "http://localhost:5127/api/admin/refresh",
//       { refreshToken }
//     );

//     localStorage.setItem("accessToken", res.data.token);
//     localStorage.setItem("refreshToken", res.data.refreshToken);

//     return res.data.token;
//   } catch {
//     localStorage.clear();
//     return null;
//   }
// };
// src/services/Authoservice.js
import axiosInstance from "./axiosInstance";
import axios from "axios";

// =======================
// LOGIN - ADMIN
// =======================
export const loginAdmin = async (username, password) => {
  const res = await axiosInstance.post("/Admin/login", { username, password });

  localStorage.setItem("accessToken", res.data.token);
  localStorage.setItem("refreshToken", res.data.refreshToken);
  localStorage.setItem("role", res.data.role);

  return res.data;
};

// =======================
// LOGIN - STUDENT
// =======================
export const loginStudent = async (username, password) => {
  const res = await axiosInstance.post("/student/login", { username, password });

  localStorage.setItem("accessToken", res.data.token);
  localStorage.setItem("refreshToken", res.data.refreshToken);
  localStorage.setItem("role", res.data.role);

  return res.data;
};

// =======================
// LOGIN - PROFESSOR
// =======================
export const loginProfessor = async (username, password) => {
  const res = await axiosInstance.post("/professor/login", { username, password });

  localStorage.setItem("accessToken", res.data.token);
  localStorage.setItem("refreshToken", res.data.refreshToken);
  localStorage.setItem("role", res.data.role);

  return res.data;
};

// =======================
// REFRESH TOKEN (ADMIN)
// =======================
export const refreshAccessToken = async () => {
  const refreshToken = localStorage.getItem("refreshToken");
  if (!refreshToken) return null;

  try {
    const res = await axios.post(
      "http://localhost:7194/api/admin/refresh",
      { refreshToken }
    );

    localStorage.setItem("accessToken", res.data.token);
    localStorage.setItem("refreshToken", res.data.refreshToken);

    return res.data.token;
  } catch (err) {
    console.error("REFRESH TOKEN FAILED:", err);
    localStorage.clear();
    return null;
  }
};

