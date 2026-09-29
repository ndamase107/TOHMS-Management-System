# TOHMS – Thahla Occuhealth Management System

## Information Systems 3E – Work Integrated Learning (INSY7315)

TOHMS (Thahla Occuhealth Management System) is a web-based occupational health management system developed for **Thahla Occuhealth**. The system was designed to digitise and centralise key occupational health administration processes including client management, employee management, medical appointments, occupational examinations, health records, medical certificates, workplace risk assessments, services and billing.

The project was developed as part of the **INSY7315 Information Systems 3E – Work Integrated Learning** module.

---

# 1. Project Overview

Thahla Occuhealth provides occupational health consulting, mobile and on-site medical services, medical surveillance and integrated risk-management services to organisations operating across different industries.

TOHMS provides a central system through which authorised users can manage the operational information associated with these services.

The main business workflow supported by the system is:

Client Company  
↓  
Employees  
↓  
Appointments  
↓  
Occupational Examinations  
↓  
Health Records  
↓  
Medical/Fitness Certificates

The system additionally supports:

Client Company  
↓  
Risk Assessments

and:

Client Company  
↓  
Services  
↓  
Invoices  
↓  
Invoice Items  
↓  
Billing Totals

The objective is to reduce reliance on fragmented/manual administrative processes while improving the organisation, accessibility and consistency of occupational health information.

---

# 2. Main System Features

TOHMS currently provides the following functionality:

- Secure user authentication
- Role-based authorisation
- Dashboard and operational statistics
- Client/company management
- Employee management
- Appointment management
- Occupational examination management
- Employee health-record management
- Medical/fitness certificate management
- Workplace risk-assessment management
- Occupational-health service management
- Invoice and billing management
- Automatic invoice calculations
- REST API functionality
- Database persistence
- Input validation
- Responsive web interface
- Automated testing
- Swagger API documentation
- Entity Framework Core database integration

---

# 3. Technology Stack

The application was developed using the following technologies:

| Technology | Purpose |
|---|---|
| C# | Primary programming language |
| ASP.NET Core MVC | Web application framework |
| Razor Views | User interface rendering |
| HTML5 | Page structure |
| CSS3 | Custom styling |
| Bootstrap | Responsive user-interface components |
| JavaScript | Client-side functionality |
| Entity Framework Core | Object-relational mapping and database access |
| SQL Server / LocalDB | Development database |
| ASP.NET Core Identity | Authentication and role management |
| REST API | Programmatic access to selected system resources |
| Swagger / OpenAPI | API documentation and testing |
| xUnit | Automated testing |
| Git | Source control |
| GitHub | Repository and collaboration platform |
| GitHub Actions | CI/CD automation |
| Microsoft Azure | Intended cloud hosting environment |

---

# 4. System Architecture

TOHMS follows an **ASP.NET Core MVC architecture** with additional repository and service abstractions where appropriate.

The major architectural layers are:

### Presentation Layer

The presentation layer consists of:

- Razor Views
- Bootstrap
- CSS
- JavaScript
- MVC Controllers

This layer is responsible for displaying information to users, receiving user input and providing navigation through the application.

### Business Logic Layer

Business logic is implemented through controllers and service classes.

Examples include:

- `ClientService`
- `DashboardService`
- Billing calculations
- Client deletion rules
- Appointment processing
- Certificate status handling

Services help separate business rules from presentation logic.

### Repository Layer

Repository abstractions are used for selected entities, particularly clients and employees.

Examples include:

- `IRepository<T>`
- `Repository<T>`
- `IClientRepository`
- `ClientRepository`
- `IEmployeeRepository`
- `EmployeeRepository`

The repository layer provides an abstraction between application logic and Entity Framework Core.

### Data Access Layer

Entity Framework Core is responsible for communication with SQL Server.

The central database context is:

`ApplicationDbContext`

It contains the application's business entities as well as ASP.NET Core Identity tables.

---

# 5. Core Domain Models

The primary TOHMS domain models include:

### Client

Represents a company receiving occupational-health services from Thahla Occuhealth.

A client can have:

- Multiple employees
- Multiple workplace risks
- Multiple invoices

### Employee

Represents an employee belonging to a client company.

An employee can have:

