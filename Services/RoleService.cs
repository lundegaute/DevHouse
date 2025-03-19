using DevHouse.Models;
using DevHouse.Data;
using DevHouse.DTO;
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
            if ( id < 1) {
                throw new ArgumentException("Id must be greater than 0");
            }
            var role = await _context.Roles.FindAsync(id);
            if ( role is null) {
                throw new InvalidOperationException("Role not found");
            }
            return role;
        }
        public async Task<Role> AddRole(string name) {
            if ( string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException("Name cannot be null or empty");
            }

            bool roleExists = await _context.Roles.AnyAsync( r => r.Name == name);
            if ( roleExists) {
                throw new InvalidOperationException("Role already in database");
            }

            var newRole = new Role {Name = name};
            _context.Roles.Add(newRole);
            await _context.SaveChangesAsync();
            return newRole;
        }
        public async Task UpdateRole(int id, UpdateRoleDTO role) {
            if (id < 1) {
                throw new IndexOutOfRangeException("Id must be greater than 1");
            } else if (string.IsNullOrWhiteSpace(role.Name)) {
                throw new ArgumentException("Name cannot be null or empty");
            } else if (id != role.Id) {
                throw new ArgumentException("Id not matching role Id");
            };

            var roleExists = await _context.Roles.AnyAsync( r => r.Name == role.Name);
            if ( roleExists ) {
                throw new InvalidOperationException("Role already in database");
            }
            
            _context.Roles.Update(new Role { Id = role.Id, Name = role.Name});
            await _context.SaveChangesAsync();
        }
        public async Task DeleteRole(int id) {
            if ( id < 1) {
                throw new IndexOutOfRangeException("Id must be 1 or greater");
            }

            var role = await _context.Roles.FindAsync(id);
            if ( role is null ) {
                throw new ArgumentException("Role not found");
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }
}