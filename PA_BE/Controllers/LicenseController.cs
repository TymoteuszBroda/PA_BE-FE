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
    public class LicenseController(DataContext context) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<License>>> GetLicenses()
        {
            var licenses = await context.Licenses.ToListAsync();

            foreach (var license in licenses)
            {
                license.Quantity = await context.LicenseInstances
                    .CountAsync(li => li.LicenseId == license.id);
            }

            return Ok(licenses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<License>> GetLicense(int id)
        {
            var license = await context.Licenses.FindAsync(id);
            if (license == null) return NotFound();

            license.Quantity = await context.LicenseInstances
                .CountAsync(li => li.LicenseId == license.id);

            return Ok(license);
        }

        [HttpPost]
public async Task<ActionResult<License>> CreateLicense(License license)
{
    var existingLicense = await context.Licenses
        .FirstOrDefaultAsync(l => l.ApplicationName == license.ApplicationName);

    if (existingLicense != null)
    {
        existingLicense.AvailableLicenses += license.AvailableLicenses;
        existingLicense.ValidTo = license.ValidTo;

        for (int i = 0; i < license.AvailableLicenses; i++)
        {
            context.LicenseInstances.Add(new LicenseInstance
            {
                LicenseId = existingLicense.id,
                ValidTo = existingLicense.ValidTo
            });
        }
        
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLicense), new { id = existingLicense.id }, existingLicense);
    }
    else
    {
        context.Licenses.Add(license);
        await context.SaveChangesAsync();

        for (int i = 0; i < license.AvailableLicenses; i++)
        {
            context.LicenseInstances.Add(new LicenseInstance
            {
                LicenseId = license.id,
                ValidTo = license.ValidTo
            });
        }
        
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLicense), new { id = license.id }, license);
    }
}

        [HttpGet("{id}/instances")]
        public async Task<ActionResult<IEnumerable<LicenseInstance>>> GetLicenseInstances(int id)
        {
            var instances = await context.LicenseInstances
            .Where(li => li.LicenseId == id)
            .ToListAsync();
            if (!instances.Any())
            {
                return NotFound();
            }
            return Ok(instances);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicense(int id, License license)
        {
            if (id != license.id) return BadRequest();

            context.Entry(license).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LicenseExists(id)) return NotFound();
                throw;
            }
            return NoContent();
        }
        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<License>>> CreateLicenses(List<License> licenses)
        {
            context.Licenses.AddRange(licenses);
            await context.SaveChangesAsync();
            return Ok(licenses);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicense(int id)
        {
            var license = await context.Licenses.FindAsync(id);
            if (license == null) return NotFound();

            context.Licenses.Remove(license);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{licenseId}/instance")]
        public async Task<IActionResult> DeleteLicenseInstance(int licenseId)
        {
            var license = await context.Licenses.FindAsync(licenseId);
            if (license == null) return NotFound();

            var instance = await context.LicenseInstances
                .FirstOrDefaultAsync(li => li.LicenseId == licenseId);

            if (instance == null) return NotFound();

            context.LicenseInstances.Remove(instance);

            if (license.AvailableLicenses > 0)
            {
                license.AvailableLicenses--;
            }

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("instance/{instanceId}")]
        public async Task<IActionResult> DeleteLicenseInstanceById(int instanceId)
        {
            var instance = await context.LicenseInstances.FindAsync(instanceId);
            if (instance == null) return NotFound();

            var license = await context.Licenses.FindAsync(instance.LicenseId);

            context.LicenseInstances.Remove(instance);

            if (license != null && license.AvailableLicenses > 0)
            {
                license.AvailableLicenses--;
            }

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("assigned-licenses")]
        public async Task<ActionResult<IEnumerable<AssignLicenseDTO>>> GetAssignedLicenses()
        {
            var assignedLicenses = await context.EmployeeLicenses
                .Include(el => el.Employee)
                .Include(el => el.License)
                .Select(el => new AssignLicenseDTO
                {
                    Id = el.id,
                    EmployeeId = el.employeeId,
                    LicenseId = el.licenseId,
                    EmployeeName = $"{el.Employee.FirstName} {el.Employee.LastName}",
                    LicenseName = el.License.ApplicationName,
                    AssignedOn = el.AssignedOn,
                    ValidTo = el.License.ValidTo
                })
                .ToListAsync();
            
            return Ok(assignedLicenses);
        }

        [HttpGet("assigned-licenses/{id}")]
        public async Task<ActionResult<AssignLicenseDTO>> GetAssignedLicense(int id)
        {
            var employeeLicense = await context.EmployeeLicenses
                .Include(el => el.Employee)
                .Include(el => el.License)
                .FirstOrDefaultAsync(el => el.id == id);

            if (employeeLicense == null) return NotFound();

            var dto = new AssignLicenseDTO
            {
                Id = employeeLicense.id,
                EmployeeId = employeeLicense.employeeId,
                LicenseId = employeeLicense.licenseId,
                EmployeeName = employeeLicense.Employee.FirstName+" "+employeeLicense.Employee.LastName,
                LicenseName = employeeLicense.License.ApplicationName,
                AssignedOn = employeeLicense.AssignedOn,
                ValidTo = employeeLicense.License.ValidTo
            };

            return Ok(dto);
        }

        [HttpPost("assign-license")]
        public async Task<ActionResult<AssignLicenseDTO>> AssignLicense([FromBody] AssignLicenseDTO request)
        {
            var existingAssignment = await context.EmployeeLicenses
                .AnyAsync(el => el.employeeId == request.EmployeeId 
                            && el.licenseId == request.LicenseId);

            if (existingAssignment)
            {
                return BadRequest("This license is already assigned to the employee");
            }

            var license = await context.Licenses
                .FirstOrDefaultAsync(l => l.id == request.LicenseId);

            if (license == null)
            {
                return NotFound("License not found");
            }

            if (license.AvailableLicenses <= 0)
            {
                return BadRequest("License quantity exhausted");
            }
            var employee = await context.Employees
                .FirstOrDefaultAsync(e => e.id == request.EmployeeId);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            var employeeLicense = new EmployeeLicense
            {
                employeeId = request.EmployeeId,
                licenseId = request.LicenseId,
                AssignedOn = DateTime.UtcNow
            };

            context.EmployeeLicenses.Add(employeeLicense);
            license.AvailableLicenses--;
            
            await context.SaveChangesAsync();

            return Ok(new AssignLicenseDTO
            {
                Id = employeeLicense.id,
                EmployeeId = employeeLicense.employeeId,
                LicenseId = employeeLicense.licenseId,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                LicenseName = license.ApplicationName,
                AssignedOn = employeeLicense.AssignedOn,
                ValidTo = license.ValidTo
            });
        }

        [HttpGet("assigned-licenses/employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<AssignLicenseDTO>>> GetAssignedLicensesByEmployee(int employeeId)
        {
            var assignedLicenses = await context.EmployeeLicenses
                .Where(el => el.employeeId == employeeId)
                .Include(el => el.Employee)
                .Include(el => el.License)
                .Select(el => new AssignLicenseDTO
                {
                    Id = el.id,
                    EmployeeId = el.employeeId,
                    LicenseId = el.licenseId,
                    EmployeeName = $"{el.Employee.FirstName} {el.Employee.LastName}",
                    LicenseName = el.License.ApplicationName,
                    AssignedOn = el.AssignedOn,
                    ValidTo = el.License.ValidTo
                })
                .ToListAsync();
            
            return Ok(assignedLicenses);
        }

        [HttpGet("assigned-licenses/license/{licenseId}")]
        public async Task<ActionResult<IEnumerable<AssignLicenseDTO>>> GetAssignedLicensesByLicense(int licenseId)
        {
            var assignedLicenses = await context.EmployeeLicenses
                .Where(el => el.licenseId == licenseId)
                .Include(el => el.Employee)
                .Include(el => el.License)
                .Select(el => new AssignLicenseDTO
                {
                    Id = el.id,
                    EmployeeId = el.employeeId,
                    LicenseId = el.licenseId,
                    EmployeeName = $"{el.Employee.FirstName} {el.Employee.LastName}",
                    LicenseName = el.License.ApplicationName,
                    AssignedOn = el.AssignedOn,
                    ValidTo = el.License.ValidTo
                })
                .ToListAsync();

            return Ok(assignedLicenses);
        }

        [HttpDelete("assigned-licenses/{id}")]
        public async Task<IActionResult> DeleteAssignedLicense(int id)
        {
            var employeeLicense = await context.EmployeeLicenses.FindAsync(id);
            if (employeeLicense == null) return NotFound();

            var license = await context.Licenses.FindAsync(employeeLicense.licenseId);
            if (license != null)
            {
                license.AvailableLicenses++;
            }

            context.EmployeeLicenses.Remove(employeeLicense);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool LicenseExists(int id)
        {
            return context.Licenses.Any(e => e.id == id);
        }
    }
}
