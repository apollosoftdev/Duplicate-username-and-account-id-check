# Username Validation Service

A robust .NET Core 6 microservice for checking duplicate usernames and account IDs in a game world environment. This service ensures unique username and account ID combinations while providing RESTful API endpoints for validation and storage operations.

## Features

- **Username Validation**: Validates usernames against requirements (6-30 characters, alphanumeric only)
- **Account Management**: Stores and manages username-account ID combinations
- **Duplicate Prevention**: Ensures no duplicate usernames or account IDs
- **Username Updates**: Allows users to change their usernames while maintaining uniqueness
- **RESTful API**: Clean, documented API endpoints
- **Scalable Architecture**: Designed to handle 1M+ users efficiently

## Requirements

- .NET 6.0 SDK
- Visual Studio 2022 (recommended)
- SQL Server LocalDB (for production) or In-Memory Database (for development)

## Project Structure

```
UsernameValidationService/
├── Controllers/
│   └── UsernameController.cs          # API endpoints
├── Data/
│   └── ApplicationDbContext.cs        # Entity Framework context
├── DTOs/
│   ├── UsernameValidationRequest.cs   # Request DTOs
│   ├── UsernameValidationResponse.cs  # Response DTOs
│   ├── UserAccountRequest.cs
│   └── UserAccountResponse.cs
├── Models/
│   └── UserAccount.cs                 # Data model
├── Services/
│   ├── IUsernameValidationService.cs  # Service interface
│   └── UsernameValidationService.cs   # Business logic
└── Program.cs                         # Application entry point
```

## Quick Start

### 1. Clone the Repository
```bash
git clone https://github.com/apollosoftdev/Duplicate-username-and-account-id-check.git
cd Duplicate-username-and-account-id-check
```

### 2. Build the Project
```bash
cd UsernameValidationService
dotnet build
```

### 3. Run the Application
```bash
dotnet run
```

The application will start on:
- **HTTPS**: `https://localhost:7044`
- **HTTP**: `http://localhost:5120`

### 4. Access Swagger Documentation
Navigate to `https://localhost:7044/swagger` to view the interactive API documentation.

## API Endpoints

### 1. Validate Username
**GET** `/api/username/validate?username={username}`

Validates if a username meets the requirements:
- 6-30 characters
- Alphanumeric characters only
- Not already taken

**Response:**
```json
{
  "isValid": true,
  "message": "Username is valid",
  "errors": []
}
```

### 2. Store User Account
**POST** `/api/username/store`

Stores a new username and account ID combination.

**Request Body:**
```json
{
  "accountId": "12345678-1234-1234-1234-123456789012",
  "username": "myusername"
}
```

**Response:**
```json
{
  "success": true,
  "message": "User account created successfully",
  "accountId": "12345678-1234-1234-1234-123456789012",
  "username": "myusername",
  "errors": []
}
```

### 3. Update Username
**POST** `/api/username/update`

Updates the username for an existing account.

**Request Body:**
```json
{
  "accountId": "12345678-1234-1234-1234-123456789012",
  "username": "newusername"
}
```

### 4. Check Username Availability
**GET** `/api/username/check-availability?username={username}`

Checks if a username is available.

**Response:**
```json
{
  "available": true,
  "username": "myusername"
}
```

### 5. Check Account Exists
**GET** `/api/username/check-account?accountId={accountId}`

Checks if an account ID exists.

**Response:**
```json
{
  "exists": true,
  "accountId": "12345678-1234-1234-1234-123456789012"
}
```

## Business Rules

1. **Account ID**: Must be a valid System.Guid
2. **Username Requirements**:
   - 6-30 characters
   - Alphanumeric characters only (a-z, A-Z, 0-9)
   - Must be unique across all users
3. **One-to-One Relationship**: One account ID can have only one username
4. **Username Updates**: When updating, the old username is replaced with the new one

## Database Configuration

### Development (In-Memory Database)
The project is configured to use an in-memory database for development, which requires no setup.

### Production (SQL Server)
To use SQL Server:

1. Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=UsernameValidationDb;Trusted_Connection=true;"
  }
}
```

2. Update `Program.cs` to use SQL Server:
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

3. Run migrations:
```bash
dotnet ef database update
```

## Testing

### Manual Testing with Swagger
1. Start the application: `dotnet run`
2. Navigate to: `https://localhost:7044/swagger`
3. Test the endpoints interactively

### Command Line Testing
When using curl commands, use the `-k` flag to bypass SSL certificate validation for development:
```bash
curl -k -X GET "https://localhost:7044/api/username/validate?username=testuser123"
```

### Example Test Scenarios

1. **Valid Username Creation**:
   ```bash
   curl -k -X POST "https://localhost:7044/api/username/store" \
        -H "Content-Type: application/json" \
        -d '{"accountId":"12345678-1234-1234-1234-123456789012","username":"testuser123"}'
   ```

2. **Username Validation**:
   ```bash
   curl -k -X GET "https://localhost:7044/api/username/validate?username=testuser123"
   ```

3. **Duplicate Username Test**:
   ```bash
   curl -k -X POST "https://localhost:7044/api/username/store" \
        -H "Content-Type: application/json" \
        -d '{"accountId":"87654321-4321-4321-4321-210987654321","username":"testuser123"}'
   ```

## Performance Considerations

- **Indexing**: Database indexes on Username and AccountId for fast lookups
- **Case-Insensitive**: Username comparisons are case-insensitive
- **Scalability**: Designed to handle 1M+ users efficiently
- **Concurrency**: Proper handling of concurrent username updates

## Error Handling

The service provides comprehensive error handling:
- Validation errors with detailed messages
- Database constraint violations
- Network and connection issues
- Proper HTTP status codes

## Development

### Adding New Features
1. Create DTOs in the `DTOs/` folder
2. Add business logic to `Services/UsernameValidationService.cs`
3. Create controllers in the `Controllers/` folder
4. Update the database context if needed

### Running Migrations
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

## Deployment

### Docker Support
The application can be containerized using Docker:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UsernameValidationService.csproj", "./"]
RUN dotnet restore "UsernameValidationService.csproj"
COPY . .
RUN dotnet build "UsernameValidationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UsernameValidationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UsernameValidationService.dll"]
```

## License

This project is created for the Full Stack Engineer (.NET/Angular) coding test.

## Support

For questions or issues, please refer to the API documentation at `/swagger` when the application is running.
