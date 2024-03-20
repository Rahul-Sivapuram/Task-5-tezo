using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeManagement;

public class EmployeeDal : IEmployeeDal
{
    private readonly string _filePath;
    public EmployeeDal(string path)
    {
        _filePath = path;
    }
    public List<T> FetchData<T>(string filePath)
    {
        string jsonData = File.ReadAllText(filePath);
        List<T> employeeData = JsonHelper.Deserialize<List<T>>(jsonData);
        return employeeData;
    }

    public bool Insert(Employee employee)
    {
        List<Employee> employeeData = FetchData<Employee>(_filePath);
        employeeData.Add(employee);
        string jsonUpdatedData = JsonHelper.Serialize<Employee>(employeeData);
        JsonHelper.Save(_filePath, jsonUpdatedData);
        return true;
    }

    public bool Insert(List<Employee> employeeData)
    {
        string jsonUpdatedData = JsonHelper.Serialize<Employee>(employeeData);
        JsonHelper.Save(_filePath, jsonUpdatedData);
        return true;
    }

    public bool Update(string employeeNumber, Employee employee)
    {
        List<Employee> employeeData = FetchData<Employee>(_filePath);
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
        List<Employee> employeeData = FetchData<Employee>(_filePath);
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

    public List<Employee> Filter(EmployeeFilter? employee)
    {
        List<Employee> employeeData = FetchData<Employee>(_filePath);
        var filteredEmployees = employeeData.Where(emp => IsEmployeeFiltered(emp, employee)).ToList();
        return filteredEmployees;
    }
    
    private bool IsEmployeeFiltered(Employee emp, EmployeeFilter employee)
    {
        bool filterEmployeeName = string.IsNullOrEmpty(employee.EmployeeName) || emp.FirstName.StartsWith(employee.EmployeeName);
        bool filterLocation = employee.Location == null || emp.LocationId == employee.Location.Id;
        bool filterJobTitle = employee.JobTitle == null || emp.JobId == employee.JobTitle.Id;
        bool filterManager = employee.Manager == null || emp.ManagerId == employee.Manager.Id;
        bool filterProject = employee.Project == null || emp.ProjectId == employee.Project.Id;
        return filterEmployeeName && filterLocation && filterJobTitle && filterManager && filterProject;
    }
}
