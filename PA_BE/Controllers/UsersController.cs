using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PermAdminAPI.Data;
using PermAdminAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PermAdminAPI.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        [HttpDelete("delete-all")]
        public async Task<ActionResult> DeleteAllUsers()
        {
            // Retrieve all users from the database
            var users = await _context.Users.ToListAsync();

            // Remove all users from the context
            _context.Users.RemoveRange(users);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return a successful response
            return NoContent();  // 204 No Content status
        }

    }
}