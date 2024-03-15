using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement;

public static class Constants
{
    public static readonly string EmployeeDetailsTemplate =
        "EmpNumber: {0}\n" +
        "EmpFirstName: {1}\n" +
        "EmpLastName: {2}\n" +
        "EmpDob: {3}\n" +
        "EmpEmailId: {4}\n" +
        "EmpMobileNumber: {5}\n" +
        "EmpJoiningDate: {6}\n" +
        "EmpLocation: {7}\n" +
        "EmpJobTitle: {8}\n" +
        "EmpDepartment: {9}\n" +
        "EmpManager: {10}\n" +
        "EmpProject: {11}\n";

    public static readonly List<string> messages = [
        "Employee deleted successfully.",
        "Deletion Failed",
        "Employee not found.",
        "No employee found with EmployeeID: {0}",
        "{0} updated successfully",
        "No Employee Found!",
        "Options\n" + "add \t - \t To add an employee\n" + "display - \t To display all employee details\n" + "search \t - \t To display a particular employee data\n" + "delete \t - \t To delete an employee based on given employeenumber\n" + "update \t - \t To update employee details based on given employeenumber\n",
        "{0} added successfully",
        "Invalid operation. Valid operations are: add, display, update, delete, filter, help",
        "Invalid command-line arguments."
        ];

}