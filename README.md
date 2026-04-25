# 🛒 E-Commerce Platform API

> A robust, layered backend service for a scalable e-commerce storefront, designed with a decoupled HTTP API surface and domain-driven entity modeling to handle complex vendor and multi-order fulfillment.

This project is implemented as a monolithic, layered backend service with a clear separation between the API, business orchestration, and data access layers. The architecture emphasizes maintainability and extensibility through dependency injection, repository abstractions, and highly explicit Entity Framework Core relationship modeling.

## 🛠️ Technology Stack
* **Framework:** ASP.NET Core Web API (.NET 8)
* **Language:** C# 12.0
* **Database:** SQL Server
* **ORM:** Entity Framework Core 8
* **API Tooling:** Swagger / OpenAPI
* **Authentication:** ASP.NET Core Identity

## ⚙️ Core System Mechanics & Architecture

The system design prioritizes explicit responsibility boundaries and complex relational constraints:

* **Layered Architecture & Repository Pattern:** * Controllers are kept thin; they delegate execution to the Service Layer (e.g., `IOrderService`), which centralizes higher-level business workflows beyond standard CRUD operations.
  * Repositories (`IProductRepository`, `ICustomerRepository`) encapsulate data access, reducing controller-level query complexity and enforcing the Dependency Inversion Principle.
* **Advanced EF Core Domain Modeling:** * The schema is strictly controlled via fluent API configurations in `OnModelCreating`, explicitly managing composite keys and cascade behaviors.
  * **Constrained Delete Behavior:** `ProductOfOrder` to `Product` uses `DeleteBehavior.Restrict` to preserve historical order integrity, while `ProductOfOrder` to `SubOrder` uses `DeleteBehavior.Cascade` to ensure line items are removed if a sub-order is canceled.
* **TPC Inheritance Strategy:** * `ApplicationUser` utilizes a Table-per-Concrete-Type (TPC) mapping strategy. Customers and Vendors map to separate concrete tables, enabling role-specific data structures while preserving shared base authentication semantics.
* **Domain-Driven Order Decomposition:** * Orders are decomposed into `Order -> SubOrder -> ProductOfOrder` to support complex multi-vendor fulfillment under a single customer transaction.

## 💻 Running Locally

1. Clone the repository:
   ```bash
   git clone [https://github.com/MTM2025MT/Ecommerce-angular-project.git](https://github.com/MTM2025MT/Ecommerce-angular-project.git)
