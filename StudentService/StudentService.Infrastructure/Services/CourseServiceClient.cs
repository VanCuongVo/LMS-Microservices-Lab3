using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StudentService.Application.DTOs.Response;
using StudentService.Application.Interfaces;

namespace StudentService.Infrastructure.Services
{
    public class CourseServiceClient : ICourseServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _courseServiceUrl;

        public CourseServiceClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _courseServiceUrl = configuration["Services:CourseServiceUrl"] ?? "http://localhost:5052";
        }

        public async Task<List<EnrollmentResponse>> GetEnrollmentsByStudentIdAsync(int studentId)
        {
            try
            {
                var requestUrl = $"{_courseServiceUrl}/api/v1/enrollments/student/{studentId}";
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<EnrollmentResponse>>>(requestUrl);
                if (response != null && response.success)
                {
                    return response.Data ?? new List<EnrollmentResponse>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling CourseService: {ex.Message}");
            }
            return new List<EnrollmentResponse>();
        }
    }
}
