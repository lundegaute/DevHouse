using DevHouse.Models;
using DevHouse.Data;
using DevHouse.DTO;
using DevHouse.Helper;
using Microsoft.EntityFrameworkCore;

namespace DevHouse.Services {
    public class RoleService {
        private readonly DataContext _context;
        public RoleService(DataContext context) {
            _context = context;
        }

        public async Task<ICollection<Role>> GetRoles() {
            return await _context.Roles.ToListAsync();
        }
        public async Task<Role> GetRole(int id) {
            ValidationHelper.IdInRangeOrException(id);
            var role = await _context.Roles.FindAsync(id);

            ValidationHelper.CheckIfExistsOrException((role, nameof(Role)));
            return role;
        }
        public async Task<Role> AddRole(AddRoleDTO role) {
            bool roleExists = await _context.Roles.AnyAsync( r => r.Name == role.Name);
            ValidationHelper.CheckIfNotInDatabaseOrException(roleExists, nameof(Role)); 

            var newRole = new Role {Name = role.Name};
            _context.Roles.Add(newRole);
            await _context.SaveChangesAsync();
            return newRole;
        }
        public async Task UpdateRole(int id, UpdateRoleDTO role) {
            ValidationHelper.CheckIfIdMatchBodyIdOrException(id, role.Id, nameof(Role));

            var roleExists = await _context.Roles.AnyAsync( r => r.Name == role.Name);
            ValidationHelper.CheckIfNotInDatabaseOrException(roleExists, nameof(Role)); 
            
            _context.Roles.Update(new Role { Id = role.Id, Name = role.Name});
            await _context.SaveChangesAsync();
        }
        public async Task DeleteRole(int id) {
            ValidationHelper.IdInRangeOrException(id);

            var role = await _context.Roles.FindAsync(id);
            ValidationHelper.CheckIfExistsOrException((role, nameof(Role)));

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }
}