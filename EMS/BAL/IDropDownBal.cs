using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace EmployeeManagement;

public interface IDropDownBal
{
    int GetItemId<T>(string inputName, string filePath);
    T GetItemByName<T>(string filePath, string userInput) where T : class;
    string GetNameById<T>(string filePath, int id);
    List<T> GetOptions<T>(string filePath);
}