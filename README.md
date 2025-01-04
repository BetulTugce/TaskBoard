# TaskBoard

## Overview

TaskBoard is a web API project currently being developed for managing tasks within teams.

## Configuration

The `appsettings.json` file is not included in the repository as it contains sensitive information, such as API keys. Instead, you should create your own `appsettings.json` file with the following structure for api project:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "PostgreSQL": "User ID=postgres;Password=your_password;Host=localhost;Port=5432;Database=your_db_name;"
  }
}
```

## Contributing
If you would like to contribute to this project, feel free to open an issue or submit a pull request.