- Multiple appointments
- Multiple examinations
- Multiple health records
- Multiple certificates

### Appointment

Represents a scheduled occupational-health appointment for an employee.

Information includes:

- Appointment type
- Appointment date
- Status
- Notes

### Examination

Represents an occupational medical examination.

Information includes:

- Examination type
- Examination date
- Fitness status
- Findings
- Recommendations

### Health Record

Stores occupational-health information associated with an employee.

### Certificate

Represents medical or fitness certificates issued following occupational-health processes.

### Risk

Represents an occupational/workplace hazard or risk associated with a client company.

Information includes:

- Hazard
- Description
- Risk level
- Control measures
- Assessment date
- Status

### Service

Represents a service offered by Thahla Occuhealth.

Each service can have a defined price and can be added to invoices.

### Invoice

Represents billing information associated with a client.

Invoices contain one or more invoice items.

### Invoice Item

Represents a service charged on an invoice.

The system calculates:

`Total Price = Quantity × Unit Price`

The invoice total is recalculated from its associated invoice items.

---

# 6. Database Relationships

Important database relationships include:

- One Client → Many Employees
- One Client → Many Risks
- One Client → Many Invoices
- One Employee → Many Appointments
- One Employee → Many Examinations
- One Employee → Many Health Records
- One Employee → Many Certificates
- One Invoice → Many Invoice Items
- One Service → Many Invoice Items

Entity Framework Core manages these relationships through foreign keys and navigation properties.

Decimal values used for service prices and billing amounts are configured with appropriate SQL precision.

---

# 7. Authentication and Security

TOHMS uses **ASP.NET Core Identity** for authentication and user management.

Users must authenticate before accessing protected parts of the system.

The application supports the following roles:

- Administrator
- Manager
- Occupational Health Practitioner
- Reception
- Client Representative

Role-based authorisation is used to restrict sensitive functionality.

For example:

- Billing functionality is restricted to authorised management roles.
- Occupational examinations and health records can be restricted to occupational-health practitioners and administrators.
- Risk-management functionality is available to appropriate operational and management roles.
- Destructive API operations are restricted to higher-authority roles.

Password policies include requirements for:

- Minimum password length
- Uppercase characters
- Lowercase characters
- Numbers
- Special characters

Account lockout protection is also configured for repeated unsuccessful login attempts.

Passwords are handled through ASP.NET Core Identity and are not stored as plain-text passwords in the database.

---

# 8. Role-Based Access Control

TOHMS implements authorisation using ASP.NET Core `[Authorize]` attributes.

Examples include:

