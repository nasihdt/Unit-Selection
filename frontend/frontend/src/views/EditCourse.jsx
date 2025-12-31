import React, { useState, useEffect } from "react";
import { FaSearch } from "react-icons/fa";
import { MdDashboard, MdMenuBook } from "react-icons/md";
import { useNavigate, useParams } from "react-router-dom";
import Logo from "../components/logo-chamran.png";
import "./styles/EditCourse.css";
import { FaBook } from "react-icons/fa"; 
import { FiLogOut } from "react-icons/fi";
import axiosInstance from "../services/axiosInstance";

const EditCourse = () => {
  const { courseId } = useParams();
  const navigate = useNavigate();
  const token = localStorage.getItem("token");
 
  const [course, setCourse] = useState({
    name: "",
    code: "",
    vahed: "",
    capacity: "",
    teacher: "",
    time: "",
    place: "",
    examDate: "",
    description: "",
  });

  const [value, setValue] = useState("");
  const [dateTime, setDateTime] = useState(new Date());
  const [searchValue, setSearchValue] = useState("");
  const [allCourses, setAllCourses] = useState([]);
  const [selectedPrerequisites, setSelectedPrerequisites] = useState([]);
  const [open, setOpen] = useState(false);

 useEffect(() => {
  const fetchPrereq = async () => {
    try {
      const res = await axiosInstance.get(
        `/admin/courses/${courseId}/prerequisites`
      );
      // فقط id پیش‌نیازها رو بگیر
      setSelectedPrerequisites(res.data.map(p => p.id));
    } catch (err) {
      console.error("خطا در دریافت پیش‌نیازها", err);
    }
  };

  fetchPrereq();
}, [courseId]);

  // useEffect(() => {
  //   const fetchCourse = async () => {
  //     try {
  //       const res = await fetch(`http://localhost:5127/api/Course/${courseId}`, {
  //         headers: { Authorization: `Bearer ${token}` },
  //       });
  //       if (res.ok) {
  //         const data = await res.json();
  //         setCourse({
  //           name: String(data.title || ""),
  //           code: String(data.code || ""),
  //           vahed: String(data.units || ""),
  //           capacity: String(data.capacity || ""),
  //           teacher: String(data.teacherName || ""),
  //           time: String(data.time || ""),
  //           place: String(data.location || ""),
  //           // examDate: String(data.examDate || ""),
  //           description: String(data.description || ""),
  //         });
  //       } else {
  //         console.error("خطا در دریافت داده درس");
  //       }
  //     } catch (error) {
  //       console.error(error);
  //     }
  //   };
  //   fetchCourse();
  // }, [courseId, token]);

  useEffect(() => {
  const fetchCourse = async () => {
    try {
      const res = await axiosInstance.get(`/Course/${courseId}`);

      const data = res.data;
      setCourse({
        name: data.title ?? "",
        code: data.code ?? "",
        vahed: String(data.units ?? ""),
        capacity: String(data.capacity ?? ""),
        teacher: data.teacherName ?? "",
        time: data.time ?? "",
        place: data.location ?? "",
        examDate: data.examDate ?? "",
        description: data.description ?? "",
      });
    } catch (error) {
      console.error("خطا در دریافت درس:", error);
    }
  };

  fetchCourse();
}, [courseId]);

  // تایمر تاریخ و ساعت
  useEffect(() => {
    const timer = setInterval(() => setDateTime(new Date()), 1000);
    return () => clearInterval(timer);
  }, []);

  // بروزرسانی فرم
  const handleChange = (e) => {
    setCourse({ ...course, [e.target.name]: e.target.value });
  };

  // اعتبارسنجی
  // const handleUpdate = async () => {
  //   for (let key in course) {
  //     if (course[key].trim() === "") {
  //       alert(`لطفاً فیلد ${key} را پر کنید!`);
  //       return;
  //     }
  //   }
  //  // map کردن داده‌ها مطابق API
  //   const payload = {
  //     title: course.name,
  //     code: course.code,
  //     units: parseInt(course.vahed),
  //     capacity: parseInt(course.capacity),
  //     teacherName: course.teacher,
  //     time: course.time,
  //     location: course.place,
  //     // examDate: course.examDate,
  //     description: course.description,
  //   };

  //   try {
  //     const res = await fetch(`http://localhost:5127/api/Course/${courseId}`, {
  //       method: "PUT",
  //       headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` },
  //       body: JSON.stringify(payload),
  //     });

  //     if (res.ok) {
  //       alert("ویرایش درس با موفقیت انجام شد!");
  //       navigate("/management");
  //     } else {
  //       const errorText = await res.text();
  //       alert("خطا در ویرایش درس: " + errorText);
  //     }
  //   } catch (error) {
  //     console.error(error);
  //     alert("مشکل در ارتباط با سرور");
  //   }
  // };
  const handleUpdate = async () => {
  
  const requiredFields = ["name", "code", "vahed", "capacity", "teacher"];

for (let key of requiredFields) {
  if (!course[key] || course[key].toString().trim() === "") {
    alert(`لطفاً فیلد ${key} را پر کنید`);
    return;
  }
}

  // آماده‌سازی payload برای PUT
  const payload = {
    title: course.name,
    code: course.code,
    units: parseInt(course.vahed),
    capacity: parseInt(course.capacity),
    teacherName: course.teacher,
    time: course.time,
    location: course.place,
    description: course.description,
    examDate: course.examDate,
  };

  try {
    // 1️⃣ آپدیت درس اصلی
    const res = await axiosInstance.put(
      `/Course/${courseId}`,
      payload
    );

    if (res.status === 200 || res.status === 204) {
      // 2️⃣ بعد از موفقیت PUT، آپدیت پیش‌نیازها
      if (selectedPrerequisites.length > 0) {
  for (const prereqId of selectedPrerequisites) {
    await axiosInstance.post(
      `/admin/courses/${courseId}/prerequisites`,
      {
        prerequisiteCourseId: prereqId
      }
    );
  }
}

      alert("ویرایش درس با موفقیت انجام شد!");
      navigate("/management");
    } else {
      alert("خطا در ویرایش درس!");
    }
  } catch (error) {
    console.error(error);
    alert("مشکل در ارتباط با سرور");
  }
};


  const handlelogin = () =>{ navigate('/login')}
  const handledashboard = () => navigate("/dashboard");
  const handlemanagecourse = () => navigate("/management");
  const handleaddnewcourse = () => navigate("/add-new-course");
  const handleLimitUnit = () => navigate("/limit");  


  return (
    <div className="container">
      <div className="frame">
        <div className="rectangle" />

        <div className="field-name-editcourse">
          <label className="label-name-editcourse">نام درس</label>
          <input
            type="text"
            name="name"
            className="ipt-name-editcourse"
            value={course.name}
            onChange={handleChange}
          />
        </div>

        <div className="field-code-editcourse">
          <label className="label-code-editcourse">کد درس</label>
          <input
            type="text"
            name="code"
            className="ipt-code-editcourse"
            value={course.code}
            onChange={handleChange}
          />
        </div>

        <div className="field-vahed-editcourse">
          <label className="label-vahed-editcourse">واحد</label>
          <input
            type="text"
            name="vahed"
            className="ipt-vahed-editcourse"
            value={course.vahed}
            onChange={handleChange}
          />
        </div>

        <div className="field-capacity-editcourse">
          <label className="label-capacity-editcourse">ظرفیت</label>
          <input
            type="text"
            name="capacity"
            className="ipt-capacity-editcourse"
            value={course.capacity}
            onChange={handleChange}
          />
        </div>

        <div className="field-teacher-editcourse">
          <label className="label-teacher-editcourse">نام استاد</label>
          <input
            type="text"
            name="teacher"
            className="ipt-teacher-editcourse"
            value={course.teacher}
            onChange={handleChange}
          />
        </div>

        <div className="field-time-editcourse">
          <label className="label-time-editcourse">زمان</label>
          <input
            type="text"
            name="time"
            className="ipt-time-editcourse"
            value={course.time}
            onChange={handleChange}
          />
        </div>

        <div className="field-place-editcourse">
          <label className="label-place-editcourse">مکان</label>
          <input
            type="text"
            name="place"
            className="ipt-place-editcourse"
            value={course.place}
            onChange={handleChange}
          />
        </div>

        <div className="field-examtime-editcourse">
          <label className="label-examtime-editcourse">تاریخ امتحان </label>
          <input
            type="date"
            name="examDate"
            className="ipt-examtime-editcourse"
            value={course.examDate}
            onChange={handleChange}
          />
        </div>

        <div className="field-description-editcourse">
          <label className="label-description-editcourse">
           پیش‌نیازها 
          </label>

          <input
            type="text"
            className="ipt-description-editcourse"
            placeholder="جستجو در دروس..."
            value={value}
            onFocus={() => setOpen(true)}
            onChange={(e) => {
            setValue(e.target.value);
            setOpen(true);
            }}
            onBlur={() => {
            setTimeout(() => setOpen(false), 200);
            }}
         />

          {open && (
            <div className="prereq-dropdown-editcourse">
            {allCourses
            .filter(c => c.title && c.title.includes(value))
            .map(c => (
            <label key={c.id} className="prereq-item-editcourse">
            <input
              type="checkbox"
              checked={selectedPrerequisites.includes(c.id)}
              onChange={() => {
                setSelectedPrerequisites(prev =>
                  prev.includes(c.id)
                    ? prev.filter(x => x !== c.id)
                    : [...prev, c.id]
                );
              }}
            />
            {c.title}
          </label>
        ))}
    </div>
          )}
</div>



        <div className="dashboard">
          <button className="btn_dashdoard-edit" onClick={handledashboard}>
            داشبورد
          </button>
          <div className="icon_doshboard">
            <MdDashboard className="icon" />
          </div>

          <button className="btn_manage_course" onClick={handlemanagecourse}>
            مدیریت دروس
          </button>
          <div className="icon_manage_course">
            <MdMenuBook className="icon" />
          </div>

          <button className="btn_limitunit_inedit_course" onClick={handleLimitUnit}>تعیین حد واحد</button>
            <div className="icon_limitunit">
              <FaBook className="icon" />
            </div>
        </div>

        <img className="shahid-chamran" alt="Shahid chamran" src={Logo} />

        <div className="box">
          <button className="btn-editcourse-edit" onClick={handleaddnewcourse}>
            افزودن درس جدید +
          </button>
        </div>

        <div className="rectangle-3" />

        <div className="date">{dateTime.toLocaleDateString("fa-IR")}</div>
        <div className="clock">{dateTime.toLocaleTimeString("fa-IR")}</div>

        <div className="search-container">
          <FaSearch className="search-icon" />
          <input
            type="text"
            placeholder="جستجو کنید..."
            value={searchValue}
            onChange={(e) => setSearchValue(e.target.value)}
            className="search-input"
          />
        </div>

        <button className="icon_exit_edit_course" onClick={handlelogin}>
          <FiLogOut className="exit_edit_course"/>     
        </button>

        <div className="course-edit-btn">
          <button className="course-edit" onClick={handleUpdate}>
            ثبت ویرایش
          </button>
        </div>
      </div>
    </div>
  );
};

export default EditCourse;