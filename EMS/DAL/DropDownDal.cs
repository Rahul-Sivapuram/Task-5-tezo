using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Reflection;

namespace EmployeeManagement;

public class DropDownDal:IDropDownDal
{
    public List<T> FetchData<T>(string filePath)
    {
        string jsonData = File.ReadAllText(filePath);
        List<T> data = JsonHelper.Deserialize<List<T>>(jsonData);
        return data;
    }
}