```csharp
[Authorize]
for functionality requiring any authenticated user, and:

[Authorize(Roles = "Administrator,Manager")]
for functionality restricted to specific roles.

This follows the principle of limiting sensitive functionality according to the responsibilities of the authenticated user.

9. Dashboard
The TOHMS dashboard provides users with a central operational overview.

Dashboard information includes:

Total clients

Total employees

Total appointments

Total certificates

Total risks

Total invoices

Upcoming appointments

Pending examinations

Active certificates

Open risks

Outstanding invoices

The dashboard obtains current information from the database through DashboardService.

This provides users with immediate visibility into important operational information.

10. Client Management
The Client Management module allows authorised users to:

Create clients

View clients

Edit client information

View client details

Delete eligible clients

Monitor active/inactive status

Validation is applied to fields such as:

Company name

Email address

Phone number

Contact information

Clients form a central part of the TOHMS data model because employees, risks and invoices are associated with client companies.

11. Employee Management
Employees are associated with client companies.

The Employee Management module stores information such as:

Employee number

First name

Last name

ID number

Job title

Department

Contact information

Date of birth

Employment date

Active status

Employee information connects the client-management component to occupational-health functionality.

12. Appointment Management
Appointments allow occupational-health activities to be scheduled for employees.

Appointments contain:

Employee

Appointment type

Appointment date

Status

Notes

Appointment information contributes to the operational workflow and dashboard statistics.

13. Occupational Examinations
The Examination module records occupational-health examinations performed for employees.

Information includes:

Employee

Examination type

Examination date

Fitness status

Findings

Recommendations

Fitness status can be used to track examinations that are still pending or have been completed.

14. Health Records
The Health Records module stores occupational-health information associated with employees.

Records contain information such as:

Employee

Record date

Record type

Notes

Recommendations

Access to health-related functionality is protected using application authentication and role-based authorisation.

15. Certificates
The Certificate module manages occupational-health and fitness certificates.

Certificate information includes:

Certificate number

Certificate type

Employee

Issue date

Expiry date

Status

The dashboard can identify active certificates using certificate status and expiry information.

16. Risk Assessments
TOHMS allows occupational risks to be recorded against client organisations.

A risk assessment can include:

Hazard

Description

Risk level

Control measures

Assessment date

Active status

This supports Thahla Occuhealth's broader occupational risk-management activities.

17. Services
The Services module stores occupational-health services provided by the organisation.

A service contains:

Service name

Description

Price

Active status

Services can subsequently be added to invoices.

18. Billing and Invoices
The Billing module allows authorised users to create and manage client invoices.

An invoice contains:

Client

Invoice number

Invoice date

Due date

Status

Total amount

Services can be added to an invoice through invoice items.

For every invoice item:

TotalPrice = Quantity × UnitPrice

The system recalculates the invoice total after invoice items are added or removed.

This reduces manual billing calculations and ensures invoice totals correspond with the services recorded against the invoice.

19. REST API
TOHMS includes REST API functionality for client information.

The Clients API is available through:

/api/clients

Supported operations include:

Method	Endpoint	Purpose
GET	/api/clients	Retrieve clients
GET	/api/clients/{id}	Retrieve a specific client
POST	/api/clients	Create a client
PUT	/api/clients/{id}	Update a client
DELETE	/api/clients/{id}	Delete a client
The API implements:

Authentication

Role-based authorisation

Model validation

Database integration

Appropriate HTTP status codes

Asynchronous database operations

Responses can include HTTP status codes such as:

200 OK

201 Created

204 No Content

400 Bad Request

401 Unauthorized

403 Forbidden

404 Not Found

409 Conflict

Deletion is protected by business rules that prevent a client with related employee or invoice information from being removed.

20. Swagger / OpenAPI
Swagger is included during development to provide API documentation and endpoint visibility.

When running in the Development environment, Swagger can be accessed at:

/swagger

Swagger provides an interface for inspecting available API endpoints and their request/response structures.

21. Input Validation
TOHMS uses ASP.NET Core model validation and Data Annotation attributes.

Examples include:

[Required]
[StringLength(200)]
[EmailAddress]
[Phone]
[Range(1, 100000)]
Validation helps prevent invalid or incomplete data from entering the application.

Both server-side validation and appropriate user-interface validation feedback are used.

22. User Interface and Responsive Design
The frontend was designed to provide:

Consistent navigation

Clear visual hierarchy

Responsive layouts

Accessible forms

Appropriate labels

Visible feedback

Consistent typography

Consistent branding

Mobile/tablet compatibility

Responsive tables

Bootstrap navigation

Dashboard cards

Clear action buttons

The system is designed to remain usable across desktop, tablet and smaller screen sizes.

23. Accessibility
Accessibility considerations include:

Semantic HTML

Form labels

Keyboard-accessible controls

Appropriate button descriptions

Visible focus states

Responsive layouts

Readable typography

Clear status indicators

Appropriate use of headings

These features improve usability for a broader range of users.

24. Automated Testing
TOHMS includes an automated test project:

TOHMS.Tests

Automated testing is implemented using xUnit.

The current automated test suite contains 15 tests.

At the time of the completed local test run:

15 Passed

0 Failed

Automated tests help verify application behaviour and reduce the risk of regressions when code changes are introduced.

Testing forms part of the project's Continuous Integration strategy.

25. Manual Testing
In addition to automated testing, TOHMS is manually tested through the complete operational workflow.

Manual testing includes:

Authentication

Dashboard navigation

Client creation

Employee creation

Appointment creation

Examination recording

Health-record creation

Certificate creation

Risk assessment

Service creation

Invoice creation

Invoice-item creation

Invoice calculations

Input validation

Responsive layout testing

Logout and access-control testing

Manual testing is particularly important for verifying Razor Views, navigation, styling and end-to-end workflows that are not fully covered by isolated automated tests.

26. Git Branching Strategy
The project uses Git and GitHub for source control.

The intended branch structure is:

feature/*
↓
development
↓
main
↓
release

Feature Branches
Individual functionality can be developed in branches such as:

feature/client-management

feature/employee-management

feature/billing

feature/authentication

feature/api

Development
The development branch integrates completed development work.

Main
The main branch represents stable application code.

Release
The release branch represents the submission/deployment-ready version of TOHMS.

27. Continuous Integration and Continuous Deployment
GitHub Actions is used to support CI/CD.

The CI process is intended to automatically:

Check out repository code.

Restore .NET dependencies.

Build the application.

Build the test project.

Execute automated tests.

Prevent unnoticed test failures from progressing through the development process.

The deployment workflow is designed to deploy approved application versions to the configured hosting environment.

This reduces manual deployment steps and provides a repeatable software-delivery process.

28. Cloud Hosting
Microsoft Azure is the intended cloud-hosting platform for TOHMS.

The deployment architecture is designed around:

ASP.NET Core web application

Azure App Service

SQL Server / Azure SQL

Environment-specific configuration

Secure connection strings

GitHub-based deployment automation

Local development currently uses SQL Server LocalDB.

Production credentials and secrets must be configured using secure environment/application configuration and must not be stored directly in the public source code.

29. Security Considerations
TOHMS processes occupational-health and business information and therefore includes multiple security controls.

Implemented security measures include:

Authentication

ASP.NET Core Identity

Password hashing through Identity

Role-based authorisation

Account lockout configuration

Model validation

Restricted controller actions

Restricted API operations

Database relationships and constraints

Server-side processing

Protected application routes

Sensitive production credentials, API secrets, passwords and production database connection strings should never be committed to GitHub.

30. Project Structure
A simplified project structure is shown below:

TOHMS
│
├── API
│   └── Controllers
│       └── ClientsApiController.cs
│
├── Controllers
│   ├── AccountController.cs
│   ├── AppointmentsController.cs
│   ├── BillingController.cs
│   ├── CertificatesController.cs
│   ├── ClientsController.cs
│   ├── EmployeesController.cs
│   ├── ExaminationsController.cs
│   ├── HealthRecordsController.cs
│   ├── HomeController.cs
│   ├── RisksController.cs
│   └── ServicesController.cs
│
├── Data
│   ├── ApplicationDbContext.cs
│   └── DbInitializer.cs
│
├── Models
│   ├── ApplicationUser.cs
│   ├── Appointment.cs
│   ├── Certificate.cs
│   ├── Client.cs
│   ├── Employee.cs
│   ├── Examination.cs
│   ├── HealthRecord.cs
│   ├── Invoice.cs
│   ├── InvoiceItem.cs
│   ├── Risk.cs
│   └── Service.cs
│
├── Repositories
│   ├── Interfaces
│   ├── ClientRepository.cs
│   ├── EmployeeRepository.cs
│   └── Repository.cs
│
├── Services
│   ├── Interfaces
│   ├── ClientService.cs
│   └── DashboardService.cs
│
├── ViewModels
│   ├── AppointmentViewModel.cs
│   ├── ClientViewModel.cs
│   ├── DashboardViewModel.cs
│   └── LoginViewModel.cs
│
├── Views
│   ├── Account
│   ├── Appointments
│   ├── Billing
│   ├── Certificates
│   ├── Clients
│   ├── Employees
│   ├── Examinations
│   ├── HealthRecords
│   ├── Home
│   ├── Risks
│   ├── Services
│   └── Shared
│
├── wwwroot
│   ├── css
│   ├── images
│   └── js
│
├── appsettings.json
└── Program.cs

TOHMS.Tests
└── Automated test classes
31. Running the Application Locally
Prerequisites
Ensure the development machine has:

Visual Studio

.NET SDK compatible with the project

SQL Server LocalDB / SQL Server

Git

A modern web browser

Setup
Step 1
Clone the repository.

Step 2
Open the TOHMS solution in Visual Studio.

Step 3
Restore NuGet packages.

Step 4
Configure the development database connection string.

Example local configuration:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TOHMSDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
Step 5
Apply Entity Framework Core migrations.

Using Package Manager Console:

Update-Database
Step 6
Build the solution.

Step 7
Run the application through Visual Studio.

32. Database Migrations
Entity Framework Core migrations are used to manage the database schema.

Migrations allow database changes to be tracked alongside application development.

Typical commands include:

Add-Migration MigrationName
and:

Update-Database
Migration files should be retained in source control so the required database structure can be reproduced.

33. Testing the Project
Automated tests can be executed through Visual Studio:

Test → Test Explorer → Run All

Alternatively, tests can be executed using the .NET CLI:

dotnet test
A successful test run should report the automated test suite without failed tests.

34. API Testing
API functionality can be inspected through Swagger during development.

Start the application and navigate to:

/swagger

The Clients API can also be tested using tools such as Postman where appropriate authentication is configured.

35. Non-Functional Requirements
The project addresses important non-functional requirements including:

Security
Authentication and role-based authorisation protect application functionality.

Usability
Navigation, consistent layouts and form design improve user interaction.

Maintainability
MVC separation, services, repositories and Entity Framework Core improve organisation of the codebase.

Reliability
Validation, database constraints and automated testing reduce common application errors.

Performance
Asynchronous database operations and targeted Entity Framework queries are used where appropriate.

Scalability
The ASP.NET Core architecture and planned Azure deployment provide a foundation for future expansion.

Accessibility
Responsive design, semantic markup and accessible controls improve usability across devices and interaction methods.

36. Technical Decisions
ASP.NET Core MVC was selected because it provides clear separation between models, views and controllers while integrating effectively with C#, Entity Framework Core, Identity and SQL Server.

Entity Framework Core was selected to simplify database access and relationship management while supporting LINQ and migrations.

ASP.NET Core Identity was selected rather than developing a custom authentication system because it provides established mechanisms for password hashing, account management, lockout policies and role-based security.

SQL Server was selected because of its integration with the Microsoft development ecosystem and Entity Framework Core.

GitHub and GitHub Actions were selected to provide version control and automated software-delivery processes.

Azure was selected as the intended hosting platform because it integrates with ASP.NET Core, SQL Server and GitHub-based deployment workflows.

37. Development Challenges
Important implementation challenges included:

Designing relationships between occupational-health entities

Implementing authentication after the initial database had already been created

Maintaining compatible Entity Framework Core package versions

Implementing role-based access restrictions

Managing invoice relationships and automatic calculations

Integrating automated tests

Configuring local HTTPS within the development environment

Maintaining correct MVC Razor View folder conventions

Preparing the application for cloud deployment and CI/CD

These challenges were addressed through incremental implementation, debugging, testing and architectural refinement.

38. Future Improvements
Potential future improvements include:

Expanded APIs for additional modules

Dedicated API DTOs

Additional automated integration testing

Email notifications

Appointment reminders

Certificate-expiry notifications

Advanced occupational-health reporting

PDF certificate generation

PDF invoice generation

Audit logging

Multi-factor authentication

Advanced dashboard charts

Search and filtering

Data export functionality

Client self-service portal

Practitioner-specific dashboards

Improved cloud monitoring

Automated database backup strategy

39. Important Security Notice
Development credentials must only be used for local testing.

Before a production deployment:

Remove hard-coded passwords.

Store secrets securely.

Configure production connection strings securely.

Enable HTTPS.

Protect production database credentials.

Review role assignments.

Disable unnecessary development functionality.

Restrict Swagger where appropriate.

Review access to occupational-health information.

No production secrets should be committed to the GitHub repository.

40. Conclusion
TOHMS provides Thahla Occuhealth with a central web-based platform for managing occupational-health operations.

The application integrates client management, employee records, appointments, examinations, health records, certificates, risk assessments, occupational-health services and billing into a single system.

The solution demonstrates frontend development, backend development, relational database integration, authentication, authorisation, API development, automated testing, source control, CI/CD concepts and cloud-deployment preparation.

The system was developed with a focus on usability, security, maintainability and alignment with the operational requirements of Thahla Occuhealth.

Project
TOHMS – Thahla Occuhealth Management System

Module: INSY7315 – Information Systems 3E (Work Integrated Learning)

Academic Year: 2026

Client: Thahla Occuhealth