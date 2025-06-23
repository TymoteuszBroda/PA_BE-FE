using System;

namespace PermAdminAPI.Models;

public class Licence
{
    public int id { get; set; }
    public required string ApplicationName { get; set; }
    public required int Quantity { get; set; }
    public required DateTime ValidTo { get; set; }
}
