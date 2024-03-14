using System;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EmployeeManagement;

public class Options
{
    [Option('o', "operation", Required = true, HelpText = "Operation to perform: add, display, update, delete, filter, help")]
    public string Operation { get; set; }

    [Option('i', "identifier", Required = false, HelpText = "Identifier for operation (e.g., employee ID)")]
    public string Identifier { get; set; }

    [Option('h', "help", HelpText = "Display this help screen")]
    public bool Help { get; set; }
}

public class Program
{
    private IConfigurationRoot _configuration;
    private ILogger _logger;
    private IEmployeeDal _employeeDal;
    private IEmployeeBal _employeeBal;
    private readonly string _filePath = "";

    public Program()
    {
        _configuration = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .Build();
        _logger = new Logger();
        _filePath = _configuration["AppSettings:FilePath"];
        _employeeDal = new EmployeeDal(_logger, _filePath);
        _employeeBal = new EmployeeBal(_logger, _employeeDal);
    }

    private Employee EmployeeInput()
    {
        long? mobileNumber = 0;
        string? employeeNumber, firstName, lastName, dateOfBirth, joiningDate, emailId, location, jobTitle, department, manager, project;
        int locationID=-1, departmentID = -1, jobID = -1;

        _logger.LogInfo("Enter Employee Number: ");
        employeeNumber = Console.ReadLine();

        _logger.LogInfo("Enter First Name: ");
        firstName = Console.ReadLine();

        _logger.LogInfo("Enter Last Name: ");
        lastName = Console.ReadLine();

        _logger.LogInfo("Enter Date Of Birth:(d/m/y)");
        dateOfBirth = Console.ReadLine();

        _logger.LogInfo("Enter Email ID: ");
        emailId = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(emailId))
        {
            while (!Regex.IsMatch(emailId, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                _logger.LogError("Invalid email format. Please enter a valid email address or leave blank to skip.");
                emailId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(emailId))
                {
                    break;
                }
            }
        }

