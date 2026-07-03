# Barber Appointment System

[Türkçe README](README.tr.md)

Barber Appointment System is an ASP.NET Core MVC web application designed to manage barber salon operations such as customer records, employees, services, appointments, and role-based user sessions. The project uses Entity Framework Core with SQL Server LocalDB and includes separate interfaces for administrators, customers, and employees.

## Features

- **Role-based authentication** for Admin, Customer, and Employee users
- **Admin dashboard** with daily earnings and employee availability overview
- **Customer management**: list, add, edit, view, and delete customer records
- **Employee management**: list, add, edit, view, and delete employee records
- **Service management**: create, edit, delete, and assign services to employees
- **Appointment booking**: customers can choose an available employee, service, and appointment date
- **Employee availability tracking** after appointment creation
- **AI hairstyle preview**: customers can upload an image and test a randomly selected hairstyle through an external hairstyle API
- **ASP.NET Core MVC structure** with Controllers, Models, Views, Migrations, and static assets

## Tech Stack

- **C#**
- **ASP.NET Core MVC**
- **.NET 8**
- **Entity Framework Core**
- **SQL Server LocalDB**
- **Cookie Authentication**
- **Razor Views / HTML / CSS / JavaScript**
- **Swagger**
- **External Hairstyle API integration**

## Project Structure

```text
BarberAppointmentSystemMore/
├── Controllers/          # MVC controllers for admin, customer, employee, salon, services, appointments, and AI
├── Models/               # Entity models and DbContext
├── Views/                # Razor pages grouped by feature
├── Migrations/           # Entity Framework Core database migrations
├── wwwroot/              # Static files such as CSS, JS, and images
├── Program.cs            # Application startup and middleware configuration
├── appsettings.json      # Database connection and application settings
└── BarberAppointmentSystem.csproj
```

## Main Modules

### Admin Panel

The admin can manage customers, employees, and services. The dashboard also displays daily earnings and employee status information.

### Customer Panel

Customers can log in, view their panel, and book appointments by selecting an available employee, a service, and an appointment date.

### Employee Panel

Employees have their own role-protected area and can be managed through CRUD operations.

### Appointment System

The appointment module creates appointments with a selected customer, employee, service, date, and status. When an appointment is created, the selected employee availability is updated.

### AI Hairstyle Preview

The AI module allows customers to upload an image and preview a generated hairstyle result using an external RapidAPI hairstyle service. The uploaded image is validated by size and file type before the API request is sent.

## Getting Started

### Prerequisites

Make sure the following tools are installed:

- Visual Studio 2022 or newer
- .NET 8 SDK
- SQL Server LocalDB or SQL Server Express

### Installation

1. Clone the repository:

```bash
git clone https://github.com/dursunozer/BarberAppointmentSystemMore.git
cd BarberAppointmentSystemMore
```

2. Restore dependencies:

```bash
dotnet restore
```

3. Check the database connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=BarberAppointmentSystem;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

4. Apply database migrations:

```bash
dotnet ef database update
```

5. Run the project:

```bash
dotnet run
```

6. Open the application in your browser using the localhost URL shown in the terminal.

## AI API Configuration

The project includes an external hairstyle API client. Before using the AI hairstyle feature, replace the placeholder API key in `Program.cs` with your own RapidAPI key:

```csharp
client.DefaultRequestHeaders.Add("x-rapidapi-key", "Your-Api-Key");
```

For security, it is recommended to store API keys in user secrets or environment variables instead of hardcoding them in the source code.

## Possible Improvements

- Move API keys and sensitive settings to environment variables
- Add password hashing instead of storing plain text passwords
- Improve appointment conflict checking by validating time intervals
- Add unit and integration tests
- Add email or SMS appointment notifications
- Improve UI responsiveness for mobile devices
- Add role-based dashboards with more detailed analytics

## License

No license file is currently provided. You may add a license depending on how you want others to use or contribute to this project.
