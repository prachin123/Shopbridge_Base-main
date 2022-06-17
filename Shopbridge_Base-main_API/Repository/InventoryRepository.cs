using Shopbridge_Base_main_API.Data;
using Shopbridge_Base_main_API.Models;
using Shopbridge_Base_main_API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopbridge_Base_main_API.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        #region Init
        //Connect to Database
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Dependancy Injection
        /// </summary>
        /// <param name="db"></param>
        public InventoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        #endregion
        #region CreateInventory
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="db"></param>
        public bool CreateInventory(Inventory inventory)
        {
            _db.Inventory.Add(inventory);
            return Save();
        }
        #endregion
        #region DeleteInventory
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="db"></param>
        public bool DeleteInventory(Inventory inventory)
        {
            _db.Inventory.Remove(inventory);
            return Save();
        }
        #endregion
        #region GetInventory
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="db"></param>
        public Inventory GetInventory(int Id)
        {
            return _db.Inventory.FirstOrDefault(a => a.Id == Id);
        }
        #endregion
        #region GetInventorys
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="db"></param>
        public ICollection<Inventory> GetInventorys()
        {
            return _db.Inventory.OrderBy(a => a.Name).ToList();
        }
        #endregion
        #region InventoryExists
        /// <summary>
        /// Exists Name Wise
        /// </summary>
        /// <param name="db"></param>
        public bool InventoryExists(string name)
        {
            bool value = _db.Inventory.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }
        #endregion
        #region InventoryExists
        /// <summary>
        /// Exists ID Wise
        /// </summary>
        /// <param name="db"></param>
        public bool InventoryExists(int id)
        {
            return _db.Inventory.Any(a => a.Id == id);
        }
        #endregion
        #region Save
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="db"></param>
        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
        #endregion
        #region UpdateInventory
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="db"></param>
        public bool UpdateInventory(Inventory inventory)
        {
            _db.Inventory.Update(inventory);
            return Save();
        }
        #endregion
    }
}
