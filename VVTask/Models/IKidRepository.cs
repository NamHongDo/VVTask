using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public interface IKidRepository
    {
        IEnumerable<KidProfile> GetAll();
        KidProfile GetProfileById(int VTaskId);
        KidProfile Add(KidProfile profile);
        KidProfile Update(KidProfile updatedProfile);
        KidProfile Delete(int id);
        int Commit();
        void UpdateStatus(KidProfile vTask);
    }
}
