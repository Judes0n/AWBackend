using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AWBackend.Models;

namespace AWBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AwdbContext _context;

        public StudentsController(AwdbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetAllStudents")]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
        {
          if (_context.Students == null)
          {
              return NotFound();
          }
            return await _context.Students.ToListAsync();
        }

        [HttpGet]
        [Route("SearchStudent/{key}")]
        public async Task<ActionResult<IEnumerable<Student>>> SearchStudent(string key)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            List<Student> list = new();
            list = await _context.Students.Where(s=>s.Name == key).ToListAsync();
            if(list.Count == 0)
            {
                list = await _context.Students.Where(s=>s.Course == key).ToListAsync();
            }
            return list;
        }

        [HttpGet]
        [Route("GetStudent/{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
          if (_context.Students == null)
          {
              return NotFound();
          }
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

     
        [HttpPut]
        [Route("UpdateStudent/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddStudent")]
        public async Task<ActionResult<Student>> AddStudent(Student student)
        {
            var test = _context.Students.FirstOrDefaultAsync(s => s.Name == student.Name);
            if ( test.IsFaulted)
            {
                return Problem("Name is Used Already!");
            }
          if (_context.Students == null)
          {
              return Problem("Entity set 'AwdbContext.Students'  is null.");
          }
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.StudentId }, student);
        }

        
        [HttpDelete]
        [Route("DeleteStudent/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Checking

        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.StudentId == id)).GetValueOrDefault();
        }
    }
}
