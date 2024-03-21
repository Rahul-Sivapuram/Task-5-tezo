using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeManagement;

public class EmployeeDal : IEmployeeDal
{
    private readonly string _filePath;
    private readonly JsonHelper _jsonHelper;
    private readonly IDropDownBal _dropDownBal;
    private readonly string rolesPath, locationsPath, managersPath, projectsPath, departmentsPath;
    public EmployeeDal(string path, JsonHelper jsonHelperObject, IDropDownBal dropDownBalObject, string jobTitleJsonPath, string locationJsonPath, string managerJsonPath, string projectJsonPath, string departmentJsonPath)
    {
        _filePath = path;
        _jsonHelper = jsonHelperObject;
        _dropDownBal = dropDownBalObject;
        rolesPath = jobTitleJsonPath;
        locationsPath = locationJsonPath;
        managersPath = managerJsonPath;
        projectsPath = projectJsonPath;
        departmentsPath = departmentJsonPath;
    }

    public List<Employee> GetAll()
    {
        string jsonData = File.ReadAllText(_filePath);
        List<Employee> employeeData = _jsonHelper.Deserialize<List<Employee>>(jsonData);
        return employeeData;
    }

    public List<EmployeeDetail> GetAllDetails()
    {
        List<Employee> employeeData = GetAll();
        List<EmployeeDetail> employeeDetailsList = new List<EmployeeDetail>();
        foreach (var employee in employeeData)
        {
            EmployeeDetail employeeDetail = new EmployeeDetail
            {
                EmployeeNumber = employee.EmployeeNumber,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Dob = employee.Dob,
                EmailId = employee.EmailId,
                MobileNumber = employee.MobileNumber,
                JoiningDate = employee.JoiningDate,
                LocationName = _dropDownBal.GetNameById(locationsPath, (int)employee.LocationId),
                JobName = _dropDownBal.GetNameById(rolesPath, (int)employee.JobId),
                DeptName = _dropDownBal.GetNameById(departmentsPath, (int)employee.DeptId),
                ManagerName = _dropDownBal.GetNameById(managersPath, (int)employee.ManagerId),
                ProjectName = _dropDownBal.GetNameById(projectsPath, (int)employee.ProjectId)
            };
            employeeDetailsList.Add(employeeDetail);
        }
        return employeeDetailsList;
    }

    public bool Insert(Employee employee)
    {
        List<Employee> employeeData = GetAll();
        employeeData.Add(employee);
        _jsonHelper.Save(_filePath, employeeData);
        return true;
    }

    public bool Insert(List<Employee> employeeData)
    {
        _jsonHelper.Save(_filePath, employeeData);
        return true;
    }

    public bool Update(string employeeNumber, Employee employee)
    {
        List<Employee> employeeData = GetAll();
        Employee existingEmployee = employeeData.FirstOrDefault(e => e.EmployeeNumber == employeeNumber);
        bool found = existingEmployee != null;
        if (found)
        {
            existingEmployee.EmployeeNumber ??= employee.EmployeeNumber;
            existingEmployee.FirstName ??= employee.FirstName;
            existingEmployee.LastName ??= employee.LastName;
            existingEmployee.Dob ??= employee.Dob;
            existingEmployee.EmailId ??= employee.EmailId;
            if (employee.MobileNumber != 0)
            {
                existingEmployee.MobileNumber = employee.MobileNumber;
            }
            existingEmployee.JoiningDate ??= employee.JoiningDate;
            if (employee.LocationId != -1)
            {
                existingEmployee.LocationId = employee.LocationId;
            }
            if (employee.JobId != -1)
            {
                existingEmployee.JobId = employee.JobId;
            }
            if (employee.DeptId != -1)
            {
                existingEmployee.DeptId = employee.DeptId;
            }

            if (employee.ManagerId != -1)
            {
                existingEmployee.ManagerId = employee.ManagerId;
            }
            if (employee.ProjectId != -1)
            {
                existingEmployee.ProjectId = employee.ProjectId;
            }
        }
        bool res = Insert(employeeData);
        return found;
    }

    public bool Delete(string employeeNumber)
    {
        List<Employee> employeeData = GetAll();
        var employeeToRemove = employeeData.FirstOrDefault(e => e.EmployeeNumber == employeeNumber);
        if (employeeToRemove != null)
        {
            employeeData.Remove(employeeToRemove);
            bool res = Insert(employeeData);
            return res;
        }
        else
        {
            return false;
        }
    }

    public List<EmployeeDetail> Filter(EmployeeFilter? employee)
    {
        List<EmployeeDetail> employeeData = GetAllDetails();
        var filteredEmployees = employeeData.Where(emp => IsEmployeeFiltered(emp, employee)).ToList();
        return filteredEmployees;
    }

    private bool IsEmployeeFiltered(EmployeeDetail emp, EmployeeFilter employee)
    {
        bool filterEmployeeName = string.IsNullOrEmpty(employee.EmployeeName) || emp.FirstName.StartsWith(employee.EmployeeName);
        bool filterLocation = employee.Location == null || emp.LocationName == employee.Location.Name;
        bool filterJobTitle = employee.JobTitle == null || emp.JobName == employee.JobTitle.Name;
        bool filterManager = employee.Manager == null || emp.ManagerName == employee.Manager.Name;
        bool filterProject = employee.Project == null || emp.ProjectName == employee.Project.Name;
        return filterEmployeeName && filterLocation && filterJobTitle && filterManager && filterProject;
    }
}
