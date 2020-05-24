using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VVTask.Models;
using VVTask.Others;
using VVTask.ViewModels;

namespace VVTask.Controllers
{
    [Authorize]
    public class KidController : Controller
    {
        private readonly IKidRepository _kidRepository;
        private readonly IVTaskRepository _vTaskRepository;
        private readonly IRewardRepository _rewardRepository;
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userMananger;

        [BindProperty]
        public Toaster MyToaster { get; set; }
        public KidController(
            IKidRepository kidRepository,
            IVTaskRepository vTaskRepository,
            IRewardRepository rewardRepository,
            AppDbContext appDbContext,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userMananger)
        {
            _kidRepository = kidRepository;
            _vTaskRepository = vTaskRepository;
            _rewardRepository = rewardRepository;
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
            _userMananger = userMananger;
        }

        public async Task<ViewResult> List( )
        {
            CheckToast();
            /*need better query*/
            var username = _httpContextAccessor.HttpContext?.User.Identity.Name;
            var currentUserProfile = _userMananger.Users.FirstOrDefault(u=>u.UserName==username);
            var list = await _appDbContext.Kids
                .Where(k=> k.ApplicationUserId == currentUserProfile.Id)
                .ToListAsync();
            KidProfileViewModel kidProfileViewModel = new KidProfileViewModel
            {
                Profiles = list,
                KidCount = list.Count(),
                userName = username
            };
            return View(kidProfileViewModel);
        }

        //View task list of each kid profile
        public async Task<ViewResult> Details(
            int KidId, 
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            CheckToast();
            var currentKid = await _kidRepository.GetProfileById(KidId);
            var rewards = await _rewardRepository.GetAllByKidId(KidId);
            int pageSize = 5;
           
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PointSortParm"] = sortOrder == "Point" ? "point_desc" : "Point";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var vTasks = from v in _appDbContext.VTasks.Where(v => v.KidId == KidId)
                         select v;
            if (!String.IsNullOrEmpty(searchString))
            {
                vTasks = vTasks.Where(v => v.Description.Contains(searchString));
            }
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
            var paginatedList = await PaginatedList<VTask>.CreateAsync(vTasks.AsNoTracking(), pageNumber ?? 1, pageSize);
            KidDetailsViewModel kidDetailsViewModel = new KidDetailsViewModel()
            {
                kid = currentKid,
                currentKidVTasks = vTasks,
                currentKidRewards = rewards,
                myToaster = MyToaster,
                paginatedList = paginatedList
            };
            return View(kidDetailsViewModel);
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
            var username = _httpContextAccessor.HttpContext?.User.Identity.Name;
            var currentUserProfile = _userMananger.Users.FirstOrDefault(u => u.UserName == username);
            if (ModelState.IsValid)
            {
                profile.ApplicationUserId = currentUserProfile.Id;
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
                await _appDbContext.SaveChangesAsync();
                //await _kidRepository.CommitAsync();
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
                MyToaster = toastObj;
            }
            else
            {
                MyToaster = new Toaster() { };
            }
        }
    }
}