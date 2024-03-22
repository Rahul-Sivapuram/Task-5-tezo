using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Reflection;
using EMS.Common;
namespace EMS.DAL;

public class DropDownDal : IDropDownDal
{
    private readonly string rolesPath, locationsPath, managersPath, projectsPath, departmentsPath;
    private readonly JsonHelper _jsonHelper;
    public DropDownDal(string jobTitleJsonPath, string locationJsonPath, string managerJsonPath, string projectJsonPath, string departmentJsonPath,JsonHelper jsonHelperObject)
    {
        rolesPath = jobTitleJsonPath;
        locationsPath = locationJsonPath;
        managersPath = managerJsonPath;
        projectsPath = projectJsonPath;
        departmentsPath = departmentJsonPath;
        _jsonHelper = jsonHelperObject;
    }
    
    public List<DropDown> GetDropDownItems(string filePath)
    {
        string jsonData = File.ReadAllText(filePath);
        List<DropDown> data = _jsonHelper.Deserialize<List<DropDown>>(jsonData);
        return data;
    }

    public List<DropDown> GetLocations()
    {
        return GetDropDownItems(locationsPath);
    }

    public List<DropDown> GetDepartments()
    {
        return GetDropDownItems(departmentsPath);
    }

    public List<DropDown> GetManagers()
    {
        return GetDropDownItems(managersPath);
    }

    public List<DropDown> GetProjects()
    {
        return GetDropDownItems(projectsPath);
    }

    public bool Insert(DropDown item)
    {
        List<DropDown> data = GetDepartments();
        data.Add(item);
        _jsonHelper.Save(departmentsPath, data);
        return true;
    }
}
