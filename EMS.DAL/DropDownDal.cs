using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Reflection;
using EMS.Common;
namespace EMS.DAL;

public class DropDownDal : IDropDownDal
{
    private readonly JsonHelper _jsonHelper;
    private readonly IConfigurationRoot _configuration;
    public DropDownDal(IConfigurationRoot configurationObject, JsonHelper jsonHelperObject)
    {   
        _configuration = configurationObject;
        _jsonHelper = jsonHelperObject;
    }
    
    private List<DropDown> GetDropDownItems(string filePath)
    {
        string jsonData = File.ReadAllText(filePath);
        List<DropDown> data = _jsonHelper.Deserialize<List<DropDown>>(jsonData);
        return data;
    }

    public List<DropDown> GetLocations()
    {
        return GetDropDownItems(Path.Combine(_configuration["BasePath"], _configuration["LocationJsonPath"]));
    }

    public List<DropDown> GetDepartments()
    {
        return GetDropDownItems(Path.Combine(_configuration["BasePath"], _configuration["DepartmentJsonPath"]));
    }

    public List<DropDown> GetManagers()
    {
        return GetDropDownItems(Path.Combine(_configuration["BasePath"], _configuration["ManagerJsonPath"]));
    }

    public List<DropDown> GetProjects()
    {
        return GetDropDownItems(Path.Combine(_configuration["BasePath"], _configuration["ProjectJsonPath"]));
    }
}