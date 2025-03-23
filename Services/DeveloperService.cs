using DevHouse.Data;
using DevHouse.Models;
using DevHouse.DTO;
using DevHouse.Helper;
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
            ValidationHelper.IdInRangeOrException(id);
            var developer = await _context.Developers.Include(t => t.Team).Include(r => r.Role).SingleOrDefaultAsync(d => d.Id == id);
            ValidationHelper.CheckIfExistsOrException((developer, nameof(Developer)));
            return developer;
        }

        public async Task<Developer> AddDeveloper(AddDeveloperDTO developer) {
            var developerExists = await _context.Developers.AnyAsync( d => d.LastName == developer.LastName);
            ValidationHelper.CheckIfNotInDatabaseOrException(developerExists, nameof(Developer));
            var team = await _context.Teams.FindAsync(developer.TeamId);
            var role = await _context.Roles.FindAsync(developer.RoleId);
            ValidationHelper.CheckIfExistsOrException((team, nameof(Team)), (role, nameof(Role)));
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
            ValidationHelper.CheckIfIdMatchBodyIdOrException(id, developer.Id, nameof(Developer));
            var developerExists = await _context.Developers.AnyAsync( d => d.LastName == developer.LastName);
            ValidationHelper.CheckIfNotInDatabaseOrException(developerExists, nameof(Developer));
            var role = await _context.Roles.FindAsync(developer.RoleId);
            var team = await _context.Teams.FindAsync(developer.TeamId);
            ValidationHelper.CheckIfExistsOrException((role, nameof(Role)), (team, nameof(Team)));
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
            ValidationHelper.IdInRangeOrException(id);
            var developerToDelete = await _context.Developers.FindAsync(id);
            ValidationHelper.CheckIfExistsOrException((developerToDelete, nameof(Developer)));
            _context.Developers.Remove(developerToDelete);
            await _context.SaveChangesAsync();
        }
    }
}