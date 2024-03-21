using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement;

public interface IRoleBal
{
    bool Insert(Role role);
    List<string> GetRoleNamesForDepartment(int id);
    int GetRoleId(string roleName);
    Role GetRoleByName(string userInput);
    List<Role> Get();
}
