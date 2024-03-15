using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement;

public interface IEmployeeBal
{
    bool Add(Employee employee);
    bool Delete(string employeeNumber);
    bool Update(string employeeNumber, Employee employeeInput);
}
