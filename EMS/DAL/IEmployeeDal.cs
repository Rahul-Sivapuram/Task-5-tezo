using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement;

public interface IEmployeeDal
{
    List<EmployeeDetail> Filter(EmployeeFilter? employee);
    List<Employee> GetAll();
    List<EmployeeDetail> GetAllDetails();
    bool Insert(List<Employee> data);
    bool Insert(Employee data);
    bool Update(string employeeNumber, Employee employee);
    bool Delete(string employeeNumber);
}
