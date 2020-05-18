using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public class KidRepository : IKidRepository
    {
        private readonly AppDbContext _appDbContext;

        public KidRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Kid Add(Kid newProfile)
        {
            _appDbContext.Add(newProfile);
            return newProfile;
        }

        public int Commit()
        {
            return _appDbContext.SaveChanges();
        }

        public Kid Delete(int id)
        {
            var profile = _appDbContext.Kids
                        .Where(p => p.KidId == id)
                        .FirstOrDefault();
            if (profile != null)
            {
                _appDbContext.Kids.Remove(profile);
            }
            return profile;
        }

        public IEnumerable<Kid> GetAll()
        {
            return from k in _appDbContext.Kids
                   orderby k.Name
                   select k;
        }

        public Kid GetProfileById(int KidId)
        {
            return _appDbContext.Kids.Find(KidId);
        }

        public Kid Update(Kid updatedProfile)
        {
            var entity = _appDbContext.Kids.Attach(updatedProfile);
            entity.State = EntityState.Modified;
            return updatedProfile;
        }

    }
}
