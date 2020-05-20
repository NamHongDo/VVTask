using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VVTask.Models;
using VVTask.ViewModels;

namespace VVTask.Controllers
{
    public class VTaskController : Controller
    {
        private readonly IVTaskRepository _vTaskRepository;
        private readonly IKidRepository _kidRepository;

        public VTask VTask { get; set; }

        public VTaskController( IVTaskRepository vTaskRepository,
                                IKidRepository kidRepository)
        {
            _kidRepository = kidRepository;
            _vTaskRepository = vTaskRepository;
        }

        //Displaying a list of VTasks
        public async Task<ActionResult> List(Kid newKidProfile)
        {
            ViewBag.Kid = newKidProfile;
            var vTasks = await _vTaskRepository.GetAllByKidId(newKidProfile.KidId);
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
            VTask vTask = new VTask()
            {
                KidId = KidId
            };
            return View(vTask);
        }

        //submitting information to create a new vtask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VTask vTask)
        {
            Kid currentKid = await _kidRepository.GetProfileById(vTask.KidId);
            if (ModelState.IsValid)
            {
                VTask newVTask = new VTask
                {
                    Description = vTask.Description,
                    Point = vTask.Point,
                    Kid = currentKid
                };
                _vTaskRepository.Add(newVTask);

                await _vTaskRepository.CommitAsync();

                var toastobj = Helper.getToastObj("Task was successfully created!", "alert-success");
                TempData.Put("toast", toastobj);
                return RedirectToAction("Details","Kid", new { vTask.KidId });
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
        public async Task<ActionResult> Edit(VTask vTask)
        {
            if (ModelState.IsValid)
            {
                _vTaskRepository.Update(vTask);
                await _vTaskRepository.CommitAsync();
                var toastobj = Helper.getToastObj("Task was sucessfully updated!", "alert-success");
                TempData.Put("toast", toastobj);
                return RedirectToAction("Details","Kid", new { vTask.KidId });
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
        public async Task<ActionResult> DeleteConfirmed(VTask vTask)
        {
            if (ModelState.IsValid)
            {
                await _vTaskRepository.Delete(vTask.VTaskId);
                await _vTaskRepository.CommitAsync();
                var toastobj = Helper.getToastObj("Task was sucessfully Deleted!", "alert-success");
                TempData.Put("toast", toastobj);
                return RedirectToAction("Details", "Kid", new { vTask.KidId });
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
        public async Task<ActionResult> ChangeStatus(VTask vTask)
        {
            Kid currentKid = await _kidRepository.GetProfileById(vTask.KidId);
            if (ModelState.IsValid)
            {
                vTask.Done = !vTask.Done;
                _vTaskRepository.Update(vTask);
                await _vTaskRepository.CommitAsync();
                if (!vTask.Done)
                {
                    currentKid.TotalPoint -= vTask.Point;
                } 
                else
                {
                    currentKid.TotalPoint += vTask.Point;
                } 
                _kidRepository.Update(currentKid);
                await _kidRepository.CommitAsync();
                var toastobj = Helper.getToastObj("Task complete status updated!", "alert-success");
                TempData.Put("toast", toastobj);
                return RedirectToAction("Details", "Kid", new { vTask.KidId });
            }
            return View(vTask);
        }
    }
}