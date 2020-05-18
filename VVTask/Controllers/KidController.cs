using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VVTask.Models;
using VVTask.ViewModels;

namespace VVTask.Controllers
{
    public class KidController : Controller
    {
        private readonly IKidRepository _kidRepository;
        private readonly AppDbContext _appDbContext;

        public KidController(IKidRepository kidRepository, AppDbContext appDbContext)
        {
            _kidRepository = kidRepository;
            _appDbContext = appDbContext;
        }
        public ViewResult List()
        {
            KidProfileViewModel kidProfileViewModel = new KidProfileViewModel
            {
                Profiles = _kidRepository.GetAll(),
            };
            return View(kidProfileViewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Kid profile)
        {
            if (ModelState.IsValid)
            {
                _kidRepository.Add(profile);
                _kidRepository.Commit();
                return RedirectToAction("List");
            }
            return View(profile);
        }
        //View task list of each kid profile
        public ActionResult Details(int KidId)
        {
            Kid data = new Kid()
            {
                KidId = KidId
            };
            return RedirectToAction("List","VTask", data);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var profile = _kidRepository.GetProfileById(id);
            if (profile == null)
                return NotFound();
            return View(profile);
        }

        // submitting new information for a existing vtask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Kid profile)
        {
            if (ModelState.IsValid)
            {
                _kidRepository.Update(profile);
                _kidRepository.Commit();
                return RedirectToAction("List", new { id = profile.KidId });
            }
            return View(profile);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var kidProfile = _kidRepository.GetProfileById(id);
            if (kidProfile == null)
                return NotFound();
            return View(kidProfile);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int KidId)
        {
            if (ModelState.IsValid)
            {
                _kidRepository.Delete(KidId);
                _kidRepository.Commit();
                return RedirectToAction("List");
            }

            return View();
        }
    }
}