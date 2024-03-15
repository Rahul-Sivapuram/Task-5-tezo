using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeManagement;

public class EmployeeDal : IEmployeeDal
{
    private readonly ILogger _consolWrite;
    private readonly string _filePath;

    public EmployeeDal(ILogger loggerObject, string path)
    {
        _consolWrite = loggerObject;
        _filePath = path;
    }

    public List<Employee> FetchData(EmployeeFilter? employeeFilterInput)
    {
     
        List<Employee> employeeData = FetchData();
        var filteredEmployees = employeeData.Where(emp =>IsEmployeeFiltered(emp, employeeFilterInput)).ToList();

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
        File.WriteAllText(_filePath, jsonUpdatedData);
        return true;
    }

    public bool Insert(List<Employee> employeeData)
    {
        string jsonUpdatedData = SerializeEmployees(employeeData);
        File.WriteAllText(_filePath, jsonUpdatedData);
        return true;
    }

    public bool Update(string employeeNumber, Employee employeeInput)
    {
        List<Employee> employeeData = FetchData();
        bool found = false;
        foreach (var item in employeeData)
        {
            if (item.EmployeeNumber == employeeNumber)
            {
                found = true;
                item.EmployeeNumber ??= employeeInput.EmployeeNumber;
                item.FirstName ??= employeeInput.FirstName;
                item.LastName ??= employeeInput.LastName;
                item.Dob ??= employeeInput.Dob;
                item.EmailId ??= employeeInput.EmailId;
                if (employeeInput.MobileNumber != 0)
                {
                    item.MobileNumber = employeeInput.MobileNumber;
                }
                item.JoiningDate ??= employeeInput.JoiningDate;
                if (employeeInput.LocationId != -1)
                {
                    item.LocationId = employeeInput.LocationId;
                }
                if (employeeInput.JobId != -1)
                {
                    item.JobId = employeeInput.JobId;
                }
                if (employeeInput.DeptId != -1)
                {
                    item.DeptId = employeeInput.DeptId;
                }
                if (employeeInput.ManagerId != -1)
                {
                    item.ManagerId = employeeInput.ManagerId;
                }
                if (employeeInput.ProjectId != -1)
                {
                    item.ProjectId = employeeInput.ProjectId;
                }
            }
        }
        bool res = Insert(employeeData);
        return found;
    }

    public bool Delete(string employeeNumber)
    {
        List<Employee> employeeData = FetchData();
        int index = -1;
        for (int i = 0; i < employeeData.Count; i++)
        {
            if (employeeData[i].EmployeeNumber == employeeNumber)
            {
                index = i;
                break;
            }
        }
        if (index != -1)
        {
            employeeData.RemoveAt(index);
            bool res = Insert(employeeData);
            return res;
        }
        else
        {
            _consolWrite.LogError(Constants.messages[2]);
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

    private bool IsEmployeeFiltered(Employee emp, EmployeeFilter employeeFilterInput)
    {
        bool filterEmployeeName = string.IsNullOrEmpty(employeeFilterInput.EmployeeName) ||
                                   emp.FirstName.StartsWith(employeeFilterInput.EmployeeName);
        bool filterLocation = !employeeFilterInput.Location.HasValue ||
                              emp.LocationId == Convert.ToInt32(employeeFilterInput.Location);
        bool filterJobTitle = !employeeFilterInput.JobTitle.HasValue ||
                              emp.JobId == Convert.ToInt32(employeeFilterInput.JobTitle);
        bool filterManager = !employeeFilterInput.Manager.HasValue ||
                             emp.ManagerId == Convert.ToInt32(employeeFilterInput.Manager);
        bool filterProject = !employeeFilterInput.Project.HasValue ||
                             emp.ProjectId == Convert.ToInt32(employeeFilterInput.Project);
        return filterEmployeeName && filterLocation && filterJobTitle && filterManager && filterProject;
    }
}
