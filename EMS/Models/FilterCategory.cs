using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement;

public class EmployeeFilter
{
    public Location? Location { get; set; }
    public JobTitle? JobTitle { get; set; }
    public Department? Department { get; set; }
    public string? Manager { get; set; }
    public string? Project { get; set; }
    public string? EmployeeName { get; set; }
}
