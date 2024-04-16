using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly Entities context;
        public DepartmentController(Entities context)
        {
            this.context = context;
        }
        [HttpGet]
        public ActionResult GetAllDepartment()
        {
            List<Department> departments = context.Departments.ToList();
            return Ok(departments);
        }

        [HttpGet]
        [Route("{Id:int}")]
        public ActionResult GetDepartmentById(int Id)
        {
            Department department = context.Departments.FirstOrDefault(x => x.Id == Id);
            return Ok(department);
        }

        [HttpGet]
        [Route("{Name:alpha}")]
        public ActionResult GetDepartmentByName(String Name)
        {
            Department department = context.Departments.FirstOrDefault(x => x.Name == Name);
            return Ok(department);
        }
        [HttpPost]
        public ActionResult PostDepartment(Department department)
        {
            if (ModelState.IsValid == true)
            {
                context.Departments.Add(department);
                context.SaveChanges();
                // return Ok("Saved");
                return Created("http://localhost:7341/api/Department" + department.Id, department);
            }


            return BadRequest(ModelState);
        }

        [HttpPut("{Id:int}")]
        public ActionResult PutDepartment([FromRoute] int Id, [FromBody] Department department)
        {
            if (ModelState.IsValid == true)
            {
                Department OldDept = context.Departments.FirstOrDefault(x => x.Id == Id);
                OldDept.Name = department.Name;
                OldDept.Manager = department.Manager;
                context.SaveChanges();
                // return Ok("Saved");
                return StatusCode(204, "Data Updated");
            }


            return BadRequest(ModelState);
        }
        [HttpDelete("{Id:int}")]
        public ActionResult DeleteDepartment([FromRoute] int Id)
        {
            Department OldDept = context.Departments.FirstOrDefault(x => x.Id == Id);
            if (OldDept != null)
            {
                context.Departments.Remove(OldDept);
                context.SaveChanges();
                return StatusCode(204, "Record Removed");
            }


            return BadRequest("Id Not Found");
        }
    }
}
