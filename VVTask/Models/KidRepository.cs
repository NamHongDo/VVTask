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

        public async Task CommitAsync()
        {
           await _appDbContext.SaveChangesAsync();
        }

        public async Task<Kid> Delete(int id)
        {
            var profile = await _appDbContext.Kids
                        .Where(p => p.KidId == id)
                        .FirstOrDefaultAsync();
            if (profile != null)
            {
                _appDbContext.Kids.Remove(profile);
            }
            return profile;
        }

        public async Task<IEnumerable<Kid>> GetAll()
        {
             return await _appDbContext.Kids.ToListAsync(); 
        }

        public async Task<Kid> GetProfileById(int KidId)
        {
            return await(_appDbContext.Kids.FirstOrDefaultAsync(k => k.KidId == KidId));
        }

        public Kid Update(Kid updatedProfile)
        {
            var entity = _appDbContext.Kids.Attach(updatedProfile);
            entity.State = EntityState.Modified;
            return updatedProfile;
        }

    }
}
