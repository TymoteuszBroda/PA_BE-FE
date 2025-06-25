using System.ComponentModel.DataAnnotations;
using System;

namespace PermAdminAPI.DTOs;

public class AssignLicenseDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Employee ID is required")]
    public int EmployeeId { get; set; }
    
    [Required(ErrorMessage = "License ID is required")]
    public int LicenseId { get; set; }
    public string? EmployeeName { get; set; }
    public string? LicenseName { get; set; }
    public DateTime AssignedOn { get; set; }
    public DateTime ValidTo { get; set; }
}
