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

            using(SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Sp_employee_All", con);
                com.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader da = com.ExecuteReader())
                {
                    while(da.Read())
                    {
                        Employee employee = new Employee();
                        employee.Id = int.Parse(da["Id"].ToString());
                        employee.Name = da["Name"].ToString();
                        employee.Age = int.Parse(da["Age"].ToString());
                        employee.Salary = decimal.Parse(da["Salary"].ToString());
                        employee.PhoneNumber = long.Parse(da["PhoneNumber"].ToString());
                        employees.Add(employee);
                    }
                }
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
            Employee emp = GetEmployeeById(id);
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand com = new SqlCommand("Sp_employee_delete", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", id);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            return emp;
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
            Employee emp = new Employee();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Sp_Employee_id", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader da = com.ExecuteReader())
                {
                    while(da.Read())
                    {
                        emp.Id = int.Parse(da["Id"].ToString());
                        emp.Name = da["Name"].ToString();
                        emp.Age = int.Parse(da["Age"].ToString());
                        emp.Salary = decimal.Parse(da["Salary"].ToString());
                        emp.PhoneNumber = long.Parse(da["PhoneNumber"].ToString());
                    }
                }
            }
            return emp;
        }
    }
}
