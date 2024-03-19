using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement;
public class RoleBal : IRoleBal
{
    private readonly IRoleDal _roleDal;
    private readonly string rolesPath, locationsPath, managersPath, projectsPath, departmentsPath;
    public RoleBal(IRoleDal roleDalObject, string jobTitleJsonPath, string locationJsonPath, string managerJsonPath, string projectJsonPath, string departmentJsonPath)
    {
        _roleDal = roleDalObject;
        rolesPath = jobTitleJsonPath;
        locationsPath = locationJsonPath;
        managersPath = managerJsonPath;
        projectsPath = projectJsonPath;
        departmentsPath = departmentJsonPath;
    }

    public bool Insert(Role role)
    {
        bool res = false;
        List<Role> jobTitleList = _roleDal.FetchRoleData<Role>(rolesPath);

        if (!jobTitleList.Exists(item => item.Name == role.Name))
        {
            role.Id = jobTitleList.Count + 1;
            _roleDal.Insert(role, rolesPath);
            res = true;
        }
        return res;
    }

    public int GetDepartmentId(string departmentName, string departmentsPath)
    {
        List<Department> departmentList = _roleDal.FetchRoleData<Department>(departmentsPath);

        if (!departmentList.Exists(item => item.Name == departmentName))
        {
            _roleDal.Insert(new Department { Id = departmentList.Count + 1, Name = departmentName }, departmentsPath);
            departmentList = _roleDal.FetchRoleData<Department>(departmentsPath);
        }

        return departmentList.FirstOrDefault(item => item.Name == departmentName)?.Id ?? -1;
    }
    
    public List<Role> GetRoles()
    {
        List<Role> roles = _roleDal.FetchRoleData<Role>(rolesPath);
        return roles;
    }
}