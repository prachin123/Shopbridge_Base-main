using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shopbridge_Base_main_WEB.Models;
using Shopbridge_Base_main_WEB.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Shopbridge_Base_main_WEB.Controllers
{
    public class HomeController : Controller
    {
        #region init
        private readonly ILogger<HomeController> _logger;
        private readonly IInventoryRepository _npRepo;
        public HomeController(ILogger<HomeController> logger, IInventoryRepository npRepo)
        {
            _logger = logger;
            _npRepo = npRepo;
        }
        #endregion
        #region Index
        public IActionResult Index()
        {
            return View(new Inventory() { });
        }
        #endregion
        #region Privacy
        public IActionResult Privacy()
        {
            return View();
        }
        #endregion
        #region Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
        #region Upsert
        public async Task<IActionResult> Upsert(int? id)
        {
            Inventory obj = new Inventory();

            if (id == null)
            {
                //this will be true for Insert/Create
                return View(obj);
            }

            //Flow will come here for update
            obj = await _npRepo.GetAsync(SD.InventoryAPIPath, id.GetValueOrDefault(), "");
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Inventory obj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;

                if (obj.Id == 0)
                {
                    await _npRepo.CreateAsync(SD.InventoryAPIPath, obj, "");
                }
                else
                {
                    await _npRepo.UpdateAsync(SD.InventoryAPIPath + obj.Id, obj, "");
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        }
        #endregion

        #region GetAllInventory
        public async Task<IActionResult> GetAllInventory()
        {
            return Json(new { data = await _npRepo.GetAllAsync(SD.InventoryAPIPath, "") });
        }
        #endregion
        #region
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _npRepo.DeleteAsync(SD.InventoryAPIPath, id, "");
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
        #endregion
    }
}
