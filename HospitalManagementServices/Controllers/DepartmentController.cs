using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Threading.Tasks;

namespace HospitalManagementServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartment _dept;

        public DepartmentController(IDepartment dept)
        {
            _dept = dept;
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment(DepartmentEntity departmentDto)
        {
            try
            {
                string result = await _dept.AddDepartment(departmentDto);
                return Ok(result); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartment()
        {
            try
            {
                var users = await _dept.GetAllDepartment();
                return Ok(users);
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"An error occurred while retrieving all users: {ex}");
                return StatusCode(500, "An error occurred while retrieving all department details");
            }
        }

        [HttpGet("{deptId}")]
        public async Task<IActionResult> GetDepartmentbyDeptId(string deptId)
        {
            try
            {
                var department = await _dept.GetDepartmentbyDeptId(deptId);
                if (department == null)
                    return NotFound();

                return Ok(department);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, ex.Message);
            }
        }

    }
}
