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

    public bool Add(Employee employee)
    {
        return _employeeDal.Insert(employee);
    }

    public bool Delete(string employeeNumber)
    {
        return _employeeDal.Delete(employeeNumber);
    }

    public bool Update(string employeeNumber, Employee employee)
    {
        bool res = _employeeDal.Update(employeeNumber, employee);
        return res;
    }

    public List<Employee> Filter(EmployeeFilter employee)
    {
        return _employeeDal.Filter(employee);
    }

    public List<Employee> Display(string employeeNumber)
    {
        List<Employee> employeeData = _employeeDal.FetchData();
        if (employeeNumber == null)
        {
            return employeeData;
        }
        else
        {
            foreach (Employee employee in employeeData)
            {
                if (employee.EmployeeNumber == employeeNumber)
                {
                    return new List<Employee>() { employee };
                }
            }
        }
        return employeeData;
    }
}

