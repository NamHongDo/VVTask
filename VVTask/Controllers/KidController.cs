using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _hostinEnvironment;
        private readonly IStatistic _statistic;

        [BindProperty]
        public Toaster MyToaster { get; set; }
        public KidController(
            IKidRepository kidRepository,
            IVTaskRepository vTaskRepository,
            IRewardRepository rewardRepository,
            AppDbContext appDbContext,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userMananger,
            IWebHostEnvironment hostinEnvironment,
            IStatistic statistic)
        {
            _kidRepository = kidRepository;
            _vTaskRepository = vTaskRepository;
            _rewardRepository = rewardRepository;
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
            _userMananger = userMananger;
            _hostinEnvironment = hostinEnvironment;
            _statistic = statistic;
        }

        public async Task<ViewResult> List()
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

        //Kid detail view includes: summary, tasks list, available rewards, redeemed reward
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
            
            //Task table sorting
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
                givenTasksCount = vTasks.Count(),
                pendingTasksCount = _statistic.countPendingTask(vTasks),
                completeTasksCount = _statistic.countCompleteTask(vTasks),
                availableRewardsCount = _statistic.countAvailableRewards(rewards),
                redeemedRewardsCount = _statistic.countRedeemedRewards(rewards),
                myToaster = MyToaster,
                paginatedList = paginatedList,
                PhotoPath = currentKid.PhotoPath
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
        public async Task<ActionResult> Create(KidCreateViewModel model)
        {
            var username = _httpContextAccessor.HttpContext?.User.Identity.Name;
            var currentUserProfile = _userMananger.Users.FirstOrDefault(u => u.UserName == username);

            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedImage(model);
                Kid newKid = new Kid()
                {
                    Name = model.Name,
                    PhotoPath = uniqueFileName,
                    ApplicationUserId = currentUserProfile.Id
                };

                _kidRepository.Add(newKid);
                await _kidRepository.CommitAsync();
                var toastobj = Helper.getToastObj("Kid profile was created successfully", "alert-success");
                TempData.Put("toast",toastobj);
                return RedirectToAction("List");
            }
            return View(model);
        }
    
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var kidProfile = await _kidRepository.GetProfileById(id);
            if (kidProfile == null)
                return NotFound();

            KidEditViewModel model = new KidEditViewModel() {
                KidId = kidProfile.KidId,
                Name = kidProfile.Name,
                TotalPoint = kidProfile.TotalPoint,
                ExistingPhotoPath = kidProfile.PhotoPath,
                ApplicationUserId = kidProfile.ApplicationUserId
            };
            return View(model);
        }

        // submitting new information for a existing vtask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(KidEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Kid existingKid = await _kidRepository.GetProfileById(model.KidId);
                existingKid.Name = model.Name;
                existingKid.TotalPoint = model.TotalPoint;
                existingKid.ApplicationUserId = model.ApplicationUserId;
                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(
                            _hostinEnvironment.WebRootPath,
                            "images",
                            model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                   existingKid.PhotoPath = ProcessUploadedImage(model);
                }  
                _kidRepository.Update(existingKid);
                await _appDbContext.SaveChangesAsync();
                //await _kidRepository.CommitAsync();
                var toastobj = Helper.getToastObj("Kid profile was edited successfully", "alert-success");
                TempData.Put("toast", toastobj);
                return RedirectToAction("Details", new { existingKid.KidId });
            }
            return View(model);
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
        public async Task<ActionResult> DeleteConfirmed(int KidId,string? ExistingPhotoPath)
        {
            if (ModelState.IsValid)
            {
                await _kidRepository.Delete(KidId);
                await _kidRepository.CommitAsync();
                var toastobj = Helper.getToastObj("Kid profile was Deleted successfully", "alert-success");
                if (ExistingPhotoPath != null)
                {
                    string filePath = Path.Combine(
                        _hostinEnvironment.WebRootPath,
                        "images",
                        ExistingPhotoPath);
                    System.IO.File.Delete(filePath);
                }
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

        private string ProcessUploadedImage(KidCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostinEnvironment.WebRootPath, "images/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

    }
}