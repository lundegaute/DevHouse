using DevHouse.Data;
using DevHouse.DTO;
using DevHouse.Helper;
using DevHouse.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHouse.Services {
    public class TeamService {
        private readonly DataContext _context;
        public TeamService(DataContext context) {
            _context = context;
        }

        public async Task<ICollection<Team>> GetTeams() {
            return await _context.Teams.ToListAsync();
        }
        public async Task<Team> GetTeam(int id) {
            ValidationHelper.IdInRangeOrException(id);

            var team = await _context.Teams.FindAsync(id);
            ValidationHelper.CheckIfExistsOrException((team, nameof(Team)));
            
            return team;
        }
        public async Task<Team> AddTeam(AddTeamDTO team) {
            bool teamExists = await _context.Teams.AnyAsync( t => t.Name == team.Name);
            ValidationHelper.CheckIfNotInDatabaseOrException(teamExists, nameof(Team));

            var newTeam = new Team { Name = team.Name};
            _context.Teams.Add(newTeam);
            await _context.SaveChangesAsync();   
            return newTeam;
        }
        public async Task UpdateTeam(int id, UpdateTeamDTO team) {
            ValidationHelper.CheckIfIdMatchBodyIdOrException(id, team.Id, nameof(Team));

            var teamExists = await _context.Teams.AnyAsync( t => t.Name == team.Name);
            ValidationHelper.CheckIfNotInDatabaseOrException(teamExists, nameof(Team));

            _context.Teams.Update(new Team { Id = team.Id, Name = team.Name });
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTeam(int id) {
            ValidationHelper.IdInRangeOrException(id);

            var teamToDelete = await _context.Teams.FindAsync(id);
            ValidationHelper.CheckIfExistsOrException((teamToDelete, nameof(Team)));

            _context.Teams.Remove(teamToDelete);
            await _context.SaveChangesAsync();
        }
    }
}