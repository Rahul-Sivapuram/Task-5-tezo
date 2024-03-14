using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement;

public interface IEmployeeBal
{
    List<Employee> Add(Employee employeeInput);
    void Delete(string employeeNumber);
    List<Employee> Edit(string empNumber, Employee formData);
}
