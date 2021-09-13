using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using EmployeeCRUDAPI.Models;
using EmployeeCRUDAPI.Services;
using EmployeeCRUDAPI.IServices;

namespace EmployeeCRUDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeservice)
        {
            this.employeeService = employeeservice;
        }

        [HttpGet]
        [Route("GET")]
        public List<Employee> GetEmployees()
        {
              return employeeService.GetFromDB();
        }

        [HttpGet]
        [Route("GETByID")]
        public Employee GetEmployeesByID(int id)
        {
            return employeeService.GetEmployeeById(id);
        }

        [HttpPost]
        [Route("AddEmployee")]
        public Employee AddEmployee(Employee emp)
        {
            return employeeService.CreateEmployee(emp);
        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        public Employee DeleteEmployee(int id)
        {
            return employeeService.DeleteEmployee(id);
        }

        [HttpPut]
        [Route("UpdateEmployee")]
        public Employee UpdateEmployee(Employee emp)
        {
            return employeeService.UpdateEmployee(emp);
        }
    }
}
