using EmployeeCRUDAPI.IServices;
using EmployeeCRUDAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeCRUDAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IConfiguration _configuration;

        public EmployeeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Employee> GetFromDB()
        {
            List<Employee> employees = new List<Employee>();

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand com = new SqlCommand("Sp_employee_All", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Employee employee = new Employee();
                employee.Id = int.Parse(dt.Rows[i]["Id"].ToString());
                employee.Name = dt.Rows[i]["Name"].ToString();
                employee.Age = int.Parse(dt.Rows[i]["Age"].ToString());
                employee.Salary = decimal.Parse(dt.Rows[i]["Salary"].ToString());
                employee.PhoneNumber = long.Parse(dt.Rows[i]["PhoneNumber"].ToString());
                employees.Add(employee);
            }

            return employees;
        }
        public Employee CreateEmployee(Employee emp)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand com = new SqlCommand("Sp_employee_Add", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Name", emp.Name);
            com.Parameters.AddWithValue("@Age", emp.Age);
            com.Parameters.AddWithValue("@Salary", emp.Salary);
            com.Parameters.AddWithValue("@PhoneNumber", emp.PhoneNumber);
            com.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            emp.Id = int.Parse(com.Parameters["@Id"].Value.ToString());
            return emp;
        }
        public Employee DeleteEmployee(int id)
        {
            Employee employee = GetEmployeeById(id);
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand com = new SqlCommand("Sp_employee_delete", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", id);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            return employee;
        }
        public Employee UpdateEmployee(Employee emp)
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

            return emp;
        }

        public Employee GetEmployeeById(int id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand com = new SqlCommand("Sp_Employee_id", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", id);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Employee employee = new Employee();
            employee.Id = int.Parse(dt.Rows[0]["Id"].ToString());
            employee.Name = dt.Rows[0]["Name"].ToString();
            employee.Age = int.Parse(dt.Rows[0]["Age"].ToString());
            employee.Salary = decimal.Parse(dt.Rows[0]["Salary"].ToString());
            employee.PhoneNumber = long.Parse(dt.Rows[0]["PhoneNumber"].ToString());

            return employee;
        }
    }
}
