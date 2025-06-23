using System;

namespace PermAdminAPI.Models;

public class AppUser
{
    public int id { get; set; }
    public required string UserName {get; set;}
    public byte[] PasswordHash { get; set; } =[];
    public byte[] PasswordSalt { get; set; } = [];

}
