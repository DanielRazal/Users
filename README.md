Users Project
This project allows you to perform registration and login operations using a RESTful API. It also supports email functionality with SendGrid integration and enables file uploads to the wwwroot directory.

Server Overview
The server-side of the Users project includes the following features:

Registration and Login: Users can register and log in to the application using the provided API endpoints.
Email Integration: The project integrates with SendGrid to enable sending and receiving emails, such as notifications and password reset emails.
Database Storage: User data is stored and managed in a Microsoft SQL Server database using the DbContext.
CORS Configuration: Cross-Origin Resource Sharing (CORS) is configured to allow communication between the server and the client application.
Token Authentication: API endpoints and user sessions are secured using token authentication.
Server-side Validations: Data input validation and error handling are implemented on the server-side.
Data Transfer Objects (DTOs): DTOs are used for data validation and defining the contract between the client and server.
Environment and Configuration Files: Application settings and deployment configurations are managed using environment and configuration files.
Interfaces for Loose Coupling: Interfaces are utilized to define contracts and promote loose coupling between components.
Attribute-based Validation: Built-in attribute classes (e.g., Required, StringLength) are used for applying validation rules.
Static File Serving: The wwwroot folder is used to serve static files, including HTML, CSS, JavaScript, and images.
Client Overview
The client-side of the Users project includes the following features:

Authorization and Authentication: The client application utilizes AuthGuard and canActivate to enforce authorization and authentication for protected routes.
HTTP Requests: Angular's HttpClientModule is used for making HTTP requests from the client application to the server's APIs.
Alerts and Notifications: The Swal (SweetAlert) library is used to display user-friendly alerts and notifications in the client application.
