using System;

namespace PermAdminAPI.Models;

public class EmployeeLicense
{
    public int id { get; set; }
    public int employeeId { get; set; }
    public Employee Employee { get; set; }
    public int licenseId { get; set; }
    public License License { get; set; }
    public DateTime AssignedOn { get; set; }
}