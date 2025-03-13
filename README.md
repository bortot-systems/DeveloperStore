# Developer Evaluation Project

`READ CAREFULLY`

## Instructions
**The test below will have up to 7 calendar days to be delivered from the date of receipt of this manual.**

- The code must be versioned in a public Github repository and a link must be sent for evaluation once completed
- Upload this template to your repository and start working from it
- Read the instructions carefully and make sure all requirements are being addressed
- The repository must provide instructions on how to configure, execute and test the project
- Documentation and overall organization will also be taken into consideration

## Use Case
**You are a developer on the DeveloperStore team. Now we need to implement the API prototypes.**

As we work with `DDD`, to reference entities from other domains, we use the `External Identities` pattern with denormalization of entity descriptions.

Therefore, you will write an API (complete CRUD) that handles sales records. The API needs to be able to inform:

* Sale number
* Date when the sale was made
* Customer
* Total sale amount
* Branch where the sale was made
* Products
* Quantities
* Unit prices
* Discounts
* Total amount for each item
* Cancelled/Not Cancelled

It's not mandatory, but it would be a differential to build code for publishing events of:
* SaleCreated
* SaleModified
* SaleCancelled
* ItemCancelled

If you write the code, **it's not required** to actually publish to any Message Broker. You can log a message in the application log or however you find most convenient.

### Business Rules

* Purchases above 4 identical items have a 10% discount
* Purchases between 10 and 20 identical items have a 20% discount
* It's not possible to sell above 20 identical items
* Purchases below 4 items cannot have a discount

These business rules define quantity-based discounting tiers and limitations:

1. Discount Tiers:
   - 4+ items: 10% discount
   - 10-20 items: 20% discount

2. Restrictions:
   - Maximum limit: 20 items per product
   - No discounts allowed for quantities below 4 items

# DeveloperStore Project Documentation

## Overview
DeveloperStore is a backend project designed to manage sales records and support operations like creating, updating, and canceling sales. It integrates key concepts like Domain-Driven Design (DDD) and event publishing to simulate real-world complexities. It includes competencies in:
- Designing and implementing APIs.
- Applying business rules effectively.
- Leveraging event-driven architecture (RabbitMQ).
- Writing clean, testable, and maintainable code.

Key features:
- Complete CRUD operations for managing sales.
- Business rules for applying item discounts based on quantity tiers.
- Event publishing for `SaleCreated`, `SaleModified`, and `SaleCancelled`.
- Logging for better traceability using Log4Net.


## Tech Stack
The project uses the following technologies:
- **Backend**: 
  - Language: C# (.NET 9.0)
  - Framework: ASP.NET Core for API development.
- **Database**: SQL Server for storing sales data.
- **Event Bus**: RabbitMQ (mocked in the implementation for simplicity).
- **Testing**: 
  - xUnit for unit and integration testing.
  - Moq for mocking dependencies during tests.
- **Logging**: Log4Net for structured and file-based logging.


## Frameworks
The project leverages these frameworks and libraries:
- **Microsoft.Extensions.DependencyInjection**: For dependency injection.
- **Entity Framework Core**: For database interactions and migrations.
- **Log4Net**: For logging to the console and files, configured with rolling log files.
- **RabbitMQ.Client**: Simulates message publishing for event-based operations.
- **xUnit**: For implementing unit tests.
- **Moq**: To mock dependencies while testing the service layers.


## Project Structure
The project follows a clean architecture approach to separate concerns and improve maintainability. The structure is as follows:

- **DeveloperStore.Api**: The entry point for the application, containing the controllers and middleware configuration.
- **DeveloperStore.Application**: Contains business logic and service interfaces.
- **DeveloperStore.Domain**: Contains domain entities, value objects, and core business rules.
- **DeveloperStore.Infrastructure**: Handles persistence (e.g., repositories) and external system integrations (e.g., RabbitMQ).
- **DeveloperStore.Shared**: Contains shared DTOs and other utility classes used across different layers.
- **DeveloperStore.Tests**: Includes unit and integration tests for validating business logic and ensuring code quality.

Each layer communicates with others through interfaces, ensuring loose coupling and easier testability.

## Step-by-Step Instructions

### 1. Clone the Repository
Clone the repository to your local machine and navigate into the project folder:
Run the following command:

```
git clone https://github.com/bortot-systems/DeveloperStore.git
cd DeveloperStore
```

### 2. Install Dependencies
Ensure you have the following installed:
- **.NET SDK 9.0** or later
- **A SQL Server instance** (e.g., installed locally or running in a Docker container)

### 3. Configure the Database
Open the **`appsettings.json`** file in the `DeveloperStore.Api` project and update the connection string to match your database settings:

```
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=DeveloperStoreDb;Trusted_Connection=True;"
}
```
Replace `localhost` and `Trusted_Connection=True` with your specific database details if necessary.

### 4. Apply Migrations to Create the Database
Run the following command to apply migrations and create the database schema:

```
dotnet ef database update
```

This will ensure that the database is created and seeded with the necessary schema.


### 5. Run the Application
Navigate to the `DeveloperStore.Api` folder and start the application:

```
dotnet run --project DeveloperStore.Api
```

The API will start and will be accessible at:

```
http://localhost:5000
```

### 6. Test the API
Use tools like **Postman, Swagger, or cURL** to test the API. For example:

```
curl --request GET http://localhost:5000/api/sales

http://localhost:5000/swagger/
```


## API Structure

The API provides endpoints to manage sales operations, including creating, updating, canceling, and retrieving sales records. It follows RESTful principles and supports JSON for request and response payloads.

### Base URL

http://localhost:5000/api

### Endpoints

#### **1. Sales API**
| Method | Endpoint                     | Description                                      
|--------|-------------------------------|--------------------------------------------------
| GET    | `/sales`                     | Retrieve a list of all sales records.            
| GET    | `/sales/{saleId}`            | Retrieve a specific sale by its ID.              
| POST   | `/sales`                     | Create a new sale record.                       
| PUT    | `/sales/{saleId}`            | Update an existing sale record.                  
| DELETE | `/sales/{saleId}`            | Cancel an existing sale (mark as canceled).      

#### **2. Events API**
Events related to sales are published for actions like creating, updating, and canceling sales. The following events are supported:
- `SaleCreated`
- `SaleModified`
- `SaleCancelled`

### Example Request and Response

#### **Creating a New Sale (POST /sales)**
**Request:**
```json
{  
  "saleNumber": "INV-20250303",
  "customerId": "a9b4d661-f123-49cc-85b8-d821c2f94bf5",
  "branch": "Test Store",
  "items": [
    {
      "productId": "a1b2c3d4-e5f6-7890-1234-56789abcdef0",
      "quantity": 5,
      "unitPrice": 20.00
    },
    {
      "productId": "123e4567-e89b-12d3-a456-426614174000",
      "quantity": 1,
      "unitPrice": 499.99
    }
  ]
}
```

#### **Updating Sale (PUT /sales)**

**Path Parameters:**  
| Name | Type   | Description                |
|------|--------|----------------------------|
| `id` | string | The unique ID of the sale. |

**Request Body Example:**

```json
{  
  "saleNumber": "INV-20250306",
  "customerId": "b72f12b1-34ac-44df-91b9-a2c492fbde4f",
  "branch": "My Store",
  "items": [
    {
      "productId": "a123bc45-def6-7890-1234-5678abcdef90",
      "quantity": 12,
      "unitPrice": 30.00
    },
    {
      "productId": "b456cd78-ef90-1234-5678-9abcdef01234",
      "quantity": 9,
      "unitPrice": 100.00
    }
  ]
}
```

