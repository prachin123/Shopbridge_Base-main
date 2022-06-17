using Microsoft.AspNetCore.Mvc;
using Shopbridge_Base_main_WEB.Models;
using Shopbridge_Base_main_WEB.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopbridge_Base_main_WEB.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryRepository _npRepo;
        public InventoryController(IInventoryRepository npRepo)
        {

            _npRepo = npRepo;
        }
        public IActionResult Index()
        {
            return View(new Inventory() { });
        }
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
                    await _npRepo.CreateAsync(SD.InventoryAPIPath, obj,"");
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

        public async Task<IActionResult> GetAllInventory()
        {
            return Json(new { data = await _npRepo.GetAllAsync(SD.InventoryAPIPath, "") });
        }

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

    }
}
