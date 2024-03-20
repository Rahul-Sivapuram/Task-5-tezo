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
    private static ILogger _logger;
    private static IEmployeeDal _employeeDal;
    private static IEmployeeBal _employeeBal;
    private static readonly string _filePath = "";
    private static readonly string _jobTitleJsonPath = "";
    private static readonly string _locationJsonPath = "";
    private static readonly string _managerJsonPath = "";
    private static readonly string _projectJsonPath = "";
    private static readonly string _departmentJsonPath = "";
    private static IRoleDal _roleDal;
    private static IRoleBal _roleBal;
    private static IDropDownBal _dropDownBal;
    private static IDropDownDal _dropDownDal;
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
        _logger = new ConsoleWriter();
        _filePath = _configuration["EmployeesJsonPath"];
        _jobTitleJsonPath = _configuration["JobTitleJsonPath"];
        _projectJsonPath = _configuration["ProjectJsonPath"];
        _locationJsonPath = _configuration["LocationJsonPath"];
        _departmentJsonPath = _configuration["DepartmentJsonPath"];
        _managerJsonPath = _configuration["ManagerJsonPath"];
        _employeeDal = new EmployeeDal(_logger, _filePath);
        _employeeBal = new EmployeeBal(_logger, _employeeDal);
        _roleDal = new RoleDal();
        _roleBal = new RoleBal(_roleDal, _jobTitleJsonPath, _locationJsonPath, _managerJsonPath, _projectJsonPath, _departmentJsonPath);
        _dropDownDal = new DropDownDal();
        _dropDownBal = new DropDownBal(_dropDownDal);
    }
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
       .WithParsed(options =>
       {
           if (options.Help)
           {
               var helpText = HelpText.AutoBuild(Parser.Default.ParseArguments<Options>(args));
               _logger.LogError(helpText);
               return;
           }

           switch (options.Operation.ToLower())
           {
               case "add":
                   Employee employee = GetEmployeeInput();
                   bool isAddSuccessful = _employeeBal.Add(employee);
                   if (isAddSuccessful)
                   {
                       _logger.LogSuccess(string.Format(Constants.EmployeeAddedSuccessMessage, employee.EmployeeNumber));
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
                               _logger.LogInfo(string.Format(Constants.EmployeeDetailsTemplate, item.EmployeeNumber, item.FirstName, item.LastName, item.Dob, item.EmailId, item.MobileNumber, item.JoiningDate, _dropDownBal.GetNameById<Location>(_locationJsonPath, (int)item.LocationId), _dropDownBal.GetNameById<Project>(_jobTitleJsonPath, (int)item.JobId), _dropDownBal.GetNameById<Department>(_departmentJsonPath, (int)item.DeptId), _dropDownBal.GetNameById<Manager>(_managerJsonPath, (int)item.ManagerId), _dropDownBal.GetNameById<Project>(_projectJsonPath, (int)item.ProjectId)));
                           }
                       }
                   }
                   else
                   {
                       _logger.LogError(Constants.NoEmployeeFoundMessage);
                   }
                   break;

               case "filter":
                   EmployeeFilter filterInput = GetEmployeeFilterInput();
                   List<Employee> filteredEmployeeData = _employeeBal.Filter(filterInput);
                   if (filteredEmployeeData.Count > 0)
                   {
                       foreach (var item in filteredEmployeeData)
                       {
                           _logger.LogInfo(string.Format(Constants.EmployeeDetailsTemplate, item.EmployeeNumber, item.FirstName, item.LastName, item.Dob, item.EmailId, item.MobileNumber, item.JoiningDate, _dropDownBal.GetNameById<Location>(_locationJsonPath, (int)item.LocationId), _dropDownBal.GetNameById<Project>(_jobTitleJsonPath, (int)item.JobId), _dropDownBal.GetNameById<Department>(_departmentJsonPath, (int)item.DeptId), _dropDownBal.GetNameById<Manager>(_managerJsonPath, (int)item.ManagerId), _dropDownBal.GetNameById<Project>(_projectJsonPath, (int)item.ProjectId)));
                       }
                   }
                   else
                   {
                       _logger.LogError(Constants.NoEmployeeFoundMessage);
                   }
                   break;

               case "delete":
                   bool res = _employeeBal.Delete(options.Identifier);
                   if (res)
                   {
                       _logger.LogSuccess(Constants.EmployeeDeletedSuccessMessage);
                   }
                   else
                   {
                       _logger.LogError(Constants.DeletionFailedMessage);
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
                       _logger.LogSuccess(string.Format(Constants.EmployeeUpdatedSuccessMessage, options.Identifier));
                   }
                   else
                   {
                       _logger.LogSuccess(string.Format(Constants.NoEmployeeWithIdMessage, options.Identifier));
                   }
                   break;

               case "add-role":
                   Role role = new Role();
                   List<string> roleDetails = GetRoleInput();
                   role.Name = roleDetails[0];
                   role.DepartmentId = _roleBal.GetDepartmentId(roleDetails[1], _departmentJsonPath);
                   bool sample = _roleBal.Insert(role);
                   if (sample)
                   {
                       _logger.LogSuccess(String.Format(Constants.EmployeeAddedSuccessMessage, roleDetails[0]));
                   }
                   else
                   {
                       _logger.LogError(Constants.InsertionFailed);
                   }
                   break;

               case "display-roles":
                   List<Role> roles = _roleBal.GetRoles();
                   foreach (var item in roles)
                   {
                       _logger.LogInfo(string.Format(Constants.RolesTemplate, item.Id, item.Name, _dropDownBal.GetNameById<Department>(_departmentJsonPath, (int)item.DepartmentId)));
                   }
                   break;

               default:
                   _logger.LogError(Constants.InvalidOperationMessage);
                   break;
           }
       })
       .WithNotParsed(errors =>
       {
           _logger.LogError(Constants.InvalidCommandLineArgsMessage);
       });
    }

    private static string ReadValidInput(string prompt, Func<string, bool> isValid)
    {
        string input;
        do
        {
            _logger.LogInfo(prompt);
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return null;
        } while (!isValid(input));
        return input;
    }

    private static List<string> GetRoleInput()
    {
        string roleName, departmentName;
        _logger.LogInfo("Enter RoleName: ");
        roleName = Console.ReadLine();

        _logger.LogInfo("Enter Department: ");
        departmentName = Console.ReadLine();
        return [roleName.ToUpper(), departmentName.ToUpper()];
    }

    private static int GetValidOption<T>(string promptMessage, string jsonPath)
    {
        PrintOptions(_dropDownBal.GetOptions<T>(jsonPath));
        string userInput = Console.ReadLine();
        int itemId = _dropDownBal.GetItemId<T>(userInput.ToUpper(), jsonPath);
        if (itemId == -1)
        {
            _logger.LogInfo(Constants.InvalidOptionSelected);
        }
        return itemId;
    }

    private static Employee GetEmployeeInput()
    {
        Employee employee = new Employee();
        _logger.LogInfo("Enter Employee Number: ");
        employee.EmployeeNumber = Console.ReadLine();

        _logger.LogInfo("Enter First Name: ");
        employee.FirstName = Console.ReadLine();

        _logger.LogInfo("Enter Last Name: ");
        employee.LastName = Console.ReadLine();

        _logger.LogInfo("Enter Date Of Birth:(d/m/y)");
        employee.Dob = Console.ReadLine();

        string emailId = ReadValidInput("Enter Email ID: ", s => string.IsNullOrWhiteSpace(s) || Regex.IsMatch(s, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"));
        employee.EmailId = emailId;

        long? mobileNumber = null;
        string mobileInput = ReadValidInput("Enter Mobile Number: ", s => string.IsNullOrWhiteSpace(s) || (long.TryParse(s, out var result) && s.Length == 10));
        if (!string.IsNullOrWhiteSpace(mobileInput))
            mobileNumber = long.Parse(mobileInput);

        employee.MobileNumber = mobileNumber ?? 0;

        _logger.LogInfo("Enter Joining Date: ");
        employee.JoiningDate = Console.ReadLine();

        employee.LocationId = GetValidOption<Location>("Enter location:", _locationJsonPath);
        employee.JobId = GetValidOption<Role>("Enter job title:", _jobTitleJsonPath);
        employee.DeptId = GetValidOption<Department>("Enter department name:", _departmentJsonPath);
        employee.ManagerId = GetValidOption<Manager>("Enter manager name:", _managerJsonPath);
        employee.ProjectId = GetValidOption<Project>("Enter project name:", _projectJsonPath);
        return employee;
    }

    private static EmployeeFilter GetEmployeeFilterInput()
    {
        EmployeeFilter employeeFilterObject = new EmployeeFilter();
        _logger.LogInfo("Enter an alphabet:");
        employeeFilterObject.EmployeeName = Console.ReadLine();
        _logger.LogInfo("Enter Location:");
        string locationInput = Console.ReadLine();
        employeeFilterObject.Location = _dropDownBal.GetItemByName<Location>(_locationJsonPath, locationInput.ToUpper());

        _logger.LogInfo("Enter JobTitle:");
        string jobTitleInput = Console.ReadLine();
        employeeFilterObject.JobTitle = _dropDownBal.GetItemByName<Role>(_jobTitleJsonPath, jobTitleInput.ToUpper());

        _logger.LogInfo("Enter Manager:");
        string managerInput = Console.ReadLine();
        employeeFilterObject.Manager = _dropDownBal.GetItemByName<Manager>(_managerJsonPath, managerInput.ToUpper());

        _logger.LogInfo("Enter Project:");
        string projectInput = Console.ReadLine();
        employeeFilterObject.Project = _dropDownBal.GetItemByName<Project>(_projectJsonPath, projectInput.ToUpper());

        return employeeFilterObject;
    }

    private static void Help()
    {
        _logger.LogInfo(Constants.OptionsMessage);
    }

    private static void PrintOptions<T>(IEnumerable<T> dataList)
    {
        var nameProperty = typeof(T).GetProperty("Name");

        Console.WriteLine("\nOptions:");

        foreach (var item in dataList)
        {
            _logger.LogInfo(nameProperty.GetValue(item) + "");
        }
    }
}