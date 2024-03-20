using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement;

public interface IEmployeeDal
{
    List<Employee> Filter(EmployeeFilter? employee);
    List<T> FetchData<T>(string filePath);
    bool Insert(List<Employee> data);
    bool Insert(Employee data);
    bool Update(string employeeNumber, Employee employee);
    bool Delete(string employeeNumber);
}
