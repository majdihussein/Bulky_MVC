# BulkyWeb

A complete e-commerce web application built using ASP.NET Core MVC with clean architecture, layered structure, and admin panel. This project demonstrates real-world implementation of a product catalog, shopping cart, user roles, and more.

---

## 📁 Project Structure

```
BulkyWeb.sln
├── BulkyWeb           → Main Web MVC project (UI Layer)
├── Bulky.DataAccess   → Handles database context, repositories, and migrations
├── Bulky.Models       → Contains all models/entities
├── Bulky.Utility      → Static details, constants, helpers
├── BulkyWeb.Tests     → Unit tests (optional)
```

---

## 🚀 Features

- Admin dashboard (CRUD for categories/products/companies)
- Role-based access: Admin, User
- Shopping cart & order management
- Identity & Authentication using ASP.NET Identity
- Payment integration (Stripe)
- Clean and maintainable codebase

---

## 🛠️ Technologies Used

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server
- Bootstrap 5
- Identity Framework
- Stripe Payment Gateway

---

## 🔧 Getting Started

### Prerequisites

- [.NET SDK 8](https://dotnet.microsoft.com/)
- SQL Server
- Visual Studio 2022+

### Setup Steps

1. **Clone the repository**

   ```bash
   git clone https://github.com/majdihussein/Bulky_MVC
   ```

2. **Apply Migrations**

   Open `Package Manager Console` and select `Bulky.DataAccess` as default project:

   ```bash
   update-database
   ```

3. **Run the project**

   Press `F5` or run via terminal:

   ```bash
   dotnet run --project BulkyWeb
   ```

---


## 📂 Folder Highlights

- `wwwroot/Images` → Contains all uploaded product images.
- `Controllers/Admin` → Contains admin area controllers.
- `Views/Shared` → Shared layouts and partials.

---

## 📦 Packages Used

- `Microsoft.EntityFrameworkCore`
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `Stripe.net`
- `AutoMapper`
- `Serilog`

---

## ✍️ Author

**Majdi Hussein**  
📧 majdi.hussein94@hotmail.com  
🔗 [LinkedIn](https://www.linkedin.com/in/eng-majdi-hussein-7801ab321)  
🔗 [GitHub](https://github.com/majdihussein)


