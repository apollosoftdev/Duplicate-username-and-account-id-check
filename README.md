# Username Validation Microservice

A robust .NET Core 6 microservice for checking duplicate usernames and account IDs in a game world environment. This service ensures unique username and account ID combinations while providing comprehensive validation and storage capabilities.

## 🎯 Project Overview

This microservice is designed to handle username validation and storage for a gaming platform with over 1M users. It provides RESTful APIs to validate usernames and store user account information with strict uniqueness constraints.

## ✨ Features

- **Username Validation**: Ensures usernames meet requirements (6-30 characters, alphanumeric only)
- **Unique Constraints**: Prevents duplicate usernames and account IDs
- **Account Management**: Store and update username-account ID combinations
- **RESTful API**: Clean, documented endpoints for all operations
- **Database Integration**: SQL Server with Entity Framework Core
- **Scalable Architecture**: Designed to handle high-volume requests
- **Comprehensive Error Handling**: Detailed validation and error responses

## 🏗️ Architecture

```
UsernameValidationService/
├── Controllers/          # API endpoints
├── Data/                # Entity Framework context
├── DTOs/                # Data transfer objects
├── Models/              # Database entities
├── Services/            # Business logic layer
└── Program.cs           # Application entry point
```

## 🚀 Quick Start

### Prerequisites

- .NET 6.0 SDK
- Visual Studio 2022 (recommended)
- SQL Server (LocalDB, Express, or Azure SQL)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Duplicate-username-and-account-id-check
   ```

2. **Configure Database Connection**
   
   Update `UsernameValidationService/appsettings.json` with your SQL Server connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=your-server;Database=UsernameValidationDb;User Id=your-username;Password=your-password;Encrypt=True;TrustServerCertificate=False;"
     }
   }
   ```

3. **Apply Database Migrations**
   ```bash
   cd UsernameValidationService
   dotnet ef database update
   ```

4. **Run the Application**
   ```bash
   dotnet run
   ```

5. **Access Swagger Documentation**
   
   Navigate to `https://localhost:7001/swagger` to view the API documentation.

## 📋 API Endpoints

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
  "username": "gamer123"
}
```

**Response:**
```json
{
  "success": true,
  "message": "User account created successfully",
  "accountId": "12345678-1234-1234-1234-123456789012",
  "username": "gamer123",
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
  "username": "newgamer123"
}
```

### 4. Check Username Availability
**GET** `/api/username/check-availability?username={username}`

Checks if a username is available.

**Response:**
```json
{
  "available": true,
  "username": "gamer123"
}
```

### 5. Check Account Existence
**GET** `/api/username/check-account?accountId={accountId}`

Checks if an account ID exists.

**Response:**
```json
{
  "exists": true,
  "accountId": "12345678-1234-1234-1234-123456789012"
}
```

## 🗄️ Database Schema

### UserAccount Table
| Column | Type | Constraints |
|--------|------|-------------|
| AccountId | GUID | Primary Key, Unique |
| Username | NVARCHAR(30) | Unique, Not Null |
| CreatedAt | DATETIME2 | Default: UTC Now |
| UpdatedAt | DATETIME2 | Nullable |

## 🔧 Configuration

### Environment Variables
- `ConnectionStrings:DefaultConnection`: SQL Server connection string
- `Logging:LogLevel`: Logging configuration

### Validation Rules
- **Username Length**: 6-30 characters
- **Username Characters**: Alphanumeric only (a-z, A-Z, 0-9)
- **Account ID**: System.Guid format
- **Uniqueness**: No duplicate usernames or account IDs

## 🧪 Testing

### Manual Testing with Swagger
1. Open `https://localhost:7001/swagger`
2. Test each endpoint with various inputs
3. Verify validation rules and error responses

### Example Test Cases

**Valid Username:**
- `gamer123` ✅
- `Player2023` ✅
- `TestUser456` ✅

**Invalid Usernames:**
- `abc` ❌ (too short)
- `verylongusername123456789` ❌ (too long)
- `user@name` ❌ (special characters)
- `user-name` ❌ (hyphens not allowed)

## 🚀 Deployment

### Local Development
```bash
dotnet run --project UsernameValidationService
```

### Production Deployment
1. Update connection string for production database
2. Set appropriate logging levels
3. Configure HTTPS certificates
4. Deploy to Azure App Service, AWS, or other hosting platform

## 📦 Dependencies

- **.NET Core 6.0**
- **Entity Framework Core 6.0.25**
- **SQL Server Provider**
- **Swashbuckle.AspNetCore** (Swagger documentation)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is created for educational and demonstration purposes.

## 🆘 Support

For issues or questions:
1. Check the API documentation at `/swagger`
2. Review the validation rules
3. Ensure database connection is properly configured

---

**Built with ❤️ using .NET Core 6 and Entity Framework Core**
