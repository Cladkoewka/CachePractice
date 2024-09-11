# Customer Management API

## Overview

This project is an ASP.NET Core Web API for managing customer data. It allows for basic CRUD (Create, Read, Update, Delete) operations on customer records. The primary focus of this project is to learn and practice caching strategies using `MemoryCache` and `Redis`.


## Technologies Used

- .NET Core
- EF Core
- PostgreSQL
- MemoryCache
- Redis 

## API Endpoints

### 1. Get All Customers
- **Endpoint**: `GET /api/customers`
- **Description**: Retrieve a list of all customers.
- **Response**: 
  - **200 OK**: Returns a list of `Customer` objects.
  - **404 Not Found**: If no customers are found.

### 2. Get Customer by ID
- **Endpoint**: `GET /api/customers/{id}`
- **Description**: Retrieve a specific customer by their ID.
- **Parameters**: 
  - `id` (int): The ID of the customer to retrieve.
- **Response**: 
  - **200 OK**: Returns the `Customer` object if found.
  - **404 Not Found**: If the customer with the specified ID is not found.

### 3. Add Customer
- **Endpoint**: `POST /api/customers`
- **Description**: Add a new customer to the database.
- **Request Body**: 
  - JSON object with the following fields:
    - `name` (string): The name of the customer.
    - `email` (string): The email of the customer.
- **Response**: 
  - **201 Created**: Returns the created `Customer` object.
  - **400 Bad Request**: If the request body is invalid.

### 4. Update Customer
- **Endpoint**: `PUT /api/customers/{id}`
- **Description**: Update an existing customer's details.
- **Parameters**: 
  - `id` (int): The ID of the customer to update.
- **Request Body**: 
  - JSON object with updated fields:
    - `name` (string): The updated name of the customer.
    - `email` (string): The updated email of the customer.
- **Response**: 
  - **204 No Content**: If the update is successful.
  - **404 Not Found**: If the customer with the specified ID is not found.

### 5. Delete Customer
- **Endpoint**: `DELETE /api/customers/{id}`
- **Description**: Delete a customer from the database.
- **Parameters**: 
  - `id` (int): The ID of the customer to delete.
- **Response**: 
  - **204 No Content**: If the deletion is successful.
  - **404 Not Found**: If the customer with the specified ID is not found.

