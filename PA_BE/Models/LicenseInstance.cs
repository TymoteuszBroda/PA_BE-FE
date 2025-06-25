using System;

namespace PermAdminAPI.Models;

public class LicenseInstance
{
    public int Id { get; set; }
    public int LicenseId { get; set; }
    public required DateTime ValidTo { get; set; }

    public License License { get; set; }
}
