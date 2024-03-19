using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement;

public interface IRoleDal
{
    List<T> FetchRoleData<T>(string _filePath);
    bool Insert<T>(T role,string filePath);
}
