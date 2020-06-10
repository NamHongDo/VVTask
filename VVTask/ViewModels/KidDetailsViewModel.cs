using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVTask.Models;
using VVTask.Others;

namespace VVTask.ViewModels
{
    public class KidDetailsViewModel
    {
        public Kid kid { get; set; }
        public int givenTasksCount { get; set; }
        public int pendingTasksCount { get; set; }
        public int completeTasksCount { get; set; }
        public int availableRewardsCount { get; set; }
        public int redeemedRewardsCount { get; set; }
        public IEnumerable<VTask> currentKidVTasks { get; set; }
        public IEnumerable<Reward> currentKidRewards { get; set; }
        [BindProperty]
        public Toaster myToaster { get; set; }
        public PaginatedList<VTask> paginatedList { get; set; }
        public string PhotoPath { get; set; }
    }
}
