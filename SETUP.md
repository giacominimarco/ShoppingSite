# Setup and Execution Instructions

This document provides step-by-step instructions to configure, execute, and test the Sales API project.

## Prerequisites

- .NET 8.0 SDK
- PostgreSQL 12 or higher
- Docker (optional, for containerized execution)

## Configuration

### 1. Database Setup

#### Option A: Using Docker (Recommended)

```bash
# Start PostgreSQL container
docker run --name postgres-sales -e POSTGRES_PASSWORD=your_password -e POSTGRES_DB=sales_db -p 5432:5432 -d postgres:15
```

#### Option B: Local PostgreSQL Installation

1. Install PostgreSQL on your system
2. Create a database named `sales_db`
3. Note the connection details

### 2. Connection String Configuration

Update the connection string in `template/backend/src/Ambev.DeveloperEvaluation.WebApi/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=sales_db;Username=postgres;Password=your_password"
  }
}
```

### 3. Environment Variables (Optional)

You can also use environment variables:

```bash
export ConnectionStrings__DefaultConnection="Host=localhost;Database=sales_db;Username=postgres;Password=your_password"
```

## Execution

### 1. Navigate to the Backend Directory

```bash
cd template/backend
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Project

```bash
dotnet build
```

### 4. Run Database Migrations

```bash
# Navigate to the WebApi project
cd src/Ambev.DeveloperEvaluation.WebApi

# Create and apply migrations
dotnet ef database update
```

### 5. Run the Application

```bash
dotnet run
```

The API will be available at:
- **Swagger UI**: https://localhost:7001/swagger
- **API Base URL**: https://localhost:7001/api

## Testing the API

### 1. Using Swagger UI

1. Open your browser and navigate to `https://localhost:7001/swagger`
2. You'll see all available endpoints for the Sales API
3. Click on any endpoint to expand and test it

### 2. Using curl

#### Create a Sale

```bash
curl -X POST "https://localhost:7001/api/sales" \
  -H "Content-Type: application/json" \
  -d '{
    "customer": "John Doe",
    "branch": "Downtown Store",
    "items": [
      {
        "product": "Beer - Premium Lager",
        "quantity": 5,
        "unitPrice": 10.50
      },
      {
        "product": "Wine - Red Blend",
        "quantity": 12,
        "unitPrice": 25.00
      }
    ]
  }'
```

#### Get a Sale

```bash
curl -X GET "https://localhost:7001/api/sales/{sale-id}"
```

#### Get Sales List

```bash
curl -X GET "https://localhost:7001/api/sales?_page=1&_size=10"
```

#### Cancel a Sale

```bash
curl -X POST "https://localhost:7001/api/sales/{sale-id}/cancel"
```

### 3. Using Postman

1. Import the collection from `template/backend/src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.http`
2. Update the base URL to `https://localhost:7001`
3. Execute the requests

## Business Rules Testing

### Discount Rules

Test the automatic discount application:

1. **No Discount (1-3 items)**: Create a sale with 3 items of the same product
2. **10% Discount (4-9 items)**: Create a sale with 5 items of the same product
3. **20% Discount (10-20 items)**: Create a sale with 12 items of the same product

### Quantity Limitations

Test the quantity limits:

1. **Valid Quantity**: Create a sale with 20 items (should work)
2. **Invalid Quantity**: Try to create a sale with 21 items (should fail)

## Troubleshooting

### Common Issues

1. **Database Connection Error**
   - Verify PostgreSQL is running
   - Check connection string in appsettings.json
   - Ensure database exists

2. **Migration Errors**
   - Delete existing migrations folder
   - Run `dotnet ef migrations add InitialCreate`
   - Run `dotnet ef database update`

3. **Port Already in Use**
   - Change the port in `launchSettings.json`
   - Or kill the process using the port

4. **SSL Certificate Issues**
   - Run `dotnet dev-certs https --trust`
   - Or use HTTP instead of HTTPS

### Logs

Check the console output for detailed logs and error messages. The application uses Serilog for structured logging.

## Development

### Project Structure

```
template/backend/
├── src/
│   ├── Ambev.DeveloperEvaluation.Domain/     # Domain entities and business logic
│   ├── Ambev.DeveloperEvaluation.Application/ # Application services and commands
│   ├── Ambev.DeveloperEvaluation.ORM/        # Data access layer
│   ├── Ambev.DeveloperEvaluation.WebApi/     # API controllers and endpoints
│   └── Ambev.DeveloperEvaluation.IoC/        # Dependency injection setup
└── tests/                                     # Unit and integration tests
```

### Adding New Features

1. Create domain entities in the Domain project
2. Add application commands/handlers in the Application project
3. Implement repository in the ORM project
4. Create API controllers in the WebApi project
5. Register dependencies in the IoC project

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/Ambev.DeveloperEvaluation.Unit/

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Production Deployment

### Docker Deployment

```bash
# Build the Docker image
docker build -t sales-api .

# Run the container
docker run -p 8080:80 -e ConnectionStrings__DefaultConnection="your_connection_string" sales-api
```

### Environment Configuration

For production, ensure to:

1. Use strong passwords for database
2. Configure proper logging
3. Set up monitoring and health checks
4. Use HTTPS in production
5. Configure CORS policies
6. Set up proper authentication/authorization

## Support

For issues or questions:

1. Check the logs for error details
2. Review the API documentation in `/.doc/sales-api.md`
3. Run the tests to verify functionality
4. Check the business rules implementation in the Domain project
