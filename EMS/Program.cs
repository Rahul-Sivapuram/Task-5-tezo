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
    private static ILogger _consoleWriter;
    private static IEmployeeDal _employeeDal;
    private static IEmployeeBal _employeeBal;
    private static readonly string _filePath = "";
    private static IConfigurationRoot GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }
    static Program()
    {
        _configuration = GetConfiguration();
        _consoleWriter = new ConsoleWriter();
        _filePath = _configuration["EmployeesJsonPath"];
        _employeeDal = new EmployeeDal(_consoleWriter, _filePath);
        _employeeBal = new EmployeeBal(_consoleWriter, _employeeDal);
    }
      public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
       .WithParsed(options =>
       {
           if (options.Help)
           {
               var helpText = HelpText.AutoBuild(Parser.Default.ParseArguments<Options>(args));
               _consoleWriter.LogError(helpText);
               return;
           }

           switch (options.Operation.ToLower())
           {
               case "add":
                   Employee employee = GetEmployeeInput();
                   bool isAddSuccessful = _employeeBal.Add(employee);
                   if (isAddSuccessful)
                   {
                       _consoleWriter.LogSuccess(string.Format(Constants.EmployeeAddedSuccessMessage, employee.EmployeeNumber));
                   }
                   break;

               case "display":
                   List<Employee> employees = _employeeBal.Display(options.Identifier);
                   if (employees.Count > 0)
                   {
                       foreach (var item in employees)
                       {
                           if (string.IsNullOrEmpty(options.Identifier) || item.EmployeeNumber == options.Identifier)
                           {
                               _consoleWriter.LogInfo(string.Format(Constants.EmployeeDetailsTemplate,
                                   item.EmployeeNumber, item.FirstName, item.LastName, item.Dob, item.EmailId,
                                   item.MobileNumber, item.JoiningDate, Enum.GetName(typeof(Location), item.LocationId),
                                   Enum.GetName(typeof(JobTitle), item.JobId), Enum.GetName(typeof(Department), item.DeptId),
                                   Enum.GetName(typeof(Manager), item.ManagerId), Enum.GetName(typeof(Project), item.ProjectId)));
                           }
                       }
                   }
                   else
                   {
                       _consoleWriter.LogError(Constants.NoEmployeeFoundMessage);
                   }
                   break;

               case "filter":
                   EmployeeFilter filterInput = GetEmployeeFilterInput();
                   List<Employee> filteredEmployeeData = _employeeBal.Filter(filterInput);
                   if (filteredEmployeeData.Count > 0)
                   {
                       foreach (var item in filteredEmployeeData)
                       {
                           _consoleWriter.LogInfo(string.Format(Constants.EmployeeDetailsTemplate,
                            item.EmployeeNumber, item.FirstName, item.LastName, item.Dob, item.EmailId,
                            item.MobileNumber, item.JoiningDate, Enum.GetName(typeof(Location), item.LocationId), Enum.GetName(typeof(JobTitle), item.JobId),
                            Enum.GetName(typeof(Department), item.DeptId), Enum.GetName(typeof(Manager), item.ManagerId), Enum.GetName(typeof(Project), item.ProjectId)));
                       }
                   }
                   else
                   {
                       _consoleWriter.LogError(Constants.NoEmployeeFoundMessage);
                   }
                   break;

               case "delete":
                   bool res = _employeeBal.Delete(options.Identifier);
                   if (res)
                   {
                       _consoleWriter.LogSuccess(Constants.EmployeeDeletedSuccessMessage);
                   }
                   else
                   {
                       _consoleWriter.LogError(Constants.DeletionFailedMessage);
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
                       _consoleWriter.LogSuccess(string.Format(Constants.EmployeeUpdatedSuccessMessage, options.Identifier));
                   }
                   else
                   {
                       _consoleWriter.LogSuccess(string.Format(Constants.NoEmployeeWithIdMessage, options.Identifier));
                   }
                   break;

               default:
                   _consoleWriter.LogError(Constants.InvalidOperationMessage);
                   break;
           }
       })
       .WithNotParsed(errors =>
       {
           _consoleWriter.LogError(Constants.InvalidCommandLineArgsMessage);
       });
    }

    private static string ReadValidInput(string prompt, Func<string, bool> isValid)
    {
        string input;
        do
        {
            _consoleWriter.LogInfo(prompt);
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return null;
        } while (!isValid(input));
        return input;
    }

    private static int GetEnumValue<TEnum>(string prompt) where TEnum : struct, Enum
    {
        _consoleWriter.LogInfo(prompt);
        string input = Console.ReadLine();
        TEnum enumValue;
        if (Enum.TryParse(input, out enumValue))
        {
            return Convert.ToInt32(enumValue);
        }
        else
        {
            _consoleWriter.LogError($"Invalid input for {typeof(TEnum).Name}!");
            return -1;
        }
    }

    private static Employee GetEmployeeInput()
    {
        Employee employee = new Employee();
        _consoleWriter.LogInfo("Enter Employee Number: ");
        employee.EmployeeNumber = Console.ReadLine();

        _consoleWriter.LogInfo("Enter First Name: ");
        employee.FirstName = Console.ReadLine();

        _consoleWriter.LogInfo("Enter Last Name: ");
        employee.LastName = Console.ReadLine();

        _consoleWriter.LogInfo("Enter Date Of Birth:(d/m/y)");
        employee.Dob = Console.ReadLine();

        string emailId = ReadValidInput("Enter Email ID: ", s => string.IsNullOrWhiteSpace(s) || Regex.IsMatch(s, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"));
        employee.EmailId = emailId;

        long? mobileNumber = null;
        string mobileInput = ReadValidInput("Enter Mobile Number: ", s => string.IsNullOrWhiteSpace(s) || (long.TryParse(s, out var result) && s.Length == 10));
        if (!string.IsNullOrWhiteSpace(mobileInput))
            mobileNumber = long.Parse(mobileInput);

        employee.MobileNumber = mobileNumber ?? 0;

        _consoleWriter.LogInfo("Enter Joining Date: ");
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
        _consoleWriter.LogInfo("Enter an alphabet:");
        employeeFilterObject.EmployeeName = Console.ReadLine();
        _consoleWriter.LogInfo("Enter Location:");
        string locationInput = Console.ReadLine();
        if (Enum.TryParse(locationInput, true, out Location location))
        {
            employeeFilterObject.Location = location;
        }
        else
        {
            employeeFilterObject.Location = null;
        }

        _consoleWriter.LogInfo("Enter JobTitle:");
        string jobTitleInput = Console.ReadLine();
        if (Enum.TryParse(jobTitleInput, true, out JobTitle jobTitle))
        {
            employeeFilterObject.JobTitle = jobTitle;
        }
        else
        {
            employeeFilterObject.JobTitle = null;
        }

        _consoleWriter.LogInfo("Enter Manager:");
        string managerInput = Console.ReadLine();
        if (Enum.TryParse(managerInput, true, out Manager manager))
        {
            employeeFilterObject.Manager = manager;
        }
        else
        {
            employeeFilterObject.Manager = null;
        }

        _consoleWriter.LogInfo("Enter Project:");
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

    private static void Help()
    {
        _consoleWriter.LogInfo(Constants.messages[6]);
    }
}