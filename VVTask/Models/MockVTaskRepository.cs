using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public class MockVTaskRepository : IVTaskRepository
    {
        readonly List<VTask> AllVTasks;
        public MockVTaskRepository() {
            AllVTasks= new List<VTask>
            {
                new VTask { Id = 1, Description = "Brushing teeth at least 10 minutes", Point = 1, Done=false }, 
                new VTask { Id = 2, Description = "Donate blood to a charity", Point = 15, Done=false }, 
                new VTask { Id = 3, Description = "Sweep and mob the floor", Point = 3, Done=false }
            };
        }
        public IEnumerable<VTask> GetAll()
        {
            return AllVTasks;
        }
        public int Commit()
        {
            return 0;
        }

        public VTask Add(VTask newVTask)
        {
            AllVTasks.Add(newVTask);
            newVTask.Id = AllVTasks.Max(t => t.Id) + 1;
            return newVTask;
        }

        public VTask GetTaskById(int VTaskId)
        {
            return AllVTasks.FirstOrDefault(t => t.Id == VTaskId);
        }

        public VTask Update(VTask updatedVTask)
        {
            var vTask = AllVTasks.FirstOrDefault(t => t.Id == updatedVTask.Id);
            if(vTask != null)
            {
                vTask.Description = updatedVTask.Description;
                vTask.Point = updatedVTask.Point;
                vTask.Done = updatedVTask.Done;
                vTask.VType = updatedVTask.VType;
            }
            return vTask;
        }

        public VTask Delete(int id)
        {
            var vTask = AllVTasks.FirstOrDefault(t => t.Id == id);
            if(vTask != null)
            {
                AllVTasks.Remove(vTask);
                Commit();
            }
            return vTask;
        }

        public void UpdateStatus(VTask updatedVTask)
        {
            var vTask = AllVTasks.FirstOrDefault(t => t.Id == updatedVTask.Id);
            if (vTask != null)
            {
                vTask.Description = updatedVTask.Description;
                vTask.Point = updatedVTask.Point;
                vTask.Done = !updatedVTask.Done;
                vTask.VType = updatedVTask.VType;
            }
        }
    }
}
