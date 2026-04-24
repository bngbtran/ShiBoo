# 🐼 ShiBoo

![.NET](https://img.shields.io/badge/.NET-8-512BD4)
![WPF](https://img.shields.io/badge/WPF-Desktop-0C54C2)
![SQLite](https://img.shields.io/badge/SQLite-Database-003B57)
![EF Core](https://img.shields.io/badge/EF_Core-ORM-6C3483)
![License](https://img.shields.io/badge/license-MIT-green)
![Status](https://img.shields.io/badge/status-active-success)

ShiBoo là ứng dụng quản lý ca trực dành cho doanh nghiệp nhỏ hoặc nhóm làm việc.
Hệ thống cho phép quản lý nhân viên, phân ca, và xử lý các yêu cầu đổi ca một cách trực quan và nhanh chóng.

---

## 🚀 Demo

* Desktop App (WPF): chạy local bằng `.NET`
* Database: SQLite (file `ShiBoo.db`)

---

## ✨ Tính năng

### 🔐 Xác thực

* Đăng nhập bằng Email / Password
* Phân quyền: **Admin / Member**

---

### 👨‍💼 Quản trị viên (Admin)

**Quản lý nhân viên**

* Thêm nhân viên
* Xóa nhân viên
* Phân quyền (Admin / Member)

**Quản lý ca trực**

* Gán ca trực theo ngày
* Xem danh sách ca toàn hệ thống
* Chỉnh sửa ca trực

**Duyệt yêu cầu**

* Duyệt đổi ca
* Từ chối yêu cầu
* Xử lý request theo trạng thái

---

### 👤 Nhân viên (Member)

**Cá nhân**

* Xem lịch trực của mình
* Đổi mật khẩu lần đầu

**Ca trực**

* Đăng ký ca mới
* Gửi yêu cầu đổi ca
* Theo dõi trạng thái (Pending / Approved / Rejected)

---

## 🛠️ Công nghệ sử dụng

### Backend / Logic

* .NET (WPF)
* Entity Framework Core
* SQLite

### Kiến trúc

* MVVM (Model - View - ViewModel)

### UI

* XAML
* Custom ControlTemplate
* DataGrid

---

## 📂 Cấu trúc dự án

```id="q1x9l2"
ShiBoo
│
├── Data
│   └── ShiBooDbContext.cs
│
├── Models
│   ├── User.cs
│   └── Shift.cs
│
├── ViewModels
│   ├── LoginViewModel.cs
│   ├── UserManagementViewModel.cs
│   └── ...
│
├── Views
│   ├── LoginView.xaml
│   ├── Admin
│   │   ├── UserManagement.xaml
│   │   └── ShiftApproval.xaml
│   │
│   └── Member
│       ├── PersonalShift.xaml
│       └── ChangePassword.xaml
│
├── Helpers
│   ├── RelayCommand.cs
│   └── ShiftDisableConverter.cs
│
├── ShiBoo.db
└── seed.sql
```

---

## 🗄️ Quy trình dữ liệu

Ứng dụng sử dụng SQLite làm database local.

```id="m5tq2k"
seed.sql
    ↓
SQLite (ShiBoo.db)
    ↓
Entity Framework Core
    ↓
WPF UI (DataGrid / Binding)
```

---

## 📊 Cấu trúc dữ liệu

### User

```json id="2q3o1m"
{
  "Id": 1,
  "Name": "System Admin",
  "Email": "admin@shiboo.com",
  "Password": "admin",
  "Role": "Admin",
  "IsFirstLogin": false
}
```

---

### Shift

```json id="r3c91x"
{
  "Id": 1,
  "UserId": 2,
  "Date": "2026-04-25",
  "ShiftName": "Ca 1",
  "Note": "",
  "Status": "Approved",
  "RequestType": "Assign"
}
```

---

## ⚙️ Cài đặt

### 1. Clone project

```id="k4s0jd"
git clone https://github.com/bngbtran/ShiBoo.git
cd shiboo
```

---

### 2. Chạy ứng dụng

```id="z6k2x1"
dotnet run
```

---

## 🧪 Seed dữ liệu

Project sử dụng file `seed.sql` để tạo dữ liệu mẫu.

### 👉 Chạy trong PowerShell

```id="y1k9q8"
sqlite3 ShiBoo.db ".read seed.sql"
```

---

## 🔑 Tài khoản mặc định

**Admin**

```id="4n9p8d"
admin@shiboo.com
admin
```

**User**

```id="x9p2lq"
user1@shiboo.com
123456
```

---

## 🧠 Logic hệ thống

### Trạng thái ca

| Status   | Ý nghĩa    |
| -------- | ---------- |
| Pending  | Chờ duyệt  |
| Approved | Đã duyệt   |
| Rejected | Bị từ chối |

---

### Loại yêu cầu

| RequestType | Ý nghĩa    |
| ----------- | ---------- |
| Assign      | Gán ca     |
| Request     | Đăng ký ca |
| Change      | Đổi ca     |

---

## ⚠️ Lưu ý

* Password hiện đang lưu dạng **plain text** (chỉ phục vụ demo)
* Nên nâng cấp sang **hash (BCrypt)** nếu dùng thực tế
* SQLite phù hợp app nhỏ, không tối ưu cho hệ thống lớn

---

## 🤝 Đóng góp

Mọi đóng góp đều được hoan nghênh.

Bạn có thể:

* Cải thiện UI/UX
* Tối ưu code MVVM
* Thêm tính năng mới

### Các bước:

```id="d1p9c3"
1. Fork repository
2. Tạo branch mới
3. Commit thay đổi
4. Tạo Pull Request
```

---

## 📜 Giấy phép

MIT License

Bạn được phép sử dụng, chỉnh sửa và phân phối theo giấy phép MIT.