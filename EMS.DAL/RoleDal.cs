using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common;
namespace EMS.DAL;

public class RoleDal : IRoleDal
{
    private readonly string _filePath;
    private readonly JsonHelper _jsonHelper;
    public RoleDal(string path,JsonHelper jsonHelperObject)
    {   
        _filePath = path;
        _jsonHelper = jsonHelperObject;
    }

    public List<Role> GetAll()
    {
        string jsonData = File.ReadAllText(_filePath);
        List<Role> roles = _jsonHelper.Deserialize<List<Role>>(jsonData);
        return roles;
    }

    public bool Insert(Role role)
    {
        List<Role> roles = GetAll();
        roles.Add(role);
        _jsonHelper.Save(_filePath, roles);
        return true;
    }

}