using DevHouse.Data;
using DevHouse.Models;
using DevHouse.DTO;
using Microsoft.EntityFrameworkCore;

namespace DevHouse.Services {
    public class DeveloperService {
        private readonly DataContext _context;
        public DeveloperService( DataContext context ) {
            _context = context;
        }

        public async Task<ICollection<Developer>> GetDevelopers() {
            return await _context.Developers.Include(t => t.Team).Include(r => r.Role).ToListAsync();
        }

        public async Task<Developer> GetDeveloper(int id) {
            if ( id < 1) {
                throw new IndexOutOfRangeException("Id must be greater than 0");
            }
            var developer = await _context.Developers.Include(t => t.Team).Include(r => r.Role).SingleOrDefaultAsync(d => d.Id == id);
            if ( developer is null) {
                throw new InvalidOperationException("Developer not found");
            }
            return developer;
        }

        public async Task<Developer> AddDeveloper(AddDeveloperDTO developer) {
            var developerExists = await _context.Developers.AnyAsync( d => d.LastName == developer.LastName);
            if ( developerExists ) {
                throw new ArgumentException("Developer already in database");
            }

            var team = await _context.Teams.FindAsync(developer.TeamId);
            if ( team is null) {
                throw new KeyNotFoundException("Team not found");
            }
            var role = await _context.Roles.FindAsync(developer.RoleId);
            if ( role is null) {
                throw new KeyNotFoundException("Role not found");
            }

            var newDeveloper = new Developer {
                FirstName = developer.FirstName,
                LastName = developer.LastName,
                Team = team,
                Role = role
            };
            _context.Developers.Add(newDeveloper);
            await _context.SaveChangesAsync();                                                                                                                         
            return newDeveloper;
        }

        public async Task UpdateDeveloper(int id, UpdateDeveloperDTO developer) {
            if ( id != developer.Id) {
                throw new ArgumentException("Id not matching developer Id");
            }

            var developerExists = await _context.Developers.AnyAsync( d => d.Id == developer.Id);
            if ( developerExists ) {
                throw new InvalidOperationException("Developer already in database");
            }

            var role = await _context.Roles.FindAsync(developer.RoleId);
            if ( role is null) {
                throw new KeyNotFoundException("Role not found");
            }
            var team = await _context.Teams.FindAsync(developer.TeamId);
            if ( team is null) {
                throw new KeyNotFoundException("Team not found");
            }

            var updatedDeveloper = new Developer {
                Id = developer.Id,
                FirstName = developer.FirstName,
                LastName = developer.LastName,
                Role = role,
                Team = team
            };
            _context.Developers.Update(updatedDeveloper);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDeveloper(int id) {
            if ( id < 1) {
                throw new IndexOutOfRangeException("Id must be greater than 0");
            }
            var developerToDelete = await _context.Developers.FindAsync(id);
            if ( developerToDelete is null) {
                throw new KeyNotFoundException("Developer not found");
            }
            _context.Developers.Remove(developerToDelete);
            await _context.SaveChangesAsync();
        }
    }
}