using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace EmployeeManagement;

public interface IDropDownBal
{
    int GetLocationId(string locationName);
    int GetManagerId(string managerName);
    int GetProjectId(string projectName);
    int GetDepartmentId(string departmentName);
    DropDown GetLocationByName(string userInput);
    DropDown GetDepartmentByName(string userInput);
    DropDown GetManagerByName(string userInput);
    DropDown GetProjectByName(string userInput);
    string GetNameById(string filePath, int id);
    List<DropDown> GetOptions(string filePath);
}