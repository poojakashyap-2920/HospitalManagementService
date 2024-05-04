using Dapper;
using Microsoft.Extensions.Configuration;
using ModelLayer.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public class DoctorService : IDoctor
    {
        private readonly HospitalContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public DoctorService(HospitalContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory, HttpClient httpClient)
        {
            _context = context;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
        }

        public async Task<string> AddDoctorDetail(string deptId, DoctorEntity doctorEntity)
        {
            try
            {
                string insertQuery = @"
            INSERT INTO Doctors (DeptId, DoctorAge, DoctorAvailability, DoctorQualifications, DoctorSpecialty)
            VALUES (@DeptId, @DoctorAge, @DoctorAvailability, @DoctorQualifications, @DoctorSpecialty);";

                var parameters = new DynamicParameters();
                parameters.Add("@DeptId", deptId, DbType.String);
                parameters.Add("@DoctorAge", doctorEntity.DoctorAge, DbType.Int32);
                parameters.Add("@DoctorAvailability", doctorEntity.DoctorAvailability, DbType.Boolean);
                parameters.Add("@DoctorQualifications", doctorEntity.DoctorQualifications, DbType.String);
                parameters.Add("@DoctorSpecialty", doctorEntity.DoctorSpecialty, DbType.String);

                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(insertQuery, parameters);
                }

                return "Doctor inserted successfully.";
            }
            catch (Exception ex)
            {
                // Handle exception
                return "Error: " + ex.Message;
            }
        }

        public async Task<List<DoctorEntity>> GetDoctorById(int UserId)
        {
            var query = "SELECT * FROM Doctors WHERE UserId = @UserId";

            using (var connection = _context.CreateConnection())
            {
                var doctors = await connection.QueryAsync<DoctorEntity>(query, new { UserId });
                return doctors.ToList();
            }
        }

       /* public async Task<List<DoctorEntity>> GetAllDoctorDetails()
        {
            var query = "SELECT * FROM Doctors";

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var doctors = await connection.QueryAsync<DoctorEntity>(query);
                    return doctors.ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"An error occurred while getting all doctors: {ex}");
                throw; // Rethrow the exception to propagate it to the caller
            }
        }*/







        public Task GetUserByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<DoctorEntity>> GetAllDoctor(DoctorEntity doctorEntity)
        {
            throw new NotImplementedException();
        }

        public  async Task<List<DoctorEntity>> GetAllDoctors()
        {
            var query = "SELECT * FROM Doctors";

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var doctors = await connection.QueryAsync<DoctorEntity>(query);
                    return doctors.ToList();
                }
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"An error occurred while getting all doctors: {ex}");
                throw; 
            }
        }


        //delete



        public async Task<DoctorEntity> DeleteDoctor(DoctorObject doctorObject, int userId)
        {
            try
            {
              
                var userResponse = await _httpClient.GetAsync($"https://localhost:7284/api/User/GetUserById?userId={userId}");
                if (!userResponse.IsSuccessStatusCode)
                {
                   
                    var errorMessage = await userResponse.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to retrieve user details: {errorMessage}");
                }

                var content = await userResponse.Content.ReadAsStringAsync();
                var userObject = JsonSerializer.Deserialize<UserObject>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                
                if (userObject == null)
                {
                   
                    throw new Exception("User not found");
                }

                
                var deleteQuery = "DELETE FROM Doctors WHERE UserId = @UserId";
                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(deleteQuery, new { UserId = userId });
                }

                var deletedDoctor = new DoctorEntity
                {
                    UserId = userId,
                    DeptId = doctorObject.DeptId,
                    DoctorAge = doctorObject.DoctorAge,
                    DoctorAvailability = doctorObject.DoctorAvailability,
                    DoctorQualifications = doctorObject.DoctorQualifications,
                    DoctorSpecialty = doctorObject.DoctorSpecialty
                };

               
                return deletedDoctor;
            }
            catch (HttpRequestException ex)
            {
                
                Console.WriteLine($"An error occurred while fetching user details: {ex.Message}");
                throw new Exception("Failed to retrieve user details", ex);
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"An error occurred while deleting the doctor: {ex}");
                throw; 
            }
        }





    }
}

