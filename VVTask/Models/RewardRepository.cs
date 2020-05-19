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

        public int Commit()
        {
            return _appDbContext.SaveChanges();
        }

        public Reward Delete(int id)
        {
            var reward = _appDbContext.Rewards
                        .Where(t => t.RewardId == id)
                        .FirstOrDefault();
            if (reward != null)
            {
                _appDbContext.Rewards.Remove(reward);
            }
            return reward;
        }

        public IEnumerable<Reward> GetAll()
        {
            var result = from r in _appDbContext.Rewards
                         orderby r.Description
                         select r;
            return result;
        }

        public IEnumerable<Reward> GetAllByKidId(int kidId)
        {
            return _appDbContext.Rewards
               .Include(v => v.Kid)
               .Where(v => v.KidId == kidId)
               .ToList();
        }

        public Reward GetRewardById(int rewardId)
        {
            return _appDbContext.Rewards.Find(rewardId);
        }

        public Reward Update(Reward updateReward)
        {
            var entity = _appDbContext.Rewards.Attach(updateReward);
            entity.State = EntityState.Modified;
            return updateReward;
        }
    }
}
