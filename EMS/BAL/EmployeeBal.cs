using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EmployeeManagement;
public class EmployeeBal : IEmployeeBal
{
    private readonly IEmployeeDal _employeeDal;
    private readonly ILogger _logger;

    public EmployeeBal(ILogger loggerObject, IEmployeeDal employeeDalObject)
    {
        _employeeDal = employeeDalObject;
        _logger = loggerObject;
    }
    
    public List<Employee> Add(Employee data)
    {
        List<Employee> employees = _employeeDal.FetchData();
        employees.Add(data);
        return employees;
    }

    public void Delete(string employeeNumber)
    {
        List<Employee> employeeData = _employeeDal.FetchData();
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
            bool res = _employeeDal.Insert(employeeData);
            string msg=res ? "Employee deleted successfully." : "Deletion Failed";
            _logger.LogSuccess(msg);
            
        }
        else
        {
            _logger.LogError("Employee not found.");
        }
    }

    public List<Employee> Edit(string empNumber, Employee formData)
    {
        List<Employee> employeeData = _employeeDal.FetchData();
        bool found = false;
        foreach (var item in employeeData)
        {
            if (item.EmployeeNumber == empNumber)
            {
                found = true;
                item.EmployeeNumber ??= formData.EmployeeNumber;
                item.FirstName ??= formData.FirstName;
                item.LastName ??= formData.LastName;
                item.Dob ??= formData.Dob;
                item.EmailId ??= formData.EmailId;
                if (formData.MobileNumber != 0)
                {
                    item.MobileNumber = formData.MobileNumber;
                }
                item.JoiningDate ??= formData.JoiningDate;
                item.Location ??= formData.Location;
                item.JobTitle ??= formData.JobTitle;
                item.Department ??= formData.Department;
                item.Manager ??= formData.Manager;
                item.Project ??= formData.Project;
            }
        }
        if (!found)
        {
            _logger.LogError($"No employee found with EmployeeID: {empNumber}");
        }
        else
        {
            _logger.LogSuccess($"{empNumber} updated successfully");
        }
        return employeeData;
    }
}

