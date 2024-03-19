using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
namespace EmployeeManagement;
public static class JsonHelper
{
    public static string Serialize<T>(List<T> data)
    {
        return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
    }

    public static T Deserialize<T>(string jsonData)
    {
        return JsonSerializer.Deserialize<T>(jsonData);
    }

    public static void Save(string filePath, string content)
    {
        File.WriteAllText(filePath, content);
    }
}