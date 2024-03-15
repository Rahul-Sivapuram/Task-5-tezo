using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement;

public interface IEmployeeDal
{
    List<Employee> FetchData(EmployeeFilter? employeeFilterInput);

    List<Employee> FetchData();

    bool Insert(List<Employee> data);

    bool Insert(Employee data);

    bool Update(string empNumber, Employee employeeInput);

    bool Delete(string employeeNumber);

}
