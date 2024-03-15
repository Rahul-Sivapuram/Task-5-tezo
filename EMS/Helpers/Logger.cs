using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement;

public class ConsoleLogger : ILogger
{
    public void LogSuccess(string message)
    {
        SetForegroundColor(ConsoleColor.Green);
        Console.WriteLine(message);
        SetForegroundColor(ConsoleColor.White);
    }

    public void LogError(string message)
    {
        SetForegroundColor(ConsoleColor.Red);
        Console.WriteLine(message);
        SetForegroundColor(ConsoleColor.White);
    }

    public void LogInfo(string message)
    {
        Console.WriteLine(message);
    }

    private void SetForegroundColor(ConsoleColor color)
    {
        Console.ForegroundColor = color;
    }
}
