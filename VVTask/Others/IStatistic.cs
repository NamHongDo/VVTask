using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVTask.Models;

namespace VVTask.Others
{
    public interface IStatistic
    {
        int countPendingTask(IEnumerable<VTask> vTasks);
        int countCompleteTask(IQueryable<VTask> vTasks);
        int countAvailableRewards(IEnumerable<Reward> rewards);
        int countRedeemedRewards(IEnumerable<Reward> rewards);
    }
}
