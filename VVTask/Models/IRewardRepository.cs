using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVTask.Models;

namespace VVTask.Models
{
    public interface IRewardRepository
    {
        IEnumerable<Reward> GetAll();
        Reward GetRewardById(int rewardId);
        Reward Add(Reward reward);
        Reward Update(Reward reward);
        Reward Delete(int id);
        int Commit();
        IEnumerable<Reward> GetAllByKidId(int id);
    }
}
