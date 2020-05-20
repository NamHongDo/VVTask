using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVTask.Models;

namespace VVTask.Models
{
    public interface IRewardRepository
    {
        Task<IEnumerable<Reward>> GetAll();
        Task<Reward> GetRewardById(int rewardId);
        Reward Add(Reward reward);
        Reward Update(Reward reward);
        Task<Reward> Delete(int id);
        Task CommitAsync();
        Task<IEnumerable<Reward>> GetAllByKidId(int id);
    }
}
