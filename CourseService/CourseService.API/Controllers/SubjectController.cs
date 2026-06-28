using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourseService.Application.DTOs.Request;
using CourseService.Application.DTOs.Response;
using CourseService.Application.Interfaces;

namespace CourseService.API.Controllers
{
    [ApiController]
    [Route("api/v1/subjects")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] QueryParameters query)
        {
            var result = await _subjectService.GetAllAsync(query);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _subjectService.GetByIdAysnc(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateSubjectRequest request)
        {
            var result = await _subjectService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.SubjectId }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateSubjectRequest request)
        {
            var result = await _subjectService.UpdateAsync(id, request);
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
            var result = await _subjectService.DeleteAsync(id);
            if (!result.success)
            {
                return NotFound(result);
            }
            return NoContent();
        }
    }
}
