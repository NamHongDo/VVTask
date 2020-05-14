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

        public KidProfile Add(KidProfile newProfile)
        {
            _appDbContext.Add(newProfile);
            return newProfile;
        }

        public int Commit()
        {
            return _appDbContext.SaveChanges();
        }

        public KidProfile Delete(int id)
        {
            var profile = _appDbContext.Profiles
                        .Where(p => p.KidId == id)
                        .FirstOrDefault();
            if (profile != null)
            {
                _appDbContext.Profiles.Remove(profile);
            }
            return profile;
        }

        public IEnumerable<KidProfile> GetAll()
        {
            return from p in _appDbContext.Profiles
                   orderby p.KidName
                   select p;
        }

        public KidProfile GetProfileById(int KidId)
        {
            return _appDbContext.Profiles.Find(KidId);
        }

        public KidProfile Update(KidProfile updatedProfile)
        {
            var entity = _appDbContext.Profiles.Attach(updatedProfile);
            entity.State = EntityState.Modified;
            return updatedProfile;
        }

        public void UpdateStatus(KidProfile vTask)
        {
            throw new NotImplementedException();
        }
    }
}
