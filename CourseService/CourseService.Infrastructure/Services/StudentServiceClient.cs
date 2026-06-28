using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using CourseService.Application.DTOs.Response;
using CourseService.Application.Interfaces;

namespace CourseService.Infrastructure.Services
{
    public class StudentServiceClient : IStudentServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _studentServiceUrl;

        public StudentServiceClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _studentServiceUrl = configuration["Services:StudentServiceUrl"] ?? "http://localhost:5051";
        }

        public async Task<StudentInEnrollmentResponse?> GetStudentByIdAsync(int studentId)
        {
            try
            {
                var requestUrl = $"{_studentServiceUrl}/api/v1/students/{studentId}";
                var response = await _httpClient.GetFromJsonAsync<StudentResponse>(requestUrl);
                if (response != null)
                {
                    return new StudentInEnrollmentResponse
                    {
                        StudentId = response.StudentId,
                        FullName = response.FullName ?? string.Empty,
                        Email = response.Email ?? string.Empty,
                        PhoneNumber = response.PhoneNumber,
                        Age = response.Age,
                        StudentCode = response.StudentCode
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling StudentService (GetById): {ex.Message}");
            }
            return null;
        }

        public async Task<List<StudentInEnrollmentResponse>> GetStudentsByIdsAsync(IEnumerable<int> studentIds)
        {
            try
            {
                // Construct batch query parameter: ?ids=1&ids=2&...
                var queryParams = string.Join("&", studentIds.Select(id => $"ids={id}"));
                var requestUrl = $"{_studentServiceUrl}/api/v1/students/batch?{queryParams}";
                
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<StudentResponse>>>(requestUrl);
                if (response != null && response.success && response.Data != null)
                {
                    return response.Data.Select(x => new StudentInEnrollmentResponse
                    {
                        StudentId = x.StudentId,
                        FullName = x.FullName ?? string.Empty,
                        Email = x.Email ?? string.Empty,
                        PhoneNumber = x.PhoneNumber,
                        Age = x.Age,
                        StudentCode = x.StudentCode
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling StudentService (GetBatch): {ex.Message}");
            }
            return new List<StudentInEnrollmentResponse>();
        }

        public async Task<List<StudentInEnrollmentResponse>> SearchStudentsAsync(string searchTerm)
        {
            try
            {
                var requestUrl = $"{_studentServiceUrl}/api/v1/students?search={Uri.EscapeDataString(searchTerm)}";
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<StudentResponse>>>(requestUrl);
                if (response != null && response.success && response.Data != null)
                {
                    return response.Data.Select(x => new StudentInEnrollmentResponse
                    {
                        StudentId = x.StudentId,
                        FullName = x.FullName ?? string.Empty,
                        Email = x.Email ?? string.Empty,
                        PhoneNumber = x.PhoneNumber,
                        Age = x.Age,
                        StudentCode = x.StudentCode
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling StudentService (Search): {ex.Message}");
            }
            return new List<StudentInEnrollmentResponse>();
        }
        
        // Helper model for deserialization
        private class StudentResponse
        {
            public int StudentId { get; set; }
            public string? FullName { get; set; }
            public string? Email { get; set; }
            public string? PhoneNumber { get; set; }
            public int Age { get; set; }
            public string? StudentCode { get; set; }
        }
    }
}
