using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement;
public class RoleDal:IRoleDal
{
    public List<T> FetchRoleData<T>(string _filePath)
    {
        string jsonData = File.ReadAllText(_filePath);
        List<T> roles= JsonHelper.Deserialize<List<T>>(jsonData);
        return roles;
    }
    public bool Insert<T>(T role,string filePath)
    {
        List<T> roles = FetchRoleData<T>(filePath);
        roles.Add(role);
        string jsonUpdatedData = JsonHelper.Serialize<T>(roles);
        JsonHelper.Save(filePath, jsonUpdatedData);
        return true;
    }
}