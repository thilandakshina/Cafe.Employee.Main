All necessary endpoints have been added.

![image](https://github.com/user-attachments/assets/cda7e9c9-9a88-4eef-939d-df5710a7f58a)

1. Navigate to the Project Directory -> CafeManagement
2. Open CafeManagement.sln using visual studio 
3. Set Cafe.API as starup project.

CafeManagement System

-- Architecture Overview --
The solution is structured into multiple projects following clean architecture:

![image](https://github.com/user-attachments/assets/a6a9d9b0-347a-44a3-a22f-bf3ea57d3fd7)


Cafe.API: Web API project handling HTTP requests
Cafe.Business: Core business logic, CQRS implementation
Cafe.Data: Data access layer with Entity Framework Core
Cafe.Business.Test: Unit tests for business logic

Key Design Patterns & Principles

CQRS (Command Query Responsibility Segregation): Separates read and write operations
Mediator Pattern: Using MediatR for handling commands and queries
Repository Pattern: Abstract data access layer
Clean Architecture: Separation of concerns with clear dependencies
Unit Testing: Using NUnit and Moq for business logic validation

-- Project Structure --

1. Cafe.Business

Commands: Write operations (Create, Update, Delete)

Cafe Commands (Create, Update, Delete)
Employee Commands (Create, Update, Delete)

Queries: Read operations

Cafe Queries
Employee Queries

Handlers: Command and Query handlers
Validators: Command validation logic
DTOs: Data Transfer Objects
MappingProfiles: AutoMapper configurations

2. Cafe.Data

Entities: Domain models

CafeEntity
EmployeeEntity


Context: DbContext and database configuration
Repositories: Data access implementation
Migrations: Database schema versions

Database

Using SQLite as per requirements
Database file: cafe.db
Migrations handled through Entity Framework Core

3. Cafe.API

Controllers: REST API endpoints
Middleware: Request/Response pipeline components
Configuration: Application settings

4. Testing

NUnit test framework
Moq for mocking dependencies
Focus on business logic testing
Command handler tests
Query handler tests





# Cafe.Employee.FE

Navigate to the Project Directory -> cafe-employee-manager

1. Install the Dependencies - npm install
2. Run the React Application - npm start


Main Page
![image](https://github.com/user-attachments/assets/2cd8dd03-7f1d-4c27-837f-9cf1ee6c3725)

Add New
![image](https://github.com/user-attachments/assets/2b491acc-2cef-4a3b-a20d-20e369f43438)

Edit
![image](https://github.com/user-attachments/assets/1857c5bf-fee9-4415-9159-83d5296b9787)


Employees Page
![image](https://github.com/user-attachments/assets/053f8230-10d4-4be2-910c-2a994124dffc)


Add New  ( when you edit only the cafe assignment is working now and while adding you get an error. but It is already saved. you can go to the list page and see.)
![image](https://github.com/user-attachments/assets/781b84a4-fa58-4217-8750-2b10c0de983c)

Edit ( when you edit only the cafe assignment is working now)
![image](https://github.com/user-attachments/assets/2e8dd3af-32bd-4486-a33a-696611b9cd59)


Filter

![image](https://github.com/user-attachments/assets/03d7ae25-944b-4058-8766-7db9f10e7cb5)
![image](https://github.com/user-attachments/assets/26a71106-1491-42bd-a76d-0400efc6cf65)






**Please note that this solution is not fully completed.
More than 75% of the work on both the front and backend is completed.

