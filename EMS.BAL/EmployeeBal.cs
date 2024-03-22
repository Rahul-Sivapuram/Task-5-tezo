using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Reflection;
using EMS.DAL;
namespace EMS.BAL;

public class EmployeeBal : IEmployeeBal
{
    private readonly IEmployeeDal _employeeDal;
    public EmployeeBal(IEmployeeDal employeeDalObject)
    {
        _employeeDal = employeeDalObject;
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

    public List<EmployeeDetail> Filter(EmployeeFilter employee)
    {
        return _employeeDal.Filter(employee);
    }

    public List<EmployeeDetail> Get(string employeeNumber)
    {
        List<EmployeeDetail> employeeData = _employeeDal.GetAllDetails();
        if (employeeNumber == null)
        {
            return employeeData;
        }
        else
        {
            return employeeData.Where(e => e.EmployeeNumber == employeeNumber).ToList();
        }
        return employeeData;
    }
}
