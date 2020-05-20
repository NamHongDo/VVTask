using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public class RewardRepository : IRewardRepository
    {
        private readonly AppDbContext _appDbContext;

        public RewardRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public Reward Add(Reward newReward)
        {
            _appDbContext.Rewards.Add(newReward);
            return newReward;
        }

        public async Task CommitAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Reward> Delete(int id)
        {
            var reward = await _appDbContext.Rewards
                        .Where(t => t.RewardId == id)
                        .FirstOrDefaultAsync();
            if (reward != null)
            {
                _appDbContext.Rewards.Remove(reward);
            }
            return reward;
        }

        public async Task<IEnumerable<Reward>> GetAll()
        {
            return await _appDbContext.Rewards.ToListAsync();
        }

        public async Task<IEnumerable<Reward>> GetAllByKidId(int kidId)
        {
            return await _appDbContext.Rewards
               .Include(v => v.Kid)
               .Where(v => v.KidId == kidId)
               .ToListAsync();
        }

        public async Task<Reward> GetRewardById(int rewardId)
        {
            return await _appDbContext.Rewards.FindAsync(rewardId);
        }

        public Reward Update(Reward updateReward)
        {
            var entity = _appDbContext.Rewards.Attach(updateReward);
            entity.State = EntityState.Modified;
            return updateReward;
        }
    }
}
