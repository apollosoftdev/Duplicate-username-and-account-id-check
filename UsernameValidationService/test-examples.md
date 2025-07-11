# API Test Examples

## Testing the Username Validation Service

### 1. Validate Username (GET)

**Valid Username:**
```bash
curl -k -X GET "https://localhost:7044/api/username/validate?username=testuser123"
```

**Response:**
```json
{
  "isValid": true,
  "message": "Username is valid",
  "errors": []
}
```

**Invalid Username (too short):**
```bash
curl -k -X GET "https://localhost:7044/api/username/validate?username=abc"
```

**Response:**
```json
{
  "isValid": false,
  "message": "Username validation failed",
  "errors": [
    "Username must be between 6 and 30 characters"
  ]
}
```

**Invalid Username (special characters):**
```bash
curl -k -X GET "https://localhost:7044/api/username/validate?username=user@name"
```

**Response:**
```json
{
  "isValid": false,
  "message": "Username validation failed",
  "errors": [
    "Username must contain only alphanumeric characters"
  ]
}
```

### 2. Store User Account (POST)

**Create New Account:**
```bash
curl -k -X POST "https://localhost:7044/api/username/store" \
     -H "Content-Type: application/json" \
     -d '{
       "accountId": "12345678-1234-1234-1234-123456789012",
       "username": "testuser123"
     }'
```

**Response:**
```json
{
  "success": true,
  "message": "User account created successfully",
  "accountId": "12345678-1234-1234-1234-123456789012",
  "username": "testuser123",
  "errors": []
}
```

**Try to Create Duplicate Account ID:**
```bash
curl -k -X POST "https://localhost:7044/api/username/store" \
     -H "Content-Type: application/json" \
     -d '{
       "accountId": "12345678-1234-1234-1234-123456789012",
       "username": "differentuser"
     }'
```

**Response:**
```json
{
  "success": false,
  "message": "Account ID already exists",
  "accountId": null,
  "username": null,
  "errors": [
    "Account ID is already registered"
  ]
}
```

### 3. Update Username (POST)

**Update Existing Account:**
```bash
curl -k -X POST "https://localhost:7044/api/username/update" \
     -H "Content-Type: application/json" \
     -d '{
       "accountId": "12345678-1234-1234-1234-123456789012",
       "username": "newusername456"
     }'
```

**Response:**
```json
{
  "success": true,
  "message": "Username updated successfully",
  "accountId": "12345678-1234-1234-1234-123456789012",
  "username": "newusername456",
  "errors": []
}
```

### 4. Check Username Availability (GET)

**Check Available Username:**
```bash
curl -k -X GET "https://localhost:7044/api/username/check-availability?username=availableuser"
```

**Response:**
```json
{
  "available": true,
  "username": "availableuser"
}
```

**Check Taken Username:**
```bash
curl -k -X GET "https://localhost:7044/api/username/check-availability?username=newusername456"
```

**Response:**
```json
{
  "available": false,
  "username": "newusername456"
}
```

### 5. Check Account Exists (GET)

**Check Existing Account:**
```bash
curl -k -X GET "https://localhost:7044/api/username/check-account?accountId=12345678-1234-1234-1234-123456789012"
```

**Response:**
```json
{
  "exists": true,
  "accountId": "12345678-1234-1234-1234-123456789012"
}
```

**Check Non-Existing Account:**
```bash
curl -k -X GET "https://localhost:7044/api/username/check-account?accountId=87654321-4321-4321-4321-210987654321"
```

**Response:**
```json
{
  "exists": false,
  "accountId": "87654321-4321-4321-4321-210987654321"
}
```

## Testing with Swagger UI

1. Start the application: `dotnet run`
2. Navigate to: `https://localhost:7000/swagger`
3. Use the interactive interface to test all endpoints
4. Try various scenarios:
   - Valid usernames (6-30 characters, alphanumeric)
   - Invalid usernames (too short, too long, special characters)
   - Duplicate account IDs
   - Duplicate usernames
   - Username updates

## Expected Behavior

- ✅ Usernames must be 6-30 characters
- ✅ Usernames must be alphanumeric only
- ✅ Usernames must be unique
- ✅ Account IDs must be unique
- ✅ One account ID can have only one username
- ✅ Username updates replace the old username
- ✅ Case-insensitive username comparisons
- ✅ Proper error messages for all validation failures 