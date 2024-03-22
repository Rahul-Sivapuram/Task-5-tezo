using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Reflection;
namespace EmployeeManagement;

public class DropDownBal : IDropDownBal
{
    private readonly IDropDownDal _dropDownDal;
    public DropDownBal(IDropDownDal dropDownDalObject)
    {
        _dropDownDal = dropDownDalObject;
    }

    public int GetLocationId(string locationName)
    {
        List<DropDown> items = _dropDownDal.GetLocations();
        return GetId(items, locationName);
    }

    public int GetManagerId(string managerName)
    {
        List<DropDown> items = _dropDownDal.GetManagers();
        return GetId(items, managerName);
    }

    public int GetProjectId(string projectName)
    {
        List<DropDown> items = _dropDownDal.GetProjects();
        return GetId(items, projectName);
    }

    public int GetDepartmentId(string departmentName)
    {
        List<DropDown> departmentList = _dropDownDal.GetDepartments();
        bool ans = departmentList.Any(department => department.Name == departmentName.ToUpper());
        if(ans){
            return GetId(departmentList,departmentName);
        }
        else{
            _dropDownDal.Insert(new DropDown { Id = departmentList.Count + 1 , Name = departmentName.ToUpper()});
            departmentList = _dropDownDal.GetDepartments();
            return GetId(departmentList,departmentName);
        } 
    }

    public DropDown GetLocationByName(string userInput)
    {
        List<DropDown> data = _dropDownDal.GetLocations();
        return GetItemByName(data, userInput);
    }

    public DropDown GetDepartmentByName(string userInput)
    {
        List<DropDown> data = _dropDownDal.GetDepartments();
        return GetItemByName(data, userInput);
    }

    public DropDown GetManagerByName(string userInput)
    {
        List<DropDown> data = _dropDownDal.GetManagers();
        return GetItemByName(data, userInput);
    }

    public DropDown GetProjectByName(string userInput)
    {
        List<DropDown> data = _dropDownDal.GetProjects();
        return GetItemByName(data, userInput);
    }

    private int GetId(List<DropDown> items, string input)
    {
        var item = items.FirstOrDefault(i => string.Equals(i.Name, input.ToUpper(), StringComparison.OrdinalIgnoreCase));
        return item?.Id ?? -1;
    }

    private DropDown GetItemByName(List<DropDown> data, string userInput)
    {
        DropDown item = data.FirstOrDefault(item => userInput.ToUpper().Equals(item.Name, StringComparison.OrdinalIgnoreCase));
        return item;
    }

    public string GetNameById(string filePath, int id)
    {
        var item = _dropDownDal.GetDropDownItems(filePath).FirstOrDefault(item => item.Id == id);
        return item?.Name;
    }
    
    public List<DropDown> GetOptions(string filePath)
    {
        List<DropDown> dataList = _dropDownDal.GetDropDownItems(filePath);
        return dataList;
    }
}
