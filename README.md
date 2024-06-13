# .Net Core Tasks WebApi
This is a simple NetCore Web API that allows to register tasks.

> Motivation: I made this project for learning purpose.

## Features
1. Database connection with PostgresQL
2. Code-First with Entity-Framework
3. Dependency Injection pattern
4. Swagger docs

## How to use it

### Run it locally
1. Make sure you have installed the DotNet SDK (>= net8.0)
2. Clone this repository
3. Run the local database using Docker (If you already have a database just update the connection string in the `appsettings.json` file and skip this step)

    ```bash
    docker compose up
    ```

4. Open a terminal in the project directory and install the dependencies.

    ```bash
    dotnet restore
    ```

5. Run the migrations.

    ```
    dotnet ef database update
    ```

6. Run the server (development mode).
    ```bash
    dotnet run
    ```

### Endpoints

Documentation can also be found on http://localhost:5272/swagger/index.html.

#### GET: /api/categories

Returns the list of categories.

##### Request Example
```bash
curl -X GET http://localhost:5272/api/categories
```

##### Response Example
```
[
  {
    "id": "string",
    "createdAt": "string",
    "updatedAt": "string",
    "name": "string",
    "description": "string",
    "weight": 0
  }
]
```

#### GET: /api/categories/{id}

Returns the information of a single category by its ID.

##### Request Example
```bash
curl -X GET http://localhost:5272/api/categories/{id}
```

##### Response Example
```
{
  "id": "string",
  "createdAt": "string",
  "updatedAt": "string",
  "name": "string",
  "description": "string",
  "weight": 0
}
```

#### POST: /api/categories

Creates a new category.

##### Request Example
```bash
curl -X POST -H 'Content-Type:application/json' http://localhost:5272/api/categories -d '{"name":"string","description":"string","weight":0}'
```

##### Response Example
```
{
  "id": "string",
  "createdAt": "string",
  "updatedAt": "string",
  "name": "string",
  "description": "string",
  "weight": 0
}
```

#### PUT: /api/categories/{id}

Updates a category by providing its ID and the data to update.

##### Request Example
```bash
curl -X PUT -H 'Content-Type:application/json' http://localhost:5272/api/categories/{id} -d '{"name":"string","description":"string","weight":0}'
```

##### Response Example
```
{
  "id": "string",
  "createdAt": "string",
  "updatedAt": "string",
  "name": "string",
  "description": "string",
  "weight": 0
}
```

#### DELETE: /api/categories/{id}

Hard-deletes a category by its ID.

##### Request Example
```bash
curl -X DELETE http://localhost:5272/api/categories/{id}
```

##### Response Example
```
{
  "id": "string",
  "createdAt": "string",
  "updatedAt": "string",
  "name": "string",
  "description": "string",
  "weight": 0
}
```

#### GET: /api/tasks/{id}

Returns the information of a single task by its ID.

##### Request Example
```bash
curl -X GET http://localhost:5272/api/tasks/{id}
```

##### Response Example
```
{
  "id": "string",
  "createdAt": "string",
  "updatedAt": "string",
  "name": "string",
  "description": "string",
  "priority": 0,
  "categoryId": "string",
  "shortDescription": "string"
}
```

#### GET: /api/tasks

Returns the tasks.

##### Request Example
```bash
curl -X GET http://localhost:5272/api/tasks
```

##### Response Example
```
[
  {
    "id": "string",
    "createdAt": "string",
    "updatedAt": "string",
    "name": "string",
    "description": "string",
    "priority": 0,
    "categoryId": "string",
    "shortDescription": "string"
  }
]
```

#### POST: /api/tasks

Creates a new task.

##### Request Example
```bash
curl -X POST -H 'Content-Type:application/json' http://localhost:5272/api/tasks -d '{"name":"string","description":"string","categoryId":"string","priority":0}'
```

##### Response Example
```
{
  "id": "string",
  "createdAt": "string",
  "updatedAt": "string",
  "name": "string",
  "description": "string",
  "priority": 0,
  "categoryId": "string",
  "shortDescription": "string"
}
```

#### PUT: /api/tasks/{id}

Updates a task by providing its ID and the data to update.

##### Request Example
```bash
curl -X PUT -H 'Content-Type:application/json' http://localhost:5272/api/tasks/{id} -d '{"name":"string","description":"string","categoryId":"string","priority":0}'
```

##### Response Example
```
{
  "id": "string",
  "createdAt": "string",
  "updatedAt": "string",
  "name": "string",
  "description": "string",
  "priority": 0,
  "categoryId": "string",
  "shortDescription": "string"
}
```

#### DELETE: /api/tasks/{id}

Hard-deletes a task by its ID.

##### Request Example
```bash
curl -X DELETE http://localhost:5272/api/tasks/{id}
```

##### Response Example
```
{
  "id": "string",
  "createdAt": "string",
  "updatedAt": "string",
  "name": "string",
  "description": "string",
  "priority": 0,
  "categoryId": "string",
  "shortDescription": "string"
}
```
