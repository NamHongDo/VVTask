using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VVTask.Models;
using VVTask.ViewModels;

namespace VVTask.Controllers
{
    public class KidProfileController : Controller
    {
        private readonly IKidRepository _kidRepository;
        private readonly AppDbContext _appDbContext;

        public KidProfileController(IKidRepository kidRepository, AppDbContext appDbContext)
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
        public ActionResult Create(KidProfile profile)
        {
            if (ModelState.IsValid)
            {
                _kidRepository.Add(profile);
                _kidRepository.Commit();
                return RedirectToAction("List");
            }
            return View(profile);
        }
        public ActionResult Details(int id)
        {
            KidProfile newKidProfile = _appDbContext.Profiles.Single(c => c.KidId == id);
            return RedirectToAction("List","VTask",newKidProfile);
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
        public ActionResult Edit(KidProfile profile)
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
        public ActionResult DeleteConfirmed(int kidID)
        {
            if (ModelState.IsValid)
            {
                _kidRepository.Delete(kidID);
                _kidRepository.Commit();
                return RedirectToAction("List");
            }

            return View();
        }
    }
}