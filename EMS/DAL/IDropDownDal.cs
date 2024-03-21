using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace EmployeeManagement;

public interface IDropDownDal
{
    List<DropDown> GetLocations();
    List<DropDown> GetDepartments();
    List<DropDown> GetManagers();
    List<DropDown> GetProjects();
    bool Insert(DropDown item);
    List<DropDown> GetDropDownItems(string filePath);
}
