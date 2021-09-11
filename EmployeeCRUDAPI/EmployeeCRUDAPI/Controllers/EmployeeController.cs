using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using EmployeeCRUDAPI.Models;

namespace EmployeeCRUDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GET")]
        public List<Employee> GetEmployees()
        {
            return GetFromDB();
        }

        [HttpGet]
        [Route("GETByID")]
        public List<Employee> GetEmployeesByID(int id)
        {
            return GetFromDB().Where(e => e.Id == id).ToList();
        }

        [HttpPost]
        [Route("AddEmployee")]
        public string CreateEmployee(Employee emp)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand com = new SqlCommand("Sp_employee_Add", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Name", emp.Name);
            com.Parameters.AddWithValue("@Age", emp.Age);
            com.Parameters.AddWithValue("@Salary", emp.Salary);
            com.Parameters.AddWithValue("@PhoneNumber", emp.PhoneNumber);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            return "Employee added successfully ";
        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        public string DeleteEmployee(int id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand com = new SqlCommand("Sp_employee_delete", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", id);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            return "Item Deleted successfully";
        }

        [HttpPut]
        [Route("UpdateEmployee")]
        public string UpdateEmployee(Employee emp)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand com = new SqlCommand("Sp_employee_update", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", emp.Id);
            com.Parameters.AddWithValue("@Name", emp.Name);
            com.Parameters.AddWithValue("@Age", emp.Age);
            com.Parameters.AddWithValue("@Salary", emp.Salary);
            com.Parameters.AddWithValue("@PhoneNumber", emp.PhoneNumber);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            con.Dispose();

            return "Employee Updated successfully";
        }

        private List<Employee> GetFromDB()
        {
            List<Employee> employees = new List<Employee>();

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand com = new SqlCommand("select * from Employees", con);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for(int i =0; i < dt.Rows.Count; i++)
            {
                Employee obj = new Employee();
                obj.Id = int.Parse(dt.Rows[i]["Id"].ToString());
                obj.Name = dt.Rows[i]["Name"].ToString();
                obj.Age = int.Parse(dt.Rows[i]["Age"].ToString());
                obj.Salary =decimal.Parse(dt.Rows[i]["Salary"].ToString());
                obj.PhoneNumber = long.Parse(dt.Rows[i]["PhoneNumber"].ToString());
                employees.Add(obj);
            }

            return employees;
        }
    }
}
