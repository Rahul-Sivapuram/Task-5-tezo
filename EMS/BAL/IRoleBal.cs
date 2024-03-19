using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement;

public interface IRoleBal
{
    bool Insert(Role role);
    List<Role> GetRoles();
    int GetDepartmentId(string departmentName, string departmentsPath);
}
