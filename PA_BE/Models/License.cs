using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PermAdminAPI.Models;

public class License
{
    public int id { get; set; }
    public required string ApplicationName { get; set; }

    [Column("Quantity")]
    public required int AvailableLicenses { get; set; }

    [NotMapped]
    public int Quantity { get; set; }

    public required DateTime ValidTo { get; set; }
}
