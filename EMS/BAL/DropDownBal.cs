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
    public int GetItemId<T>(string inputName, string filePath)
    {
        List<T> items = _dropDownDal.FetchData<T>(filePath);
        PropertyInfo nameProperty = typeof(T).GetProperty("Name");
        PropertyInfo idProperty = typeof(T).GetProperty("Id");

        if (nameProperty == null || idProperty == null)
        {
            throw new InvalidOperationException("Type T does not have 'Name' or 'Id' property.");
        }

        T item = items.FirstOrDefault(i =>
        {
            object nameValue = nameProperty.GetValue(i);
            return nameValue != null && nameValue.ToString().Equals(inputName, StringComparison.OrdinalIgnoreCase);
        });

        if (item != null)
        {
            return (int)idProperty.GetValue(item);
        }
        else
        {
            return -1;
        }
    }

    public T GetItemByName<T>(string filePath, string userInput) where T : class
    {
        List<T> items = _dropDownDal.FetchData<T>(filePath);
        PropertyInfo nameProperty = typeof(T).GetProperty("Name");
        if (nameProperty == null)
        {
            throw new InvalidOperationException("Type T does not have a 'Name' property.");
        }

        T item = items.FirstOrDefault(item => userInput.Equals(nameProperty.GetValue(item)?.ToString(), StringComparison.OrdinalIgnoreCase));
        if (item != null)
        {
            return item;
        }
        else
        {
            return null;
        }
    }

    public string GetNameById<T>(string filePath, int id)
    {
        List<T> items = _dropDownDal.FetchData<T>(filePath);
        string ans = null;
        var idProperty = typeof(T).GetProperty("Id");
        var nameProperty = typeof(T).GetProperty("Name");
        if (idProperty == null || nameProperty == null)
        {
            throw new InvalidOperationException("Type T does not have 'Id' or 'Name' property.");
        }
        foreach (var i in items)
        {
            var itemId = (int)idProperty.GetValue(i);

            if (itemId == id)
            {
                ans = (string)nameProperty.GetValue(i);
                break;
            }
        }
        return ans;
    }

    public List<T> GetOptions<T>(string filePath)
    {
        List<T> dataList = _dropDownDal.FetchData<T>(filePath);
        return dataList;
    }
}