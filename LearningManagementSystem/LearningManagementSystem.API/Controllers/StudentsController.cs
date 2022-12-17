﻿using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(StudentCreateModel model)
        {
            var res = await _studentService.AddAsync(model);
            return res.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _studentService.GetAll());
        }

        [HttpGet("withoutgroups")]
        public async Task<IActionResult> GetStudentsWithoutGroups()
        {
            return Ok(await _studentService.GetStudentsWithoutGroups());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTeacher(Guid id)
        {
            await _studentService.RemoveStudentAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _studentService.GetByIdAsync(id));
        }
    }
}
