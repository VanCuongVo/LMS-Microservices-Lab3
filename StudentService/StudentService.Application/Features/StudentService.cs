using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentService.Application.DTOs.Request;
using StudentService.Application.DTOs.Response;
using StudentService.Application.Extensions;
using StudentService.Application.Interfaces;
using StudentService.Application.Utility;
using StudentService.Domain.Entities;
using StudentService.Domain.IUnitOfWork;

namespace StudentService.Application.Features
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<StudentResponse>> CreateAsync(CreateStudentRequest request)
        {
            var student = new Student
            {
                Dateofbirth = DateTime.SpecifyKind(request.DateOfBirth, DateTimeKind.Utc),
                Email = request.Email,
                Fullname = request.FullName,
                Age = request.Age,
                Phonenumber = request.Phonenumber,
                Studentcode = request.Studentcode
            };

            var res = await _unitOfWork.Students.AddAsync(student);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<StudentResponse>
            {
                success = true,
                message = "Create student successfully",
                Data = res.ToStudentResponse()
            };
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student != null)
            {
                await _unitOfWork.Students.DeleteAsync(student.Studentid);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse<bool>
                {
                    success = true,
                    message = "Delete student successfully",
                    Data = true
                };
            }
            return new ApiResponse<bool>
            {
                success = false,
                message = "Delete student Fails",
                Data = false
            };
        }

        public async Task<ApiResponse<object>> GetAllAsync(QueryParameters query)
        {
            var studentsQuery = _unitOfWork.Students.GetQueryable();

            studentsQuery = StudentQueryExtensions.Search(studentsQuery, query);

            var totalItems = await studentsQuery.CountAsync();

            studentsQuery = StudentQueryExtensions.Sort(studentsQuery, query);
            studentsQuery = StudentQueryExtensions.Paging(studentsQuery, query);

            var students = await studentsQuery.ToListAsync();
            var response = StudentMapperExtensions.ToStudentResponseList(students);

            // Expand enrollments if requested
            if (!string.IsNullOrEmpty(query.Expand) && query.Expand.Split(',').Any(x => x.Trim().Equals("enrollment", StringComparison.OrdinalIgnoreCase)))
            {
                foreach (var student in response)
                {
                    student.Enrollments = new List<EnrollmentResponse>();
                }
            }

            var shapeData = response.SelectFields(query.Fields);

            return new ApiResponse<object>
            {
                success = true,
                message = "Get students successfully",
                Data = shapeData,
                pagination = new PaginationMetadata
                {
                    Page = query.Page,
                    PageSize = query.Size,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / query.Size)
                }
            };
        }

        public async Task<StudentResponse?> GetByIdAsync(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null)
            {
                return null;
            }

            var response = student.ToStudentResponse();
            response.Enrollments = new List<EnrollmentResponse>();

            return response;
        }

        public async Task<ApiResponse<StudentResponse>> UpdateAsync(int id, UpdateStudentRequest request)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null)
            {
                return new ApiResponse<StudentResponse>
                {
                    success = false,
                    message = "Student not found"
                };
            }

            student.Email = request.Email;
            student.Dateofbirth = DateTime.SpecifyKind(request.DateOfBirth, DateTimeKind.Utc);
            student.Fullname = request.FullName;
            student.Age = request.Age;
            student.Phonenumber = request.Phonenumber;
            student.Studentcode = request.Studentcode;

            await _unitOfWork.Students.UpdateAsync(student);
            await _unitOfWork.SaveChangesAsync();

            var response = student.ToStudentResponse();
            response.Enrollments = new List<EnrollmentResponse>();

            return new ApiResponse<StudentResponse>
            {
                success = true,
                message = "Update student successfully",
                Data = response
            };
        }

        public async Task<ApiResponse<List<StudentResponse>>> GetStudentsBatchAsync(IEnumerable<int> ids)
        {
            var studentList = await _unitOfWork.Students.GetQueryable()
                .Where(s => ids.Contains(s.Studentid))
                .ToListAsync();

            var responseList = studentList.ToStudentResponseList();

            return new ApiResponse<List<StudentResponse>>
            {
                success = true,
                message = "Get batch of students successfully",
                Data = responseList
            };
        }
    }
}
