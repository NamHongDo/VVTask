using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public interface IKidRepository
    {
        IEnumerable<Kid> GetAll();
        Kid GetProfileById(int VTaskId);
        Kid Add(Kid profile);
        Kid Update(Kid updatedProfile);
        Kid Delete(int id);
        int Commit();
    }
}
