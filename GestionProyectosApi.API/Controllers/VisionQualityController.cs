using ApiTableroPowerBiFepep.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiTableroPowerBiFepep.API.Controllers
{
    public class VisionQualityController : Controller
    {
        private IVisionQualityService _visionQualityService;

        public VisionQualityController(IVisionQualityService visionQualityService)
        {
            _visionQualityService = visionQualityService;
        }

        public void ConsolidatedReport()
        {
            //_visionQualityService.ConsolidatedReport();
        }

        // GET: VisionQualityController
        public ActionResult Index()
        {
            return View();
        }

        // GET: VisionQualityController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VisionQualityController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VisionQualityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VisionQualityController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VisionQualityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VisionQualityController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VisionQualityController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
