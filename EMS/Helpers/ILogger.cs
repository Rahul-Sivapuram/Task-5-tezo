using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public interface ILogger
    {
        void LogSuccess(string message);
        void LogData(string format, params object[] args);
        void LogError(string message);
        void LogInfo(string message);
    }
}