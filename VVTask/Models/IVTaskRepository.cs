using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public interface IVTaskRepository
    {
        Task<IEnumerable<VTask>> GetAll();
        VTask GetTaskById(int VTaskId);
        VTask Add(VTask vTask);
        VTask Update(VTask updateVTask);
        Task<VTask> Delete(int id);
        Task CommitAsync();
        Task<IEnumerable<VTask>> GetAllByKidId(int id);
    }
}
