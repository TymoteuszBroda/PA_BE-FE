using System;

namespace PermAdminAPI.Models;

public class EmployeeLicence
{
    public int id { get; set; }
    public int employeeId { get; set; }
    public Employee Employee { get; set; }
    public int licenceId { get; set; }
    public Licence Licence { get; set; }
}