using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseService.Application.DTOs.Request;
using CourseService.Application.DTOs.Response;
using CourseService.Application.Extensions;
using CourseService.Application.Interfaces;
using CourseService.Application.Utility;
using CourseService.Domain.Entities;
using CourseService.Domain.IUnitOfWork;

namespace CourseService.Application.Features
{
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<SubjectResponse>> CreateAsync(CreateSubjectRequest request)
        {
            var subject = new Subject
            {
                Subjectname = request.SubjectName,
                Subjectcode = request.SubjectCode,
                Credit = request.Credit
            };

            var res = await _unitOfWork.Subjects.AddAsync(subject);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<SubjectResponse>
            {
                success = true,
                message = "Create subjects successfully",
                Data = res.ToSubjectResponse()
            };
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var subject = await _unitOfWork.Subjects.GetByIdAsync(id);
            if (subject != null)
            {
                await _unitOfWork.Subjects.DeleteAsync(subject.Subjectid);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse<bool>
                {
                    success = true,
                    message = "Delete subjects successfully"
                };
            }

            return new ApiResponse<bool>
            {
                success = false,
                message = "Delete subjects Fails"
            };
        }

        public async Task<ApiResponse<IEnumerable<SubjectResponse>>> GetAllAsync(QueryParameters query)
        {
            IQueryable<Subject> subjectQuery = _unitOfWork.Subjects.GetQueryable();

            subjectQuery = SubjectQueryExtensions.Search(subjectQuery, query);
            int totalItems = await subjectQuery.CountAsync();

            subjectQuery = SubjectQueryExtensions.Sort(subjectQuery, query);
            subjectQuery = SubjectQueryExtensions.Paging(subjectQuery, query);

            List<Subject> subjects = await subjectQuery.ToListAsync();
            var response = SubjectMapperExtensions.ToSubjectResponseList(subjects);

            return new ApiResponse<IEnumerable<SubjectResponse>>
            {
                success = true,
                message = "Get subjects successfully",
                Data = response,
                pagination = new PaginationMetadata
                {
                    PageSize = query.Size,
                    Page = query.Page,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / query.Size)
                }
            };
        }

        public async Task<SubjectResponse?> GetByIdAysnc(int id)
        {
            var subject = await _unitOfWork.Subjects.GetByIdAsync(id);
            if (subject == null)
            {
                return null;
            }

            return subject.ToSubjectResponse();
        }

        public async Task<ApiResponse<SubjectResponse>> UpdateAsync(int id, UpdateSubjectRequest request)
        {
            var subject = await _unitOfWork.Subjects.GetByIdAsync(id);
            if (subject == null)
            {
                return new ApiResponse<SubjectResponse>
                {
                    success = false,
                    message = "Subject Not Found"
                };
            }

            subject.Subjectcode = request.SubjectCode;
            subject.Credit = request.Credit;
            subject.Subjectname = request.SubjectName;

            await _unitOfWork.Subjects.UpdateAsync(subject);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<SubjectResponse>
            {
                success = true,
                message = "Update subjects successfully",
                Data = subject.ToSubjectResponse()
            };
        }
    }
}
