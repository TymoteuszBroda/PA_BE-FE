using System.ComponentModel.DataAnnotations;
using System;

namespace PermAdminAPI.DTOs;

public class AssignLicenceDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Employee ID is required")]
    public int EmployeeId { get; set; }
    
    [Required(ErrorMessage = "Licence ID is required")]
    public int LicenceId { get; set; }
    public string? EmployeeName { get; set; }
    public string? LicenceName { get; set; }
}
