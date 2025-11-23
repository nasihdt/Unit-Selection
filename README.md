University Registration API – Sprint 1

این پروژه یک Web API برای سیستم انتخاب واحد دانشگاه است که در اسپرینت ۱ فقط روی بخش مدیر (Admin) و CRUD درس‌ها (Course) تمرکز دارد.
این ریپازیتوری مربوط به Backend Developer #1 است (زیرساخت، دیتابیس، مدل‌ها، Repository).

تکنولوژی‌های مورد استفاده:
- ASP.NET Core 10.0
- Entity Framework Core 9.0.1
- PostgreSQL
- Npgsql EF Core Provider
- EF Core Migrations
- Swagger / OpenAPI

ساختار پروژه:
UniversityRegistration.Api
 - Controllers/
 - Data/
     AppDbContext.cs
 - Models/
     Admin.cs
     Course.cs
 - Repository/
     Interfaces/
         IAdminRepository.cs
         ICourseRepository.cs
     Implementations/
         AdminRepository.cs
         CourseRepository.cs
 - Migrations/
 - appsettings.json
 - Program.cs

مدل‌ها (Entities):
Admin:
    Id
    Username
    Password
    Role

Course:
    Id
    Title
    Code
    Units
    Capacity
    TeacherName
    Time
    Location
    Description (اختیاری)

دیتابیس و Seed اولیه:
Connection String:
Host=localhost
Port=5432
Database=UniversityRegistrationDb
Username=postgres
Password=YOUR_PASSWORD

Admin اولیه:
Username: admin
Password: Admin@123
Role: Admin

Courses اولیه:
- برنامه‌نویسی ۱ — CS101
- پایگاه داده — CS202

Repository Layer:
IAdminRepository:
    GetByUsernameAsync
    GetByUsernameAndPasswordAsync

ICourseRepository:
    GetAllAsync
    GetByIdAsync
    FindByCodeAsync
    AddAsync
    UpdateAsync
    DeleteAsync

تنظیمات Program.cs:
- ثبت DbContext با Npgsql
- ثبت Repositoryها به صورت Scoped
- فعال‌سازی Swagger

نحوه اجرای پروژه:
۱. نصب .NET 10 و PostgreSQL
۲. اجرای مایگریشن‌ها:
   dotnet ef database update
۳. اجرای پروژه:
   dotnet run
۴. باز کردن Swagger:
   https://localhost:<port>/swagger

وضعیت اسپرینت ۱:
مدل‌ها: انجام شد
DbContext: انجام شد
Seed: انجام شد
Repository Layer: انجام شد
Swagger و DI: انجام شد
Migration: انجام شد
پروژه آماده ادامه برای Backend Developer #2 است.
