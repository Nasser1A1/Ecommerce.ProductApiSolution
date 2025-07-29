# 🛍️ Product Microservice

This is the **Product Microservice** for a microservices-based e-commerce system.  
It is responsible for **CRUD operations** on products and exposes RESTful endpoints to manage them.

---

## ✅ Responsibilities

- Create, read, update, and delete product data
- Expose endpoints to be consumed by the API Gateway
- Enforce business rules related to products

---

## 📦 Features

- 🔧 ASP.NET Core Web API
- 🗃️ Entity Framework Core with PostgreSQL or SQL Server
- 🧱 Clean architecture (separation of concerns)
- 🧪 Unit of Work & Repository Patterns
- ✅ DTOs, Mapping, Validation
- 🔐 JWT Authentication and Role-Based Authorization (when integrated via API Gateway)

---

## 🧩 Project Dependencies

> ⚠️ To run this project, you must also clone and reference the **Shared Library Project**:

🔗 [SharedLibrarySolution on GitHub](https://github.com/Nasser1A1/SharedLibrarySolution)

This shared library contains essential base classes, middleware, extensions, response wrappers, and possibly domain models used across all services.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- PostgreSQL or SQL Server database
- Reference to the [SharedLibrarySolution](https://github.com/Nasser1A1/SharedLibrarySolution)

### Setup Instructions

1. **Clone this repository** and the shared library:
   ```bash
   git clone https://github.com/<your-org>/ProductMicroservice.git
   git clone https://github.com/Nasser1A1/SharedLibrarySolution.git
   ```

2. **Reference the shared library project** in your `ProductApi.csproj`:
   ```xml
   <ProjectReference Include="..\SharedLibrarySolution\eCommerce.SharedLib\eCommerce.SharedLib.csproj" />
   ```

3. **Apply EF Migrations** and update the database:
   ```bash
   dotnet ef database update
   ```

4. **Run the service**:
   ```bash
   dotnet run
   ```

---

## 📁 Project Structure

```
ProductApi/
├── Application/
│   ├── Interfaces/
│   └── DTOs/
├── Domain/
│   └── Entities/
├── Infrastructure/
│   └── Repositories/
├── Presentation/
│   └── Controllers/
├── ProductApi.csproj
└── Program.cs
```

---

## 🔒 Security

- Handles authorization via **API Gateway**
- Expects a valid **JWT token** in protected routes (configured at the gateway)

---

## 🧪 Testing

- Includes unit testing for application services
- Integration testing support for controller endpoints

---

## 🤝 Contribution

Feel free to fork, raise issues, or submit pull requests!

---

## 📄 License

MIT License

---

## 👨‍💻 Author

Developed by [Mahmoud Mady](https://github.com/MahmoudMady)