        _logger.LogInfo("Enter Mobile Number: ");
        string mobileInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(mobileInput))
        {
            while (!(long.TryParse(mobileInput, out _) && mobileInput.Length == 10))
            {
                _logger.LogError("Invalid input for Mobile Number. Please enter a valid mobile number or leave blank to skip.");
                mobileInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(mobileInput))
                {
                    break;
                }
            }
            if (!string.IsNullOrWhiteSpace(mobileInput))
            {
                mobileNumber = long.Parse(mobileInput);
            }
        }
        _logger.LogInfo("Enter Joining Date: ");
        joiningDate = Console.ReadLine();

        _logger.LogInfo("Enter Location: ");
        location = Console.ReadLine();
        Location locationEnum;
        if (Enum.TryParse(location, out locationEnum))
        {
            locationID = (int)locationEnum;
        }
        else
        {
            _logger.LogError("Invalid location input!");
        }

        _logger.LogInfo("Enter Job Title: ");
        jobTitle = Console.ReadLine();
        JobTitle jobTitleEnum;
        if (Enum.TryParse(jobTitle, out jobTitleEnum))
        {
            jobID = (int)jobTitleEnum;
        }
        else
        {
            _logger.LogError("Invalid job title input!");
        }

        _logger.LogInfo("Enter Department: ");
        department = Console.ReadLine();
        Department departmentEnum;
        if (Enum.TryParse(department, out departmentEnum))
        {
            departmentID = (int)departmentEnum;
          
        }
        else
        {
            _logger.LogError("Invalid department input!");
        }

        _logger.LogInfo("Enter Assigned Manager: ");
        manager = Console.ReadLine();

        _logger.LogInfo("Enter Assigned Project: ");
        project = Console.ReadLine();

        Employee data = new Employee
        {
            EmployeeNumber = employeeNumber,
            FirstName = firstName,
            LastName = lastName,
            Dob = dateOfBirth,
            EmailId = emailId,
            MobileNumber = (long)mobileNumber,
            JoiningDate = joiningDate,
            Location = location,
            LocationID = locationID,
            JobTitle = jobTitle,
            JobID = jobID,
            Department = department,
            DeptID = departmentID,
            Manager = manager,
            Project = project,
        };
        return data;
    }

    private EmployeeFilter EmployeeFilterInput()
    {
        EmployeeFilter employeeFilterObject = new EmployeeFilter();
        _logger.LogInfo("Enter an alphabet:");
        employeeFilterObject.EmployeeName = Console.ReadLine();

        _logger.LogInfo("Enter Location:");
        string locationInput = Console.ReadLine();
        if (Enum.TryParse(locationInput, true, out Location location))
        {
            employeeFilterObject.Location = location;
        }
        else
        {
            Console.WriteLine("Invalid location input!");
        }

        _logger.LogInfo("Enter JobTitle:");
        string jobTitleInput = Console.ReadLine();
        if (Enum.TryParse(jobTitleInput, true, out JobTitle jobTitle))
        {
            employeeFilterObject.JobTitle = jobTitle;
        }
        else
        {
            Console.WriteLine("Invalid location input!");
        }

        _logger.LogInfo("Enter Manager:");
        employeeFilterObject.Manager = Console.ReadLine();


        _logger.LogInfo("Enter Project:");
        employeeFilterObject.Project = Console.ReadLine();
        return employeeFilterObject;
    }

    private void Get(string? employeeNumber)
    {
        List<Employee> employees = _employeeDal.FetchData();
        if (employees.Count > 0 && String.IsNullOrEmpty(employeeNumber))
        {

            foreach (var item in employees)
            {
                _logger.LogData("EmpNumber: {0}\n" + "EmpFirstName: {1}\n" + "EmpLastName: {2}\n" + "EmpDob: {3}\n" + "EmpEmailId: {4}\n" + "EmpMobileNumber: {5}\n" + "EmpJoiningDate: {6}\n" + "EmpLocation: {7}\n" + "EmpJobTitle: {8}\n" + "EmpDepartment: {9}\n" + "EmpManager: {10}\n" + "EmpProject: {11}\n", item.EmployeeNumber, item.FirstName, item.LastName, item.Dob, item.EmailId, item.MobileNumber, item.JoiningDate, item.Location, item.JobTitle, item.Department, item.Manager, item.Project);
            }
        }
        else if (employees.Count > 0 && !string.IsNullOrEmpty(employeeNumber))
        {
            foreach (var item in employees)
            {
                if (item.EmployeeNumber == employeeNumber)
                {
                    _logger.LogData("EmpNumber: {0}\n" + "EmpFirstName: {1}\n" + "EmpLastName: {2}\n" + "EmpDob: {3}\n" + "EmpEmailId: {4}\n" + "EmpMobileNumber: {5}\n" + "EmpJoiningDate: {6}\n" + "EmpLocation: {7}\n" + "EmpJobTitle: {8}\n" + "EmpDepartment: {9}\n" + "EmpManager: {10}\n" + "EmpProject: {11}", item.EmployeeNumber, item.FirstName, item.LastName, item.Dob, item.EmailId, item.MobileNumber, item.JoiningDate, item.Location, item.JobTitle, item.Department, item.Manager, item.Project);
                }
            }
        }
        else
        {
            _logger.LogError("No Employee Data");
        }
    }

    private void Help()
    {
        _logger.LogInfo("Options\n" + "add \t - \t To add an employee\n" + "display - \t To display all employee details\n" + "search \t - \t To display a particular employee data\n" + "delete \t - \t To delete an employee based on given employeenumber\n" + "update \t - \t To update employee details based on given employeenumber\n");
    }

    public static void Main(string[] args)
    {
        Program programObject = new Program();
        Parser.Default.ParseArguments<Options>(args)
       .WithParsed(options =>
       {
           if (options.Help)
           {
               var helpText = HelpText.AutoBuild(Parser.Default.ParseArguments<Options>(args));
               programObject._logger.LogError(helpText);
               return;
           }

           switch (options.Operation.ToLower())
           {
               case "add":
                   Employee data = programObject.EmployeeInput();
                   List<Employee> updatedEmployeeData = programObject._employeeBal.Add(data);
                   if (programObject._employeeDal.Insert(updatedEmployeeData))
                   {
                       programObject._logger.LogSuccess($"{data.EmployeeNumber} added successfully");
                   }
                   break;

               case "display":
                   programObject.Get(options.Identifier);
                   break;

               case "filter":
                   EmployeeFilter filterInput = programObject.EmployeeFilterInput();
                   List<Employee> filteredEmployeeData = programObject._employeeDal.FetchData(filterInput);
                   if (filteredEmployeeData.Count > 0)
                   {
                       programObject._logger.LogSuccess("Filtered Employees:");
                       foreach (var item in filteredEmployeeData)
                       {
                           programObject._logger.LogData("\nEmpNumber: {0}\n" + "EmpFirstName: {1}\n" + "EmpLastName: {2}\n" + "EmpDob: {3}\n" + "EmpEmailId: {4}\n" + "EmpMobileNumber: {5}\n" + "EmpJoiningDate: {6}\n" + "EmpLocation: {7}\n" + "EmpJobTitle: {8}\n" + "EmpDepartment: {9}\n" + "EmpManager: {10}\n" + "EmpProject: {11}\n", item.EmployeeNumber, item.FirstName, item.LastName, item.Dob, item.EmailId, item.MobileNumber, item.JoiningDate, item.Location, item.JobTitle, item.Department, item.Manager, item.Project);
                       }
                   }
                   else
                   {
                       programObject._logger.LogError("No Employee Found!");
                   }
                   break;

               case "delete":
                   programObject._employeeBal.Delete(options.Identifier);
                   break;

               case "help":
                   programObject.Help();
                   break;

               case "update":
                   Employee data1 = programObject.EmployeeInput();
                   List<Employee> edittedEmployeeData = programObject._employeeBal.Edit(options.Identifier, data1);
                   bool res = programObject._employeeDal.Update(edittedEmployeeData);
                   if (res)
                   {
                       programObject._logger.LogSuccess("Employee updated successfully.");
                   }
                   break;

               default:
                   programObject._logger.LogError("Invalid operation. Valid operations are: add, display, update, delete, filter, help");
                   break;
           }
       })
       .WithNotParsed(errors =>
       {
           programObject._logger.LogError("Invalid command-line arguments.");
       });
    }
}


