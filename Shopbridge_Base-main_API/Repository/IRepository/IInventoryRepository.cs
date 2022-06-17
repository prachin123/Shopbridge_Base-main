using Shopbridge_Base_main_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopbridge_Base_main_API.Repository.IRepository
{
   public interface IInventoryRepository
    {
        ICollection<Inventory> GetInventorys();
        Inventory GetInventory(int inventory);

        bool InventoryExists(string name);
        bool InventoryExists(int id);

        bool CreateInventory(Inventory inventory);
        bool UpdateInventory(Inventory inventory);
        bool DeleteInventory(Inventory inventory);
        bool Save();
    }
}
