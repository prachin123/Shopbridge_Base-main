using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopbridge_Base_main_API.Models;
using Shopbridge_Base_main_API.Models.Dtos;
using Shopbridge_Base_main_API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopbridge_Base_main_API.Controllers
{

    /// <summary>
    /// If Version is not defined then get default version name is v1.0
    /// </summary>
    [Route("api/v{version:apiVersion}/inventory")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class InventoryController : ControllerBase
    {
        #region init
        private readonly IInventoryRepository _npRepo;
        private readonly IMapper _mapper;

        public InventoryController(IInventoryRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }
        #endregion
        #region GetInventorys

        [ProducesResponseType(200, Type = typeof(List<InventoryDTO>))]
        [HttpGet]
        public IActionResult GetInventorys()
        {
            var objList = _npRepo.GetInventorys();
            var objDto = new List<InventoryDTO>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<InventoryDTO>(obj));
            }
            return Ok(objDto);
        }
        #endregion

        #region GetInventory
        [ProducesResponseType(200, Type = typeof(InventoryDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [HttpGet("{Id:int}", Name = "GetInventory")]
        public IActionResult GetInventory(int Id)
        {
            var obj = _npRepo.GetInventory(Id);
            if (obj == null)
            {
                return NotFound();
            }

            //using Auto mapper
            var objDto = _mapper.Map<InventoryDTO>(obj);
            return Ok(objDto);

        }
        #endregion

        #region CreateInventory
        
        [ProducesResponseType(201, Type = typeof(InventoryDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IActionResult CreateInventory([FromBody] InventoryDTO inventoryDTO)
        {
            if (inventoryDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (_npRepo.InventoryExists(inventoryDTO.Name))
            {
                ModelState.AddModelError("", "Inventory Exists!");
                return StatusCode(404, ModelState);
            }
            var inventoryDTOObj = _mapper.Map<Inventory>(inventoryDTO);
            if (!_npRepo.CreateInventory(inventoryDTOObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {inventoryDTOObj.Name}");
                return StatusCode(500, ModelState);
            }
            
            return CreatedAtRoute("GetInventory", new
            {
                version = HttpContext.GetRequestedApiVersion().ToString(),Id = inventoryDTOObj.Id
            }, inventoryDTOObj);
        }
        #endregion

        #region UpdateInventory
        [HttpPatch("{Id:int}", Name = "UpdateInventory")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateInventory(int Id, [FromBody] InventoryDTO inventoryDTO)
        {
            if (inventoryDTO == null || Id != inventoryDTO.Id)
            {
                return BadRequest(ModelState);
            }

            var inventoryObj = _mapper.Map<Inventory>(inventoryDTO);
            if (!_npRepo.UpdateInventory(inventoryObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {inventoryObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetInventory", new { Id = inventoryObj.Id }, inventoryObj);

        }
        #endregion

        #region DeleteInventory
        [HttpDelete("{Id:int}", Name = "DeleteInventory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteInventory(int Id)
        {
            if (!_npRepo.InventoryExists(Id))
            {
                return NotFound();
            }

            var inventoryObj = _npRepo.GetInventory(Id);
            if (!_npRepo.DeleteInventory(inventoryObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {inventoryObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
        #endregion
    }
}
