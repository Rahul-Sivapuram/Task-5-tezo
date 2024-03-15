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

public static class Program
{
    private static IConfigurationRoot _configuration;
    private static ILogger _consoleWrite;
    private static IEmployeeDal _employeeDal;
    private static IEmployeeBal _employeeBal;
    private static readonly string _filePath = "";

    static Program()
    {
        _configuration = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .Build();
        _consoleWrite = new ConsoleLogger();
        _filePath = _configuration["EmployeesJsonPath"];
        _employeeDal = new EmployeeDal(_consoleWrite, _filePath);
        _employeeBal = new EmployeeBal(_consoleWrite, _employeeDal);
    }

    private static int GetEnumValue<TEnum>(string prompt) where TEnum : struct,Enum
    {
        _consoleWrite.LogInfo(prompt);
        string input = Console.ReadLine();
        TEnum enumValue;
        if (Enum.TryParse(input, out enumValue))
        {
            return Convert.ToInt32(enumValue);
        }
        else
        {
            _consoleWrite.LogError($"Invalid input for {typeof(TEnum).Name}!");
            return -1;
        }
    }
    private static Employee GetEmployeeInput()
    {
        Employee employee = new Employee();
        long? mobileNumber = null;

        _consoleWrite.LogInfo("Enter Employee Number: ");
        employee.EmployeeNumber = Console.ReadLine();

        _consoleWrite.LogInfo("Enter First Name: ");
        employee.FirstName = Console.ReadLine();

        _consoleWrite.LogInfo("Enter Last Name: ");
        employee.LastName = Console.ReadLine();

        _consoleWrite.LogInfo("Enter Date Of Birth:(d/m/y)");
        employee.Dob = Console.ReadLine();

        _consoleWrite.LogInfo("Enter Email ID: ");
        string emailId = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(emailId))
        {
            while (!Regex.IsMatch(emailId, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                _consoleWrite.LogError("Invalid email format. Please enter a valid email address or leave blank to skip.");
                emailId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(emailId))
                {
                    break;
                }
            }
        }
        employee.EmailId = emailId;

