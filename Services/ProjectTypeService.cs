using DevHouse.Models;
using DevHouse.Data;
using DevHouse.DTO;
using DevHouse.Helper;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;

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
            ValidationHelper.IdInRangeOrException(id);
            var projectType = await _context.ProjectTypes.FindAsync(id);
            ValidationHelper.CheckIfExistsOrException((projectType, nameof(ProjectType)));
            return projectType;
        }

        public async Task<ProjectType> AddProjectType( AddProjectTypeDTO projectType) {
            bool projectTypeExists = await _context.ProjectTypes.AnyAsync( p => p.Name == projectType.Name);
            ValidationHelper.CheckIfNotInDatabaseOrException(projectTypeExists, nameof(ProjectType));

            var newProjectType = new ProjectType { Name = projectType.Name};
            _context.ProjectTypes.Add(newProjectType);
            await _context.SaveChangesAsync();
            return newProjectType;
        }

        public async Task<ProjectType> UpdateProjectType(int id, UpdateProjectTypeDTO projectType) {
            ValidationHelper.CheckIfIdMatchBodyIdOrException(id, projectType.Id, nameof(ProjectType));

            bool projectTypeExists = await _context.ProjectTypes.AnyAsync( p => p.Name == projectType.Name);
            ValidationHelper.CheckIfNotInDatabaseOrException(projectTypeExists, nameof(ProjectType));

            var updatedProjectType = new ProjectType { 
                Id = projectType.Id, 
                Name = projectType.Name
            };

            _context.ProjectTypes.Update(updatedProjectType);
            await _context.SaveChangesAsync();
            return updatedProjectType;
        }

        public async Task DeleteProjectType(int id) {
            ValidationHelper.IdInRangeOrException(id);
            var projectType = await _context.ProjectTypes.FindAsync(id);
            ValidationHelper.CheckIfExistsOrException((projectType, nameof(ProjectType)));  
            _context.ProjectTypes.Remove(projectType);
            await _context.SaveChangesAsync();
        }
    }
}