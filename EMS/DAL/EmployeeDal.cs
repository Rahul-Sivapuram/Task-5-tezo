using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeManagement;
public class EmployeeDal : IEmployeeDal
{
    private readonly ILogger _logger;
    private readonly string _filePath;

    public EmployeeDal(ILogger loggerObject, string path)
    {
        _logger = loggerObject;
        _filePath = path;
    }

    public List<Employee> FetchData(EmployeeFilter? employeeFilterInput)
    {
        Console.WriteLine(employeeFilterInput);
        List<Employee> employeeData = FetchData();
        var filteredEmployees = employeeData.Where(emp =>
           (string.IsNullOrEmpty(employeeFilterInput.EmployeeName) || emp.FirstName.StartsWith(employeeFilterInput.EmployeeName)) &&
            (!employeeFilterInput.Location.HasValue || emp.Location == employeeFilterInput.Location.ToString()) &&
            (!employeeFilterInput.JobTitle.HasValue  || emp.JobTitle == employeeFilterInput.JobTitle.ToString()) &&
            (string.IsNullOrEmpty(employeeFilterInput.Manager) || emp.Manager == employeeFilterInput.Manager) &&
            (string.IsNullOrEmpty(employeeFilterInput.Project) || emp.Project == employeeFilterInput.Project)
        ).ToList();

        return filteredEmployees;
    }
    
    public List<Employee> FetchData()
    {
        string jsonData = File.ReadAllText(_filePath);
        List<Employee> employeeData = JDeSerialize(jsonData);
        _logger.LogSuccess("Data fetched successfully");
        return employeeData;
    }

    public bool Insert(List<Employee> data)
    {
        string jsonUpdatedData = JSerialize(data);
        File.WriteAllText(_filePath, jsonUpdatedData);
        return true;
    }

    public bool Update(List<Employee> data)
    {
        string jsonUpdatedData = JSerialize(data);
        File.WriteAllText(_filePath, jsonUpdatedData);
        return true;
    }

    private List<Employee> JDeSerialize(string jsonData)
    {
        return JsonSerializer.Deserialize<List<Employee>>(jsonData);
    }

    private string JSerialize(List<Employee> data)
    {
        return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
    }
}
