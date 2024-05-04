using BussinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using ModelLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ModelLayer.Entity;
using Microsoft.Graph;
using System.Numerics;
using ModelLayer.Entity;
using OfficeDevPnP.Core.Entities;

namespace HospitalManagementServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorManagementController : ControllerBase
    {
        private readonly IDoctor _doctor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;


        public DoctorManagementController(IDoctor doctor, IHttpClientFactory httpClientFactory, HttpClient httpClient)
        {
            _doctor = doctor;
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;

        }

        /*[HttpGet("btdoctorid")]
        public async Task<IActionResult> GetDoctorById(int doctorid)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("userById");
                var response = await httpClient.GetAsync($"GetUserByUserId?userid={doctorid}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<UserObject>>();
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Failed to get doctor information");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting doctor information: {ex}");
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }*/

        /*[HttpGet("getallDoctor")]
        public async Task<IActionResult> GetAllDoctor()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("doctorService");
                var response = await httpClient.GetAsync("User/AllUserDoctors");


                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<User>>();
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Failed to get doctor information");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting doctor information: {ex}");
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
*/

        /*[HttpGet("getallDoctor")]
        public async Task<IActionResult> GetAllDoctor()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("doctorService");
                var response = await httpClient.GetAsync("User/AllUserDoctors");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<Doctor>>();
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Failed to get doctor information");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting doctor information: {ex}");
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }*/



        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddDoctorDetail(string deptId, DoctorEntity doctorEntity)
        {
            try
            {
                string result = await _doctor.AddDoctorDetail(deptId, doctorEntity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding doctor: {ex}");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }


        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetDoctorById(int UserId)
        {
            try
            {
                var value = await _doctor.GetDoctorById(UserId);

                if (value != null)
                {
                    var userResponse = await _httpClient.GetAsync($"https://localhost:7284/api/User/GetUserByUserId?userid={UserId}");
                    if (userResponse.IsSuccessStatusCode)
                    {
                        var responseObject = await userResponse.Content.ReadFromJsonAsync<DoctorObject>();
                        var mergeData = new
                        {
                            DoctorData = value,
                            UserData = responseObject
                        };
                        return Ok(new { Success = true, Message = "Doctor details retrieved successfully", Data = mergeData });
                    }
                    else
                    {
                        return BadRequest("Failed to get doctor information");
                    }
                }
                else
                {
                    return NotFound("Doctor not found");
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { Success = false, Message = $"An HTTP request error occurred: {ex.Message}" });
            }
            catch (JsonException ex)
            {
                return StatusCode(500, new { Success = false, Message = $"Error parsing JSON response: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = $"An unexpected error occurred: {ex.Message}" });
            }
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllDoctors()
        {
            try
            {
                var doctors = await _doctor.GetAllDoctors();
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting all doctors: {ex}");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("getallDoctor")]
        public async Task<IActionResult> GetAllDoctor()
        {
            try
            {
               
                var doctorValues = await _doctor.GetAllDoctors();
                if (doctorValues == null)
                {
                    return StatusCode(500, new { Success = false, Message = "Failed to retrieve doctor details" });
                }

                var userResponse = await _httpClient.GetAsync("https://localhost:7284/api/User/AllUserDoctors");
                if (!userResponse.IsSuccessStatusCode)
                {
                    return StatusCode(500, new { Success = false, Message = "Failed to retrieve user details" });
                }

                var content = await userResponse.Content.ReadAsStringAsync();
                var userObject = JsonSerializer.Deserialize<List<DoctorObject>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

               
                var mergeData = new
                {
                    DoctorData = doctorValues,
                    UserData = userObject
                };

                return Ok(new { Success = true, Message = "User and Doctor Details retrieved Successfully", Data = mergeData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An unexpected error occurred." });
            }
        }


        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteDoctor(int userId)
        {
            try
            {
                
                var deletedDoctor = await _doctor.DeleteDoctor(new DoctorObject { UserId = userId }, userId);

               
                if (deletedDoctor != null)
                {
                    return Ok(new { Success = true, Message = "Doctor deleted successfully", Data = deletedDoctor });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Doctor not found or already deleted" });
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"An error occurred while deleting the doctor: {ex}");
                return StatusCode(500, new { Success = false, Message = "An unexpected error occurred." });
            }
        }



    }
}








