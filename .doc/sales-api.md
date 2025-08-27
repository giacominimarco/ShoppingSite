[Back to README](../README.md)

# Sales API

The Sales API provides endpoints for managing sales records with business rules for quantity-based discounts and limitations.

## Endpoints

### Create Sale

Creates a new sale with items and applies business rules for discounts.

**POST** `/api/sales`

#### Request Body

```json
{
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
}
```

#### Response

```json
{
  "success": true,
  "message": "Sale created successfully",
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "saleNumber": "SALE-20241201-ABC12345",
    "saleDate": "2024-12-01T10:30:00Z",
    "customer": "John Doe",
    "totalAmount": 315.00,
    "branch": "Downtown Store",
    "status": "Active",
    "createdAt": "2024-12-01T10:30:00Z",
    "items": [
      {
        "id": "123e4567-e89b-12d3-a456-426614174001",
        "product": "Beer - Premium Lager",
        "quantity": 5,
        "unitPrice": 10.50,
        "discount": 10.0,
        "totalAmount": 47.25
      },
      {
        "id": "123e4567-e89b-12d3-a456-426614174002",
        "product": "Wine - Red Blend",
        "quantity": 12,
        "unitPrice": 25.00,
        "discount": 20.0,
        "totalAmount": 240.00
      }
    ]
  }
}
```

### Get Sale

Retrieves a sale by its ID.

**GET** `/api/sales/{id}`

#### Response

```json
{
  "success": true,
  "message": "Sale retrieved successfully",
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "saleNumber": "SALE-20241201-ABC12345",
    "saleDate": "2024-12-01T10:30:00Z",
    "customer": "John Doe",
    "totalAmount": 315.00,
    "branch": "Downtown Store",
    "status": "Active",
    "createdAt": "2024-12-01T10:30:00Z",
    "updatedAt": null,
    "items": [
      {
        "id": "123e4567-e89b-12d3-a456-426614174001",
        "product": "Beer - Premium Lager",
        "quantity": 5,
        "unitPrice": 10.50,
        "discount": 10.0,
        "totalAmount": 47.25
      }
    ]
  }
}
```

### Get Sales List

Retrieves a paginated list of sales with optional filtering.

**GET** `/api/sales`

#### Query Parameters

- `_page`: Page number (default: 1)
- `_size`: Number of items per page (default: 10)
- `customer`: Filter by customer name (supports partial matching with `*`)
- `branch`: Filter by branch name (supports partial matching with `*`)
- `status`: Filter by sale status (Active, Cancelled)
- `_minDate`: Filter by minimum sale date
- `_maxDate`: Filter by maximum sale date
- `_minTotalAmount`: Filter by minimum total amount
- `_maxTotalAmount`: Filter by maximum total amount
- `_order`: Order by field(s) (e.g., "SaleDate desc, TotalAmount asc")

#### Example Request

```
GET /api/sales?_page=1&_size=20&customer=John*&_minDate=2024-01-01&_order="SaleDate desc"
```

#### Response

```json
{
  "success": true,
  "message": "Sales retrieved successfully",
  "data": {
    "sales": [
      {
        "id": "123e4567-e89b-12d3-a456-426614174000",
        "saleNumber": "SALE-20241201-ABC12345",
        "saleDate": "2024-12-01T10:30:00Z",
        "customer": "John Doe",
        "totalAmount": 315.00,
        "branch": "Downtown Store",
        "status": "Active",
        "createdAt": "2024-12-01T10:30:00Z"
      }
    ],
    "totalCount": 1,
    "page": 1,
    "size": 20,
    "totalPages": 1
  }
}
```

### Cancel Sale

Cancels a sale by its ID.

**POST** `/api/sales/{id}/cancel`

#### Response

```json
{
  "success": true,
  "message": "Sale cancelled successfully",
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "saleNumber": "SALE-20241201-ABC12345",
    "status": "Cancelled",
    "cancelledAt": "2024-12-01T11:00:00Z"
  }
}
```

## Business Rules

### Quantity-Based Discounts

The API automatically applies discounts based on item quantities:

- **4+ items**: 10% discount
- **10-20 items**: 20% discount
- **Below 4 items**: No discount

### Quantity Limitations

- **Maximum limit**: 20 items per product
- **Minimum limit**: 1 item per product

### Validation Rules

- Customer name is required and cannot exceed 200 characters
- Branch name is required and cannot exceed 200 characters
- At least one item is required per sale
- Product name is required and cannot exceed 200 characters
- Quantity must be between 1 and 20
- Unit price must be greater than zero

## Error Responses

### Validation Error

```json
{
  "type": "ValidationError",
  "error": "Invalid input data",
  "detail": "The 'quantity' field must be between 1 and 20"
}
```

### Resource Not Found

```json
{
  "type": "ResourceNotFound",
  "error": "Sale not found",
  "detail": "The sale with ID 123e4567-e89b-12d3-a456-426614174000 does not exist"
}
```

### Business Rule Violation

```json
{
  "type": "BusinessRuleViolation",
  "error": "Quantity limit exceeded",
  "detail": "Cannot sell more than 20 identical items"
}
```

## Events

The API publishes the following domain events (logged to console in this implementation):

- `SaleCreatedEvent`: When a new sale is created
- `SaleModifiedEvent`: When a sale is modified
- `SaleCancelledEvent`: When a sale is cancelled
- `ItemCancelledEvent`: When an item is cancelled from a sale

<br/>
<div style="display: flex; justify-content: space-between;">
  <a href="./general-api.md">Previous: General API</a>
  <a href="../README.md">Next: README</a>
</div>
