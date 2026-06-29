using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CourseService.Application.DTOs.Request;
using CourseService.Application.DTOs.Response;
using CourseService.Application.Interfaces;

namespace CourseService.API.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<object>>> GetAll([FromQuery] QueryParameters request)
        {
            var result = await _courseService.GetAllAsync(request);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _courseService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Course not found"
                });
            }
            return Ok(result);
        }

        [HttpGet("{id:int}/enrollments")]
        public async Task<IActionResult> GetEnrollments(int id, [FromQuery] QueryParameters query)
        {
            var result = await _courseService.GetEnrollmentsAsync(id, query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCourseRequest request)
        {
            var result = await _courseService.CreateAsync(request);
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Data!.CourseId },
                result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateCourseRequest request)
        {
            var result = await _courseService.UpdateAsync(id, request);
            if (!result.success)
            {
                return NotFound(result);
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _courseService.DeleteAsync(id);
            if (!result.success)
            {
                return NotFound(result);
            }
            return NoContent();
        }
    }
}
