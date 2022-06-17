using Shopbridge_Base_main_WEB.Models;
using Shopbridge_Base_main_WEB.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shopbridge_Base_main_WEB.Repository
{
    public class InventoryRepository : Repository<Inventory>, IInventoryRepository
    {

        private readonly IHttpClientFactory _clientFactory;

        public InventoryRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}

