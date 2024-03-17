using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeManagement;

public class EmployeeDal : IEmployeeDal
{
    private readonly ILogger _consoleWriter;
    private readonly string _filePath;
    public EmployeeDal(ILogger loggerObject, string path)
    {
        _consoleWriter = loggerObject;
        _filePath = path;
    }
    public List<Employee> Filter(EmployeeFilter? employee)
    {
        List<Employee> employeeData = FetchData();
        var filteredEmployees = employeeData.Where(emp => IsEmployeeFiltered(emp, employee)).ToList();
        return filteredEmployees;
    }

    public List<Employee> FetchData()
    {
        string jsonData = File.ReadAllText(_filePath);
        List<Employee> employeeData = DeserializeEmployees(jsonData);
        return employeeData;
    }

    public bool Insert(Employee employee)
    {
        List<Employee> employeeData = FetchData();
        employeeData.Add(employee);
        string jsonUpdatedData = SerializeEmployees(employeeData);
        Save(_filePath, jsonUpdatedData);
        return true;
    }

    public bool Insert(List<Employee> employeeData)
    {
        string jsonUpdatedData = SerializeEmployees(employeeData);
        Save(_filePath, jsonUpdatedData);
        return true;
    }

    public bool Update(string employeeNumber, Employee employee)
    {
        List<Employee> employeeData = FetchData();
        bool found = false;
        foreach (var item in employeeData)
        {
            if (item.EmployeeNumber == employeeNumber)
            {
                found = true;
                item.EmployeeNumber ??= employee.EmployeeNumber;
                item.FirstName ??= employee.FirstName;
                item.LastName ??= employee.LastName;
                item.Dob ??= employee.Dob;
                item.EmailId ??= employee.EmailId;
                if (employee.MobileNumber != 0)
                {
                    item.MobileNumber = employee.MobileNumber;
                }
                item.JoiningDate ??= employee.JoiningDate;
                if (employee.LocationId != -1)
                {
                    item.LocationId = employee.LocationId;
                }
                if (employee.JobId != -1)
                {
                    item.JobId = employee.JobId;
                }
                if (employee.DeptId != -1)
                {
                    item.DeptId = employee.DeptId;
                }
                if (employee.ManagerId != -1)
                {
                    item.ManagerId = employee.ManagerId;
                }
                if (employee.ProjectId != -1)
                {
                    item.ProjectId = employee.ProjectId;
                }
            }
        }
        bool res = Insert(employeeData);
        return found;
    }

    public bool Delete(string employeeNumber)
    {
        List<Employee> employeeData = FetchData();
        var employeeToRemove = employeeData.FirstOrDefault(e => e.EmployeeNumber == employeeNumber);
        if (employeeToRemove != null)
        {
            employeeData.Remove(employeeToRemove);
            bool res = Insert(employeeData);
            return res;
        }
        else
        {
            return false;
        }
    }

    private List<Employee> DeserializeEmployees(string jsonData)
    {
        return JsonSerializer.Deserialize<List<Employee>>(jsonData);
    }

    private string SerializeEmployees(List<Employee> data)
    {
        return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
    }

    private bool IsEmployeeFiltered(Employee emp, EmployeeFilter employee)
    {
        bool filterEmployeeName = string.IsNullOrEmpty(employee.EmployeeName) ||
                                   emp.FirstName.StartsWith(employee.EmployeeName);
        bool filterLocation = !employee.Location.HasValue ||
                              emp.LocationId == Convert.ToInt32(employee.Location);
        bool filterJobTitle = !employee.JobTitle.HasValue ||
                              emp.JobId == Convert.ToInt32(employee.JobTitle);
        bool filterManager = !employee.Manager.HasValue ||
                             emp.ManagerId == Convert.ToInt32(employee.Manager);
        bool filterProject = !employee.Project.HasValue ||
                             emp.ProjectId == Convert.ToInt32(employee.Project);
        return filterEmployeeName && filterLocation && filterJobTitle && filterManager && filterProject;
    }

    private void Save(string filePath, string content)
    {
        File.WriteAllText(filePath, content);
    }
}
