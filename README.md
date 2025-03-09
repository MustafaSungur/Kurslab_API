# **KursLab**

## Introduction
Welcome to the **EducationAPI** project! This project serves as an interactive educational platform where users can enroll in courses, watch educational content, rate courses, comment on lessons, and like comments. Additionally, users can become instructors, upload their own courses, and manage their content.

## Project Overview
EducationAPI is a **RESTful API** developed using **.NET 8**, designed to support a scalable and secure education platform. It allows users to:
- Register and log in to access educational content.
- Watch and interact with courses by rating, commenting, and liking comments.
- Become instructors and upload their own educational content.
- Manage their courses through an instructor panel, where they can edit, delete, and view statistics on their courses.
- Administrators can manage users, comments, ratings, and oversee all courses and statistics through an admin panel.

## Technologies Used
This project leverages modern technologies for performance, security, and maintainability:

- **.NET 8** - Latest version of the .NET framework for high-performance APIs.
- **Layered Architecture** - Separation of concerns for scalability and maintainability.
- **Entity Framework Core** - ORM for database interactions.
- **Identity Framework** - User authentication and authorization.
- **JWT (JSON Web Tokens)** - Secure user authentication.
- **PostgreSQL** - Relational database for storing content and user data.
- **AutoMapper** - Simplifies object-to-object mapping.
- **Swagger** - API documentation and testing.

## Project Architecture
The **EducationAPI** project follows a **layered architecture**, ensuring separation of concerns and scalability:

```
educationAPI/
├── Education.WebApi/         # API layer handling HTTP requests
│   ├── Controllers/          # Controllers managing endpoints
│   ├── wwwroot/              # Static files
│   ├── appsettings.json      # Configuration file
│   ├── Program.cs            # Main entry point of the API
│   ├── DbInitializer.cs      # Database initialization
│   ├── SwaggerFileOperationFilter.cs # Swagger customization
│
├── Education.Entity/         # Domain models and DTOs
│   ├── DTOs/                 # Data Transfer Objects
│   ├── Enums/                # Enumeration types
│   ├── Models/               # Entity models for the database
│
├── Education.Business/       # Business logic layer
│   ├── Core/                 # Core services (token handling, validation, video processing)
│   ├── Services/             # Service interfaces & implementations
│   ├── Exceptions/           # Error handling and Response
│   ├── MappingProfiles/      # AutoMapper configurations
│
├── Education.Data/           # Data access layer
│   ├── Repositories/         # Repository pattern implementation
│   ├── Migrations/           # Database migrations
│   ├── AppDbContext.cs       # EF Core database context
```

### Key Components
- **Web API Layer (Education.WebApi):**
  - Hosts controllers handling HTTP requests.
  - Configures Swagger documentation.
  - Handles dependency injection.

- **Entity Layer (Education.Entity):**
  - Defines DTOs for data transfer.
  - Contains models representing database tables.
  - Includes enums for structured categorization.

- **Business Layer (Education.Business):**
  - Implements business logic using service interfaces.
  - Handles authentication, course management, and user interactions.
  - Maps DTOs to entities using AutoMapper.

- **Data Layer (Education.Data):**
  - Implements the repository pattern for data access.
  - Manages database operations via Entity Framework Core.
  - Handles database migrations using PostgreSQL.

## User Roles & Permissions
### **Regular Users**
- Register and log in.
- Watch educational content.
- Rate courses.
- Comment on courses.
- Like other users' comments.
- Enroll in courses.

### **Instructors**
- Upload their own educational content.
- Edit and delete their courses.
- View course statistics (e.g., engagement, ratings, comments).

### **Administrators**
- Manage all users (view, delete accounts).
- View and manage all comments.
- View and manage all ratings.
- View all educational content statistics.
- Delete any course, comment, or rating.

## API Endpoints
The API exposes various endpoints for authentication, content management, and interactions.

### **Authentication Endpoints**
| HTTP Method | Endpoint | Purpose |
|------------|----------|----------|
| POST | `/api/Auth/Login` | Logs in a user and returns a JWT token |
| POST | `/api/Auth/ForgetPassword` | Sends a password reset email |
| POST | `/api/Auth/ResetPassword` | Resets the password using a token |

### **User Endpoints**
| HTTP Method | Endpoint | Purpose |
|------------|----------|----------|
| POST | `/api/ApplicationUser/Create` | Creates a new user |
| GET | `/api/ApplicationUser/{id}` | Retrieves user details |
| PUT | `/api/ApplicationUser/Update/{id}` | Updates user profile |
| DELETE | `/api/ApplicationUser/Delete/{id}` | Deletes a user (Admin only) |
| GET | `/api/ApplicationUser/Analysis` | Retrieves user statistics (Admin only) |

### **Content Management**
| HTTP Method | Endpoint | Purpose |
|------------|----------|----------|
| POST | `/api/Content/Create` | Creates new content (Instructor only) |
| GET | `/api/Content/{id}` | Retrieves content by ID |
| PUT | `/api/Content/Update/{id}` | Updates content details (Instructor only) |
| DELETE | `/api/Content/{id}` | Deletes content (Instructor/Admin only) |
| POST | `/api/Content/FilterContents` | Retrieves filtered content |
| GET | `/api/Content/GetTopContents` | Retrieves top-rated content |
| GET | `/api/Content/Statistics` | Retrieves course statistics (Instructor/Admin only) |

## Conclusion
EducationAPI provides a flexible and scalable solution for educational content management. Users can watch and interact with educational content, instructors can create and manage courses, and administrators have full control over the platform. 🚀
