using EmployeeCRUDAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeCRUDAPI.IServices
{
    public interface IEmployeeService
    {
        List<Employee> GetFromDB();
        Employee CreateEmployee(Employee emp);
        Employee DeleteEmployee(int id);
        Employee UpdateEmployee(Employee emp);
        Employee GetEmployeeById(int id);
    }
}
