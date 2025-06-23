using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PermAdminAPI.Data;
using PermAdminAPI.DTOs;
using PermAdminAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PermAdminAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenceController(DataContext context) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Licence>>> GetLicences()
        {
            var licences = await context.Licences.ToListAsync();
            return Ok(licences);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Licence>> GetLicence(int id)
        {
            var licence = await context.Licences.FindAsync(id);
            if (licence == null) return NotFound();
            return Ok(licence);
        }

        [HttpPost]
public async Task<ActionResult<Licence>> CreateLicence(Licence licence)
{
    var existingLicence = await context.Licences
        .FirstOrDefaultAsync(l => l.ApplicationName == licence.ApplicationName);

    if (existingLicence != null)
    {
        existingLicence.Quantity += licence.Quantity;
        existingLicence.ValidTo = licence.ValidTo;

        for (int i = 0; i < licence.Quantity; i++)
        {
            context.LicenceInstances.Add(new LicenceInstance
            {
                LicenceId = existingLicence.id,
                ValidTo = existingLicence.ValidTo
            });
        }
        
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLicence), new { id = existingLicence.id }, existingLicence);
    }
    else
    {
        context.Licences.Add(licence);
        await context.SaveChangesAsync();

        for (int i = 0; i < licence.Quantity; i++)
        {
            context.LicenceInstances.Add(new LicenceInstance
            {
                LicenceId = licence.id,
                ValidTo = licence.ValidTo
            });
        }
        
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLicence), new { id = licence.id }, licence);
    }
}

        [HttpGet("{id}/instances")]
        public async Task<ActionResult<IEnumerable<LicenceInstance>>> GetLicenceInstances(int id)
        {
            var instances = await context.LicenceInstances
            .Where(li => li.LicenceId == id)
            .ToListAsync();
            if (!instances.Any())
            {
                return NotFound();
            }
            return Ok(instances);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicence(int id, Licence licence)
        {
            if (id != licence.id) return BadRequest();

            context.Entry(licence).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LicenceExists(id)) return NotFound();
                throw;
            }
            return NoContent();
        }
        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<Licence>>> CreateLicences(List<Licence> licences)
        {
            context.Licences.AddRange(licences);
            await context.SaveChangesAsync();
            return Ok(licences);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicence(int id)
        {
            var licence = await context.Licences.FindAsync(id);
            if (licence == null) return NotFound();

            context.Licences.Remove(licence);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("assigned-licences")]
        public async Task<ActionResult<IEnumerable<AssignLicenceDTO>>> GetAssignedLicences()
        {
            var assignedLicences = await context.EmployeeLicences
                .Include(el => el.Employee)
                .Include(el => el.Licence)
                .Select(el => new AssignLicenceDTO 
                {
                    Id = el.id,
                    EmployeeId = el.employeeId,
                    LicenceId = el.licenceId,
                    EmployeeName = $"{el.Employee.FirstName} {el.Employee.LastName}",
                    LicenceName = el.Licence.ApplicationName
                })
                .ToListAsync();
            
            return Ok(assignedLicences);
        }

        [HttpGet("assigned-licences/{id}")]
        public async Task<ActionResult<AssignLicenceDTO>> GetAssignedLicence(int id)
        {
            var employeeLicence = await context.EmployeeLicences
                .Include(el => el.Employee)
                .Include(el => el.Licence)
                .FirstOrDefaultAsync(el => el.id == id);

            if (employeeLicence == null) return NotFound();

            var dto = new AssignLicenceDTO
            {
                Id = employeeLicence.id,
                EmployeeId = employeeLicence.employeeId,
                LicenceId = employeeLicence.licenceId,
                EmployeeName = employeeLicence.Employee.FirstName+" "+employeeLicence.Employee.LastName,
                LicenceName = employeeLicence.Licence.ApplicationName
            };

            return Ok(dto);
        }

        [HttpPost("assign-licence")]
        public async Task<ActionResult<AssignLicenceDTO>> AssignLicence([FromBody] AssignLicenceDTO request)
        {
            var existingAssignment = await context.EmployeeLicences
                .AnyAsync(el => el.employeeId == request.EmployeeId 
                            && el.licenceId == request.LicenceId);

            if (existingAssignment)
            {
                return BadRequest("This licence is already assigned to the employee");
            }

            var licence = await context.Licences
                .FirstOrDefaultAsync(l => l.id == request.LicenceId);

            if (licence == null)
            {
                return NotFound("Licence not found");
            }

            if (licence.Quantity <= 0)
            {
                return BadRequest("Licence quantity exhausted");
            }
            var employee = await context.Employees
                .FirstOrDefaultAsync(e => e.id == request.EmployeeId);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            var employeeLicence = new EmployeeLicence
            {
                employeeId = request.EmployeeId,
                licenceId = request.LicenceId
            };

            context.EmployeeLicences.Add(employeeLicence);
            licence.Quantity--;
            
            await context.SaveChangesAsync();

            return Ok(new AssignLicenceDTO
            {
                Id = employeeLicence.id,
                EmployeeId = employeeLicence.employeeId,
                LicenceId = employeeLicence.licenceId,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                LicenceName = licence.ApplicationName
            });
        }

        [HttpGet("assigned-licences/employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<AssignLicenceDTO>>> GetAssignedLicencesByEmployee(int employeeId)
        {
            var assignedLicences = await context.EmployeeLicences
                .Where(el => el.employeeId == employeeId)
                .Include(el => el.Employee)
                .Include(el => el.Licence)
                .Select(el => new AssignLicenceDTO 
                {
                    Id = el.id,
                    EmployeeId = el.employeeId,
                    LicenceId = el.licenceId,
                    EmployeeName = $"{el.Employee.FirstName} {el.Employee.LastName}",
                    LicenceName = el.Licence.ApplicationName
                })
                .ToListAsync();
            
            return Ok(assignedLicences);
        }

        [HttpDelete("assigned-licences/{id}")]
        public async Task<IActionResult> DeleteAssignedLicence(int id)
        {
            var employeeLicence = await context.EmployeeLicences.FindAsync(id);
            if (employeeLicence == null) return NotFound();

            var licence = await context.Licences.FindAsync(employeeLicence.licenceId);
            if (licence != null)
            {
                licence.Quantity++;
            }

            context.EmployeeLicences.Remove(employeeLicence);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool LicenceExists(int id)
        {
            return context.Licences.Any(e => e.id == id);
        }
    }
}
