using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EmployeeManagement;

public class EmployeeBal : IEmployeeBal
{
    private readonly IEmployeeDal _employeeDal;
    private readonly ILogger _consolWrite;

    public EmployeeBal(ILogger loggerObject, IEmployeeDal employeeDalObject)
    {
        _employeeDal = employeeDalObject;
        _consolWrite = loggerObject;
    }
    
    public bool Add(Employee employee)
    {
        return _employeeDal.Insert(employee);
    }

    public bool Delete(string employeeNumber)
    {
        return _employeeDal.Delete(employeeNumber);
    }

    public bool Update(string empNumber, Employee employeeInput)
    {
        bool res=_employeeDal.Update(empNumber, employeeInput);
        return res;
    }
}

