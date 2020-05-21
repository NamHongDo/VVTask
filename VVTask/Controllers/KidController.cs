using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VVTask.Models;
using VVTask.ViewModels;

namespace VVTask.Controllers
{
    public class KidController : Controller
    {
        private readonly IKidRepository _kidRepository;
        private readonly IVTaskRepository _vTaskRepository;
        private readonly IRewardRepository _rewardRepository;
        [BindProperty]
        public Toaster _myToaster { get; set; }
        public KidController(
            IKidRepository kidRepository,
            IVTaskRepository vTaskRepository,
            IRewardRepository rewardRepository)
        {
            _kidRepository = kidRepository;
            _vTaskRepository = vTaskRepository;
            _rewardRepository = rewardRepository;
            _myToaster = new Toaster();
        }
        public async Task<ViewResult> List()
        {
            CheckToast();
            KidProfileViewModel kidProfileViewModel = new KidProfileViewModel
            {
                Profiles = await _kidRepository.GetAll()
            };
            return View(kidProfileViewModel);
        }

        //View task list of each kid profile
        public async Task<ViewResult> Details(int KidId, string sortOrder)
        {
            CheckToast();

            var currentKid = await _kidRepository.GetProfileById(KidId);
            var vTasks = await _vTaskRepository.GetAllByKidId(KidId);
            var rewards = await _rewardRepository.GetAllByKidId(KidId);

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PointSortParm"] = sortOrder == "Point" ? "point_desc" : "Point";
            switch (sortOrder)
            {
                case "name_desc":
                    vTasks = vTasks.OrderByDescending(v => v.Description);
                    break;
                case "Point":
                    vTasks = vTasks.OrderBy(v => v.Point);
                    break;
                case "point_desc":
                    vTasks = vTasks.OrderByDescending(v => v.Point);
                    break;
                default:
                    vTasks = vTasks.OrderBy(v => v.Description);
                    break;
            }
            KidDetailsViewModel kidDetailsViewModel = new KidDetailsViewModel()
            {
                kid = currentKid,
                currentKidVTasks = vTasks,
                currentKidRewards = rewards,
                myToaster = _myToaster
            };

            return View(kidDetailsViewModel);
            // return RedirectToAction("List","VTask", data);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Kid profile)
        {
            if (ModelState.IsValid)
            {
                _kidRepository.Add(profile);
                await _kidRepository.CommitAsync();
                var toastobj = Helper.getToastObj("Kid profile was created successfully", "alert-success");
                TempData.Put("toast",toastobj);
                return RedirectToAction("List");
            }
            return View(profile);
        }
    
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var profile = await _kidRepository.GetProfileById(id);
            if (profile == null)
                return NotFound();
            return View(profile);
        }

        // submitting new information for a existing vtask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Kid profile)
        {
            if (ModelState.IsValid)
            {
                _kidRepository.Update(profile);
                await _kidRepository.CommitAsync();
                var toastobj = Helper.getToastObj("Kid profile was edited successfully", "alert-success");
                TempData.Put("toast", toastobj);
                return RedirectToAction("Details", new { profile.KidId });
            }
            return View(profile);
        }
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var kidProfile = await _kidRepository.GetProfileById(id);
            if (kidProfile == null)
                return NotFound();
            return View(kidProfile);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int KidId)
        {
            if (ModelState.IsValid)
            {
                await _kidRepository.Delete(KidId);
                await _kidRepository.CommitAsync();
                var toastobj = Helper.getToastObj("Kid profile was Deleted successfully", "alert-success");
                TempData.Put("toast", toastobj);
                return RedirectToAction("List");
            }
            return View();
        }
        private void CheckToast()
        {
            var toastObj = TempData.Get<Toaster>("toast");
            if (toastObj != null)
            {
                _myToaster = toastObj;
            }
            else
            {
                _myToaster = new Toaster() { };
            }
        }
    }
}