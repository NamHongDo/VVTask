using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VVTask.Models;

namespace VVTask.Controllers
{
    public class RewardController : Controller
    {
        private readonly IRewardRepository _rewardRepository;
        private readonly IKidRepository _kidRepository;

        public RewardController(
            IRewardRepository rewardRepository,
            IKidRepository kidRepository)
        {
            _rewardRepository = rewardRepository;
            _kidRepository = kidRepository;
        }

        //Displaying the detail of a single task
        public ActionResult Details(int id)
        {
            var reward = _rewardRepository.GetRewardById(id);
            if (reward == null)
                return NotFound();
            return View(reward);
        }

        //redirect to vtask creation page
        [HttpGet]
        public ActionResult Create(int KidId)
        {
            Reward reward = new Reward()
            {
                KidId = KidId
            };
            return View(reward);
        }

        //submitting information to create a new vtask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reward reward)
        {
            Kid currentKid = _kidRepository.GetProfileById(reward.KidId);
            if (ModelState.IsValid)
            {
                Reward newReward = new Reward
                {
                    Description = reward.Description,
                    Point = reward.Point,
                    Kid = currentKid
                };
                _rewardRepository.Add(newReward);

                _rewardRepository.Commit();
                return RedirectToAction("Details", "Kid", new { reward.KidId });
            }
            return View(reward);
        }

        //editing an existing vtask
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var vTask = _rewardRepository.GetRewardById(id);
            if (vTask == null)
                return NotFound();
            return View(vTask);
        }

        // submitting new information for a existing vtask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Reward reward)
        {
            if (ModelState.IsValid)
            {
                _rewardRepository.Update(reward);
                _rewardRepository.Commit();
                return RedirectToAction("Details", "Kid", new { reward.KidId });
            }
            return View(reward);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var reward = _rewardRepository.GetRewardById(id);
            if (reward == null)
                return NotFound();
            return View(reward);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Reward reward)
        {
            if (ModelState.IsValid)
            {
                _rewardRepository.Delete(reward.RewardId);
                _rewardRepository.Commit();
                return RedirectToAction("Details", "Kid", new { reward.KidId });
            }
            return View();
        }

        [HttpGet]
        public ActionResult Redeem(int id)
        {
            var vTask = _rewardRepository.GetRewardById(id);
            if (vTask == null)
                return NotFound();
            return View(vTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Redeem(Reward reward)
        {
            Kid currentKid = _kidRepository.GetProfileById(reward.KidId);
            if (ModelState.IsValid)
            {
                reward.Acquired = !reward.Acquired;
                if (reward.Acquired)
                {
                    if (currentKid.TotalPoint < reward.Point)
                    {
                        reward.Acquired = !reward.Acquired;
                        return RedirectToAction("Details", "Kid", new { reward.KidId });
                    }
                    else
                    {
                        currentKid.TotalPoint -= reward.Point;
                    }
                }
                else
                {
                    currentKid.TotalPoint += reward.Point;
                }
                _rewardRepository.Update(reward);
                _rewardRepository.Commit();
                _kidRepository.Update(currentKid);
                _kidRepository.Commit();
                return RedirectToAction("Details", "Kid", new { reward.KidId });
            }
            return View(reward);
        }
    }
}