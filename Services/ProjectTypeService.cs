using DevHouse.Models;
using DevHouse.Data;
using DevHouse.DTO;
using Microsoft.EntityFrameworkCore;

namespace DevHouse.Services {
    public class ProjectTypeService {
        private readonly DataContext _context;

        public ProjectTypeService(DataContext dataContext) {
            _context = dataContext;
        }

        public async Task<ICollection<ProjectType>> GetProjectTypes() {
            return await _context.ProjectTypes.ToListAsync();   
        }

        public async Task<ProjectType> GetProjectType (int id) {
            if ( id < 1) {
                throw new IndexOutOfRangeException("Id must be greater than 0");
            }
            var projectType = await _context.ProjectTypes.FindAsync(id);
            if ( projectType is null ) {
                throw new KeyNotFoundException("Project type not found");
            }
            return projectType;
        }

        public async Task<ProjectType> AddProjectType( AddProjectTypeDTO projectType) {
            var projectTypeExists = await _context.ProjectTypes.AnyAsync( p => p.Name == projectType.Name);
            if ( projectTypeExists ) {
                throw new ArgumentException("Project type already in database");
            }
            var newProjectType = new ProjectType { Name = projectType.Name};
            _context.ProjectTypes.Add(newProjectType);
            await _context.SaveChangesAsync();
            return newProjectType;
        }

        public async Task<ProjectType> UpdateProjectType(int id, UpdateProjectTypeDTO projectType) {
            if ( id != projectType.Id) {
                throw new ArgumentException("Id not matching project type Id");
            }

            var projectTypeExists = await _context.ProjectTypes.AnyAsync( p => p.Name == projectType.Name);
            if ( projectTypeExists ) {
                throw new InvalidOperationException("Project type already in database");
            }

            var updatedProjectType = new ProjectType { 
                Id = projectType.Id, 
                Name = projectType.Name
            };

            _context.ProjectTypes.Update(updatedProjectType);
            await _context.SaveChangesAsync();
            return updatedProjectType;
        }

        public async Task DeleteProjectType(int id) {
            if ( id < 1) {
                throw new IndexOutOfRangeException("Id must be 1 or greater");
            }
            var projectType = await _context.ProjectTypes.FindAsync(id);
            if ( projectType is null) {
                throw new KeyNotFoundException("Project type not found");
            }
            _context.ProjectTypes.Remove(projectType);
            await _context.SaveChangesAsync();
        }
    }
}