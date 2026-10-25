using CodeFirstEFAPI.Data;
using CodeFirstEFAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstEFAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDBContext studentDBContext;

        public StudentController(StudentDBContext studentDBContext) 
        {
            this.studentDBContext = studentDBContext;
        }

        [HttpGet]
        [Route("Students")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var studentList = await studentDBContext.Students.ToListAsync();

            return Ok(studentList);
        }

        [HttpGet("StudentWithStandard")]
        public IActionResult GetStudentsWithStandardName()
        {
            var students = from student in this.studentDBContext.Students
                                  join standard in this.studentDBContext.Standards
                                  on student.StandardId equals standard.ID
                                  select new
                                  {
                                      ID = student.Id,
                                      Name = student.Name,
                                      Gender = student.Gender,
                                      StandardID = standard.ID,
                                      StandardName = standard.Name
                                  };

            if(students == null || !students.Any())
            {
                return NotFound("No data found");
            }

            return Ok(students);
        }
    }
}
