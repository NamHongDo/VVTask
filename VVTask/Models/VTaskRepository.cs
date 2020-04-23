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
            _appDbContext.Add(newVTask);
            return newVTask;
        }

        public int Commit()
        {
            return _appDbContext.SaveChanges();
        }

        public VTask Delete(int id)
        {
            var vTask = _appDbContext.VTasks
                        .Where(t => t.Id == id)
                        .FirstOrDefault();
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

        public void UpdateStatus(VTask vTask)
        {
            throw new NotImplementedException();
        }

        IEnumerable<VTask> IVTaskRepository.GetAll()
        {
            return from v in _appDbContext.VTasks
                   orderby v.Description
                   select v;
        }


    }
}
