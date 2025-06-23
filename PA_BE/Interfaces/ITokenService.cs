using System;
using PermAdminAPI.Models;


namespace PermAdminAPI.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}