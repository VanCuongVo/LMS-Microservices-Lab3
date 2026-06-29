using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CourseService.Application.DTOs.Request;
using CourseService.Application.DTOs.Response;
using CourseService.Application.Interfaces;

namespace CourseService.API.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<object>>> GetAll([FromQuery] QueryParameters request)
        {
            var result = await _enrollmentService.GetAllAsync(request);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EnrollmentResponse>> GetById(int id)
        {
            var result = await _enrollmentService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(new
                {
                    Message = $"Enrollment with id {id} not found"
                });
            }
            return Ok(result);
        }

        [HttpGet("student/{studentId:int}")]
        public async Task<ActionResult<ApiResponse<List<EnrollmentResponse>>>> GetByStudentId(int studentId)
        {
            var result = await _enrollmentService.GetByStudentIdAsync(studentId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EnrollmentResponse>>> Create([FromBody] CreateEnrollmentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<EnrollmentResponse>
                    {
                        success = false,
                        message = "Invalid request"
                    });
                }

                var result = await _enrollmentService.CreateAsync(request);
                if (!result.success)
                {
                    return BadRequest(result);
                }

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.Data?.EnrollmentId },
                    result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<EnrollmentResponse>
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse<EnrollmentResponse?>>> Update(int id, [FromBody] UpdateEnrollmentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<EnrollmentResponse?>
                    {
                        success = false,
                        message = "Invalid request"
                    });
                }

                var result = await _enrollmentService.UpdateAsync(id, request);
                if (!result.success)
                {
                    return NotFound(result);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<EnrollmentResponse?>
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
