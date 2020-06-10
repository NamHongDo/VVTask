using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVTask.Models;

namespace VVTask.Others
{
    public class Statistic:IStatistic
    {

        public int countPendingTask(IEnumerable<VTask> vTasks)
        {
            int pendingTaskCount = vTasks.Where(t => t.Done == false).Count();
            return pendingTaskCount;
        }

        public int countCompleteTask(IQueryable<VTask> vTasks)
        {
            int completeTaskCount = vTasks.Where(t => t.Done == true).Count();
            return completeTaskCount;
        }

        public int countAvailableRewards(IEnumerable<Reward> rewards)
        {
            int availableRewardCount = rewards.Where(r => r.Acquired == false).Count();
            return availableRewardCount;
        }

        public int countRedeemedRewards(IEnumerable<Reward> rewards)
        {
            int redeemedRewardCount = rewards.Where(r => r.Acquired == true).Count();
            return redeemedRewardCount;
        }
    }
}
