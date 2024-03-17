
Running The Application:
//---------------------------------------------------------------------------
You can run the application using the dotnet run command followed by various options:

display: Display all employee details.
command - dotnet run -- -o display

update: Update employee information.
command - dotnet run -- -o update -i TZ101

delete: Delete an employee.
command - dotnet run -- -o delete -i TZ101

help: Display help information.
command - dotnet run -- -o help

add: Add a new employee.
command - dotnet run -- -o add

filter: Apply filters to employee data.
command - dotnet run -- -o filter

Command Line Arguments
-o: Specifies the operation to perform (display, update, delete, help, add, filter).
-i: Specifies the employee ID for update and delete operations.