using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VVTask.Models;
using VVTask.ViewModels;

using Microsoft.EntityFrameworkCore;

namespace VVTask.Controllers
{
    public class VTaskController : Controller
    {
        private readonly IVTaskRepository _vTaskRepository;
        private readonly AppDbContext _appDbContext;

        public VTask VTask { get; set; }

        public VTaskController( IVTaskRepository vTaskRepository,
                                AppDbContext appDbContext)
        {
            _vTaskRepository = vTaskRepository;
            _appDbContext = appDbContext;
        }

        public ViewResult TestList()
        {
            /*
            var VTasks = _appDbContext.VTasks.Include(v => v.Kid)
                          .ToList();
                          */
            var VTasks = _appDbContext.VTasks
                .Include(v => v.Kid)
                .Where(v=> v.KidId == 2)
            .ToList();
            return View(VTasks);
        }

        //Displaying a list of VTasks
        public ActionResult List(Kid newKidProfile)
        {
            ViewBag.Kid = newKidProfile;
            var vTasks = _appDbContext.VTasks
               .Include(v => v.Kid)
               .Where(v => v.KidId == newKidProfile.KidId)
            .ToList();
            if (vTasks != null)
            {
                VTaskListViewModel vTasksListViewModel = new VTaskListViewModel
                {
                    VTasks = vTasks,
                    KidId = newKidProfile.KidId
                };
                return View(vTasksListViewModel);
            }
            else
            {
                VTaskListViewModel vTasksListViewModel = new VTaskListViewModel
                {
                    VTasks = null,
                    KidId = newKidProfile.KidId
                };
                return View(vTasksListViewModel);
            }       
        }
        //Displaying the detail of a single task
        public ActionResult Details(int id)
        {
            var vTask = _vTaskRepository.GetTaskById(id);
            if (vTask == null)
                return NotFound();
            return View(vTask);
        }

        //redirect to vtask creation page
        [HttpGet]
        public ActionResult Create(int KidId)
        {
            VTask addVTaskViewModel = new VTask()
            {
                KidId = KidId
            };
            return View(addVTaskViewModel);
        }

        //submitting information to create a new vtask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VTask addVTaskViewModel)
        {
            Kid currentKid = _appDbContext.Kids.Single(k => k.KidId == addVTaskViewModel.KidId);
            if (ModelState.IsValid)
            {
                VTask newVTask = new VTask
                {
                    Description = addVTaskViewModel.Description,
                    Point = addVTaskViewModel.Point,
                    Kid = currentKid
                };
                _vTaskRepository.Add(newVTask);

                _vTaskRepository.Commit();
                return RedirectToAction("List","VTask", currentKid);
            }
            return View(addVTaskViewModel);
        }

        //editing an existing vtask
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var vTask = _vTaskRepository.GetTaskById(id);
            if (vTask == null)
                return NotFound();
            return View(vTask);
        }

        // submitting new information for a existing vtask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VTask vTask)
        {
            if (ModelState.IsValid)
            {
                _vTaskRepository.Update(vTask);
                _vTaskRepository.Commit();
                return RedirectToAction("Details", new { id = vTask.VTaskId });
            }
            return View(vTask);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var vTask = _vTaskRepository.GetTaskById(id);
            if (vTask == null)
                return NotFound();
            return View(vTask);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(VTask vTask)
        {
            Kid currentKid = _appDbContext.Kids.Single(k => k.KidId == vTask.KidId);
            if (ModelState.IsValid)
            {
                _vTaskRepository.Delete(vTask.VTaskId);
                _vTaskRepository.Commit();
                return RedirectToAction("List","VTask", currentKid);
            }

            return View();
        }

        [HttpGet]
        public ActionResult ChangeStatus(int id)
        {
            var vTask = _vTaskRepository.GetTaskById(id);
            if (vTask == null)
                return NotFound();
            return View(vTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatus(VTask vTask)
        {
            if (ModelState.IsValid)
            {
                _vTaskRepository.UpdateStatus(vTask);
                _vTaskRepository.Commit();
                return RedirectToAction("List");
            }
            return View(vTask);
        }
    }
}
