using DevHouse.Data;
using DevHouse.DTO;
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
            if ( id < 1) {
                throw new IndexOutOfRangeException("Id must be greater than 0");
            }
            var team = await _context.Teams.FindAsync(id);
            if ( team is null) {
                throw new InvalidOperationException("Team not found");
            }
            return team;
        }
        public async Task<Team> AddTeam(string name) {
            // if ( string.IsNullOrWhiteSpace(name)) {
            //     throw new ArgumentException("Name cannot be null or empty");
            // }
            var teamExists = await _context.Teams.AnyAsync( t => t.Name == name);
            if ( teamExists ) {
                throw new InvalidOperationException("Team already in database");
            }
            var newTeam = new Team { Name = name};
            _context.Teams.Add(newTeam);
            await _context.SaveChangesAsync();   
            return newTeam;
        }
        public async Task UpdateTeam(int id, UpdateTeamDTO team) {
            // if ( id < 1 ) {
            //     throw new IndexOutOfRangeException("Id must be greater than 0");
            // }
            // if ( string.IsNullOrWhiteSpace(team.Name)) {
            //     throw new ArgumentException("Name cannot be null or empty");
            // }
            if ( id != team.Id) {
                throw new ArgumentException("Id not matching team Id");
            }

            var teamExists = await _context.Teams.AnyAsync( t => t.Name == team.Name);
            if ( teamExists ) {
                throw new InvalidOperationException("Team already in database");
            }

            _context.Teams.Update(new Team { Id = team.Id, Name = team.Name });
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTeam(int id) {
            if (id < 1) {
                throw new IndexOutOfRangeException("Id must be greater tha 0");
            }
            var teamToDelete = await _context.Teams.FindAsync(id);
            if ( teamToDelete is null) {
                throw new InvalidOperationException("Team not found");
            }

            _context.Teams.Remove(teamToDelete);
            await _context.SaveChangesAsync();
        }
    }
}