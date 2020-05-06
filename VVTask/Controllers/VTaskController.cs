using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VVTask.Models;
using VVTask.ViewModels;


namespace VVTask.Controllers
{
    public class VTaskController : Controller
    {
        private readonly IVTaskRepository _vTaskRepository;
        private readonly IHtmlHelper _htmlHelper;
        public VTask VTask { get; set; }

        public VTaskController( IVTaskRepository vTaskRepository,
                                IHtmlHelper htmlHelper)
        {
            _vTaskRepository = vTaskRepository;
            this._htmlHelper = htmlHelper;
        }
        //Displaying a list of VTasks
        public ViewResult List()
        {
            VTaskListViewModel vTasksListViewModel = new VTaskListViewModel
            {
                VTasks = _vTaskRepository.GetAll(),
                CurrentCategory = "VTask List"
            };
            return View(vTasksListViewModel);
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
        public ActionResult Create()
        {
            return View();
        }

        //submitting information to create a new vtask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VTask vTask)
        {
            if (ModelState.IsValid)
            {
                _vTaskRepository.Add(vTask);
                _vTaskRepository.Commit();
                return RedirectToAction("List");
            }
            return View(vTask);
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
                return RedirectToAction("Details", new { id = vTask.Id });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(VTask vTask)
        {
            if (ModelState.IsValid)
            {
                _vTaskRepository.Delete(vTask.Id);
                _vTaskRepository.Commit();
                return RedirectToAction("List");
            }

            return View(vTask);
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
