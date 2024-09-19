BookBazaar - Online Bookstore Platform.

-*Description*: BookBazaar is an online bookstore platform built using ASP.NET Core MVC architecture. The project is structured into several layers and areas to manage both administrative and customer-facing functionalities effectively. This repository contains the complete implementation of a multi-layered architecture following the repository and unit of work design patterns, ensuring maintainability, scalability, and separation of concerns.
*Technologies*:
- *ASP.NET Core MVC*: Framework for building the web application.
- *Entity Framework Core*: ORM for interacting with the database.
- *Razor Pages*: For creating dynamic, server-side web pages.
- *ASP.NET Core Identity*: For user management (registration, login, roles).
- *Stripe*: For managing Payments.
*Design Patterns Used*:
- *Repository Pattern*: This design pattern abstracts the data access logic, making the code more maintainable and testable.
- *Unit of Work Pattern*: This pattern coordinates the work of multiple repositories, ensuring that changes to multiple entities are managed in a single transaction.
*Project Architecture*:
1. *BookBazaar.DataAccess*:
   - This layer handles data access and the interaction with the database using Entity Framework Core.
   - *Key folders*:
     - *Data*: 
       - ApplicationDbContext.cs: Defines the DbContext for the project, acting as the link between the database and the application.
     - *DbInitializer*: 
       - DbInitializer.cs and IDbInitializer.cs: Responsible for database seeding and initialization during application startup.
     - *Repository*: 
       - Implements the repository pattern, separating data access logic from the rest of the application.
       - *Interfaces*: Includes interfaces like IBookRepository, IUnitOfWork, etc., that define the contract for database operations.
       - *Implementations*: Concrete classes like BookRepository, CategoryRepository, OrderRepository, etc., that implement these interfaces to manage CRUD operations for different entities like books, orders, users, etc.
2. *Areas*:
   - The application is divided into 2 key areas: Admin and Customer, each with its controllers and views.
   - The application contains another 2 areas: Company and Employee - Only Admins can control them
   
   - *Admin*:
     - Contains controllers to manage backend administrative tasks, such as managing books, categories, companies, orders, and users.
     - *Controllers*:
       - BookController.cs, CategoryController.cs, CompanyController.cs, OrderController.cs, UserController.cs: Manage CRUD operations for their respective entities.
     - *Views*: 
       - Each entity (Book, Category, Company, etc.) has a corresponding folder with Razor views for managing the UI.
   
   - *Customer*:
     - This area is responsible for customer-facing operations such as viewing books, managing their shopping cart, and chatting.
     - *Controllers*:
       - CartController.cs: Manages shopping cart functionalities.
       - HomeController.cs: Handles the homepage and general customer navigation.
     - *Views*:
       - Organized into folders (Cart, Home), each containing views (Razor pages) for rendering the customer interface.
       
   - *Identity*:
       - This area contains components for managing user authentication and authorization using ASP.NET Core Identity.
