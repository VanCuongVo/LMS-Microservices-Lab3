using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentService.Application.DTOs.Request;
using StudentService.Application.DTOs.Response;
using StudentService.Application.Interfaces;

namespace StudentService.API.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<object>>> GetAll([FromQuery] QueryParameters query)
        {
            var result = await _studentService.GetAllAsync(query);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _studentService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Student with id {id} not found" });
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentRequest request)
        {
            try
            {
                var result = await _studentService.CreateAsync(request);
                if (!result.success)
                {
                    return BadRequest(result);
                }

                return CreatedAtAction(nameof(GetById), new { id = result.Data!.StudentId }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentRequest request)
        {
            try
            {
                var result = await _studentService.UpdateAsync(id, request);
                if (!result.success)
                {
                    return NotFound(result);
                }
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("batch")]
        public async Task<ActionResult<ApiResponse<List<StudentResponse>>>> GetBatch([FromQuery] int[] ids)
        {
            var result = await _studentService.GetStudentsBatchAsync(ids);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _studentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
