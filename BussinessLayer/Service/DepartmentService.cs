using Dapper;
using Microsoft.Extensions.Configuration;
using ModelLayer.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public class DepartmentService : IDepartment
    {

        private readonly HospitalContext _context;
        private readonly IConfiguration _configuration;
        public DepartmentService(HospitalContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<string> AddDepartment(DepartmentEntity departmentEntity)
        {
            try
            {
                string insertQuery = @"
                    INSERT INTO Departments (DeptId, DeptName, SpecialtyDepartment)
                    VALUES (@DeptId, @DeptName, @SpecialtyDepartment);";

                var parameters = new DynamicParameters();
                parameters.Add("@DeptId", departmentEntity.DeptId, DbType.Int32);
                parameters.Add("@DeptName", departmentEntity.DeptName, DbType.String);
                parameters.Add("@SpecialtyDepartment", departmentEntity.SpecialtyDepartment, DbType.String);

                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(insertQuery, parameters);
                }

                return "Department inserted successfully.";
            }
            catch (Exception ex)
            {
                // Handle exception
                return "Error: " + ex.Message;
            }
        }

       /* public Task DeleteDepartment(DepartmentEntity departmentEntity)
        {
            throw new NotImplementedException();
        }*/

        public  async Task<List<DepartmentEntity>> GetAllDepartment()
        {
            try
            {
                var query = "SELECT * FROM Departments";
                using (var connection = _context.CreateConnection())
                {
                    var users = await connection.QueryAsync<DepartmentEntity>(query);
                    return users.ToList();
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposess
                Console.WriteLine($"Error occurred in GetAllUsers: {ex}");

                // Rethrow the exception to propagate it further
                throw;
            }
        }

        public async Task<IEnumerable<DepartmentEntity>> GetDepartmentbyDeptId(string deptid)
        {
            try
            {
                // Query to select the department with the specified ID from the Departments table
                var query = "SELECT * FROM Departments WHERE DeptId = @DeptId";

                // Execute the query asynchronously
                using (var connection = _context.CreateConnection())
                {
                    // Query the database for the department with the specified ID
                    var departments = await connection.QueryAsync<DepartmentEntity>(query, new { DeptId = deptid });

                    // Check if any department is found
                    if (departments.Any())
                    {
                        return departments;
                    }
                    else
                    {
                        // Throw an exception with a custom message indicating that the department with the specified ID is not available
                        throw new Exception($"Department with ID '{deptid}' does not exist.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error occurred in GetDepartmentbyDeptId: {ex}");

                // Ret  hrow the exception to propagate it further
                throw;
            }
        }


    }
}

