# Introduction
    1.This is a Employee Management System console application.
    2.It supports all CRUD operations for employees.

# Prerequisites
Before running this application, ensure that you have the following prerequisites:
- .NET Core SDK installed.

# Running The Application:

You can run the application using the dotnet run command followed by various options:

display: Display all employee details.
- `dotnet run -- -o display`

update: Update employee information.
- `dotnet run -- -o update -i TZ101`

delete: Delete an employee.
- `dotnet run -- -o delete -i TZ101`

help: Display help information.
- `dotnet run -- -o help`

add: Add a new employee.
- `dotnet run -- -o add`

filter: Apply filters to employee data.
- `dotnet run -- -o filter`

Command Line Arguments.
- -o: Specifies the operation to perform (display, update, delete, help, add, filter).
- -i: Specifies the employee ID for update and delete operations.