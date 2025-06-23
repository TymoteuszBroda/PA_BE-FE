using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PermAdminAPI.Data;
using PermAdminAPI.Models;

namespace PermAdminAPI.Controllers
{
    public class EmployeesController(DataContext context) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await context.Employees.ToListAsync();
            return Ok(employees);
        }
    
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee data is required.");
            }

            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.id }, employee);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.id)
            {
                return BadRequest(new { message = "Employee ID mismatch" });
            }

            var existingEmployee = await context.Employees.FindAsync(id);
            if (existingEmployee == null)
            {
                return NotFound(new { message = "Employee not found" });
            }

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Phone = employee.Phone;
            existingEmployee.Position = employee.Position;

            await context.SaveChangesAsync();

            return Ok(existingEmployee);
        }

         [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var employee = await context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound(new { message = "Employee not found" });
            }

            context.Employees.Remove(employee);
            await context.SaveChangesAsync();

            return Ok(new { message = "Employee deleted successfully" });
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> AddEmployees(List<Employee> employees)
        {
            if (employees == null || employees.Count == 0)
            {
                return BadRequest(new { message = "Employee list is required" });
            }

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            return Ok(new { message = $"{employees.Count} employees added successfully" });
        }
    }
}