        _consoleWrite.LogInfo("Enter Mobile Number: ");
        string mobileInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(mobileInput))
        {
            while (!(long.TryParse(mobileInput, out _) && mobileInput.Length == 10))
            {
                _consoleWrite.LogError("Invalid input for Mobile Number. Please enter a valid mobile number or leave blank to skip.");
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
        employee.MobileNumber = mobileNumber ?? 0;

        _consoleWrite.LogInfo("Enter Joining Date: ");
        employee.JoiningDate = Console.ReadLine();

        employee.LocationId = GetEnumValue<Location>("Enter Location: ");
        employee.JobId = GetEnumValue<JobTitle>("Enter Job Title: ");
        employee.DeptId = GetEnumValue<Department>("Enter Department: ");
        employee.ManagerId = GetEnumValue<Manager>("Enter Assigned Manager: ");
        employee.ProjectId = GetEnumValue<Project>("Enter Assigned Project: ");
        return employee;
    }

    private static EmployeeFilter GetEmployeeFilterInput()
    {
        EmployeeFilter employeeFilterObject = new EmployeeFilter();
        _consoleWrite.LogInfo("Enter an alphabet:");
        employeeFilterObject.EmployeeName = Console.ReadLine();

        _consoleWrite.LogInfo("Enter Location:");
        string locationInput = Console.ReadLine();
        if (Enum.TryParse(locationInput, true, out Location location))
        {
            employeeFilterObject.Location = location;
        }
        else
        {
            employeeFilterObject.Location = null;

        }

        _consoleWrite.LogInfo("Enter JobTitle:");
        string jobTitleInput = Console.ReadLine();
        if (Enum.TryParse(jobTitleInput, true, out JobTitle jobTitle))
        {
            employeeFilterObject.JobTitle = jobTitle;
        }
        else
        {
            employeeFilterObject.JobTitle = null;

        }

        _consoleWrite.LogInfo("Enter Manager:");
        string managerInput = Console.ReadLine();
        if (Enum.TryParse(managerInput, true, out Manager manager))
        {
            employeeFilterObject.Manager = manager;
        }
        else
        {
            employeeFilterObject.Manager = null;

        }


        _consoleWrite.LogInfo("Enter Project:");
        string projectInput = Console.ReadLine();
        if (Enum.TryParse(projectInput, true, out Project project))
        {
            employeeFilterObject.Project = project;
        }
        else
        {
            employeeFilterObject.Project = null;

        }

        return employeeFilterObject;
    }

    private static void Get(string? employeeNumber)
    {
        List<Employee> employees = _employeeDal.FetchData();
        if (employees.Count > 0 && String.IsNullOrEmpty(employeeNumber))
        {

            foreach (var item in employees)
            {
                _consoleWrite.LogInfo(string.Format(Constants.EmployeeDetailsTemplate,
                item.EmployeeNumber, item.FirstName, item.LastName, item.Dob, item.EmailId,
                item.MobileNumber, item.JoiningDate, Enum.GetName(typeof(Location), item.LocationId), Enum.GetName(typeof(JobTitle), item.JobId),
                Enum.GetName(typeof(Department), item.DeptId), Enum.GetName(typeof(Manager), item.ManagerId), Enum.GetName(typeof(Project), item.ProjectId)));
            }
        }
        else if (employees.Count > 0 && !string.IsNullOrEmpty(employeeNumber))
        {
            foreach (var item in employees)
            {
                if (item.EmployeeNumber == employeeNumber)
                {
                    _consoleWrite.LogInfo(string.Format(Constants.EmployeeDetailsTemplate,
                item.EmployeeNumber, item.FirstName, item.LastName, item.Dob, item.EmailId,
                item.MobileNumber, item.JoiningDate, Enum.GetName(typeof(Location), item.LocationId), Enum.GetName(typeof(JobTitle), item.JobId),
                Enum.GetName(typeof(Department), item.DeptId), Enum.GetName(typeof(Manager), item.ManagerId), Enum.GetName(typeof(Project), item.ProjectId)));
                }
            }
        }
        else
        {
            _consoleWrite.LogError(Constants.messages[5]);
        }
    }

    private static void Help()
    {
        Console.WriteLine("Options");
        Console.WriteLine("add    -    To add an employee");
        Console.WriteLine("display -  To display all employee details");
        Console.WriteLine("search  -    To display a particular employee data");
        Console.WriteLine("delete  -    To delete an employee based on given employeenumber");
        Console.WriteLine("update  -    To update employee details based on given employeenumber");
    }

    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
       .WithParsed(options =>
       {
           if (options.Help)
           {
               var helpText = HelpText.AutoBuild(Parser.Default.ParseArguments<Options>(args));
               _consoleWrite.LogError(helpText);
               return;
           }

           switch (options.Operation.ToLower())
           {
               case "add":
                   Employee employee = GetEmployeeInput();
                   bool isAddSuccessful = _employeeBal.Add(employee);
                   if (isAddSuccessful)
                   {
                       _consoleWrite.LogSuccess(string.Format(Constants.messages[7], employee.EmployeeNumber));
                   }
                   break;

               case "display":
                   Get(options.Identifier);
                   break;

               case "filter":
                   EmployeeFilter filterInput = GetEmployeeFilterInput();
                   List<Employee> filteredEmployeeData = _employeeDal.FetchData(filterInput);
                   if (filteredEmployeeData.Count > 0)
                   {
                       foreach (var item in filteredEmployeeData)
                       {
                           _consoleWrite.LogInfo(string.Format(Constants.EmployeeDetailsTemplate,
                            item.EmployeeNumber, item.FirstName, item.LastName, item.Dob, item.EmailId,
                            item.MobileNumber, item.JoiningDate, Enum.GetName(typeof(Location), item.LocationId), Enum.GetName(typeof(JobTitle), item.JobId),
                            Enum.GetName(typeof(Department), item.DeptId), Enum.GetName(typeof(Manager), item.ManagerId), Enum.GetName(typeof(Project), item.ProjectId)));
                       }
                   }
                   else
                   {
                       _consoleWrite.LogError(Constants.messages[5]);
                   }
                   break;

               case "delete":
                   bool res = _employeeBal.Delete(options.Identifier);
                   if (res)
                   {
                       _consoleWrite.LogSuccess(Constants.messages[0]);
                   }
                   else
                   {
                       _consoleWrite.LogError(Constants.messages[1]);
                   }
                   break;

               case "help":
                   Help();
                   break;

               case "update":
                   Employee employeeToUpdate = GetEmployeeInput();
                   bool isUpdated = _employeeBal.Update(options.Identifier, employeeToUpdate);
                   if (isUpdated)
                   {
                       _consoleWrite.LogSuccess(string.Format(Constants.messages[4], options.Identifier));
                   }
                   else
                   {
                       _consoleWrite.LogSuccess(string.Format(Constants.messages[3], options.Identifier));
                   }
                   break;

               default:
                   _consoleWrite.LogError(Constants.messages[8]);
                   break;
           }
       })
       .WithNotParsed(errors =>
       {
           _consoleWrite.LogError(Constants.messages[9]);
       });
    }
}




/*

        _consoleWrite.LogInfo("Enter Location: ");
        location = Console.ReadLine();
        Location locationEnum;
        if (Enum.TryParse(location, out locationEnum))
        {
            locationID = (int)locationEnum;
        }
        else
        {
            _consoleWrite.LogError("Invalid location input!");
        }

        _consoleWrite.LogInfo("Enter Job Title: ");
        jobTitle = Console.ReadLine();
        JobTitle jobTitleEnum;
        if (Enum.TryParse(jobTitle, out jobTitleEnum))
        {
            jobID = (int)jobTitleEnum;
        }
        else
        {
            _consoleWrite.LogError("Invalid job title input!");
        }

        _consoleWrite.LogInfo("Enter Department: ");
        department = Console.ReadLine();
        Department departmentEnum;
        if (Enum.TryParse(department, out departmentEnum))
        {
            deptID = (int)departmentEnum;

        }
        else
        {
            _consoleWrite.LogError("Invalid department input!");
        }

        _consoleWrite.LogInfo("Enter Assigned Manager: ");
        manager = Console.ReadLine();
        Manager managerEnum;
        if (Enum.TryParse(location, out managerEnum))
        {
            managerID = (int)managerEnum;
        }
        else
        {
            _consoleWrite.LogError("Invalid location input!");
        }

        _consoleWrite.LogInfo("Enter Assigned Project: ");
        project = Console.ReadLine();
        Project projectEnum;
        if (Enum.TryParse(location, out projectEnum))
        {
            projectID = (int)projectEnum;
        }
        else
        {
            _consoleWrite.LogError("Invalid location input!");
        }
*/