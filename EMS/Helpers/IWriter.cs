using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement;
public interface IWriter
{
    void ShowSuccess(string message);
    void ShowError(string message);
    void ShowInfo(string message);
}