using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public class VTaskRepository : IVTaskRepository
    {
        private readonly AppDbContext _appDbContext;

        public VTaskRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public VTask Add(VTask newVTask)
        {
            _appDbContext.VTasks.Add(newVTask);
            return newVTask;
        }

        public async Task CommitAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<VTask> Delete(int id)
        {
            var vTask = await _appDbContext.VTasks
                        .Where(t => t.VTaskId == id)
                        .FirstOrDefaultAsync();
            if(vTask != null)
            {
                _appDbContext.VTasks.Remove(vTask);
            }
            return vTask;
        }

        public VTask GetTaskById(int id)
        {
            return _appDbContext.VTasks.Find(id);
        }

        public VTask Update(VTask updatedVTask)
        {
            var entity = _appDbContext.VTasks.Attach(updatedVTask);
            entity.State = EntityState.Modified;
            return updatedVTask;
        }

        public async Task<IEnumerable<VTask>> GetAll()
        {
            return await _appDbContext.VTasks.ToListAsync();
        }
        public async Task<IEnumerable<VTask>> GetAllByKidId(int KidId)
        {
            return await _appDbContext.VTasks
                           .Include(v => v.Kid)
                           .Where(v => v.KidId == KidId)
                           .ToListAsync();
        }
    }
}
