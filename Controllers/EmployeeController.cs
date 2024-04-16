using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTO;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly Entities context;
        public EmployeeController(Entities context)
        {
            this.context = context;
        }
        [HttpGet]
        public ActionResult GetAllEmployees()
        {
            List<Employee> employees = context.Employees.Include(s=>s.Department).ToList();
            return Ok(employees);
        }
        [HttpGet("{id:int}")]
        public ActionResult GetEmpById(int id)
        {
            Employee employee = context.Employees.Include(s => s.Department).
                FirstOrDefault(s=>s.Id==id);
            EmployeeDataWithDepartment employeeDataWithDepartment = new EmployeeDataWithDepartment();
            employeeDataWithDepartment.Adderess = employee.Address;
            employeeDataWithDepartment.DepartmentName = employee.Department.Name;
            employeeDataWithDepartment.EmployeeName = employee.Name;
            employeeDataWithDepartment.Id = employee.Id;
            return Ok(employeeDataWithDepartment);
        }
    }
}
