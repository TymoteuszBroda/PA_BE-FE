using System;

namespace PermAdminAPI.Models;

public class LicenceInstance
{
    public int Id { get; set; }
    public int LicenceId { get; set; }
    public required DateTime ValidTo { get; set; }

    public Licence Licence { get; set; }
}
