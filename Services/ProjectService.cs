using DevHouse.Models;
using DevHouse.DTO;
using DevHouse.Data;
using DevHouse.Helper;
using Microsoft.EntityFrameworkCore;

namespace DevHouse.Services {
    public class ProjectService {
        private readonly DataContext _context;
        public ProjectService(DataContext context) {
            _context = context;
        }      

        public async Task<ICollection<Project>> GetProjects() {
            return await _context.Projects.Include(p => p.ProjectType).Include(t => t.Team).ThenInclude(d => d.Developers).ThenInclude(r => r.Role).ToListAsync();
        }
        public async Task<Project> GetProject(int id) {
            ValidationHelper.IdInRangeOrException(id);
            var project = await _context.Projects.Include(p => p.ProjectType).Include(t => t.Team).ThenInclude(d => d.Developers).ThenInclude(r => r.Role).SingleOrDefaultAsync(p => p.Id == id);
            ValidationHelper.CheckIfExistsOrException((project, nameof(Project)));
            return project;
        } 

        public async Task<Project> AddProject(AddProjectDTO project) {
            var projectExists = await _context.Projects.AnyAsync(p => p.Name == project.Name);
            ValidationHelper.CheckIfNotInDatabaseOrException(projectExists, nameof(Project));

            var team = await _context.Teams.FindAsync(project.TeamId);
            var projectType = await _context.ProjectTypes.FindAsync(project.ProjectTypeId);
            ValidationHelper.CheckIfExistsOrException((team, nameof(Team)), (projectType, nameof(ProjectType)));
            
            var newProject = new Project {
                Name = project.Name,
                Team = team,
                ProjectType = projectType
            };
            _context.Projects.Add(newProject);
            await _context.SaveChangesAsync();
            return newProject;
        }

        public async Task UpdateProject(int id, UpdateProjectDTO project) {
            ValidationHelper.CheckIfIdMatchBodyIdOrException(id, project.Id, nameof(Project));
            var projectExists = await _context.Projects.AnyAsync(p => p.Name == project.Name);
            ValidationHelper.CheckIfNotInDatabaseOrException(projectExists, "Project");
            
            var team = await _context.Teams.FindAsync(project.TeamId);
            var projectType = await _context.ProjectTypes.FindAsync(project.ProjectTypeId);
            ValidationHelper.CheckIfExistsOrException((team, nameof(Team)), (projectType, nameof(ProjectType)));    
            
            var updatedProject = new Project {
                Id = project.Id,
                Name = project.Name,
                ProjectType = projectType,
                Team = team
            };
            _context.Projects.Update(updatedProject);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProject(int id) {
            ValidationHelper.IdInRangeOrException(id);
            var projectToDelete = await _context.Projects.FindAsync(id);
            ValidationHelper.CheckIfExistsOrException((projectToDelete, nameof(Project))); 
            _context.Projects.Remove(projectToDelete);
            await _context.SaveChangesAsync();
        }

    }
}