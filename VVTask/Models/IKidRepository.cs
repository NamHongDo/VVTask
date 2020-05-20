using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public interface IKidRepository
    {
        Task<IEnumerable<Kid>> GetAll();
        Task<Kid> GetProfileById(int VTaskId);
        Kid Add(Kid profile);
        Kid Update(Kid updatedProfile);
        Task<Kid> Delete(int id);
        Task CommitAsync();
    }
}
