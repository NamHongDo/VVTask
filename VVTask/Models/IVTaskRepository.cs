using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public interface IVTaskRepository
    {
        IEnumerable<VTask> GetAll();
        VTask GetTaskById(int VTaskId);
        VTask Add(VTask vTask);
        VTask Update(VTask updateVTask);
        VTask Delete(int id);
        int Commit();
        void UpdateStatus(VTask vTask);
    }
}
