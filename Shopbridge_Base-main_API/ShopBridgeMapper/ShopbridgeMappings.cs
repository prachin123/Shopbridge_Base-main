using AutoMapper;
using Shopbridge_Base_main_API.Models;
using Shopbridge_Base_main_API.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopbridge_Base_main_API.ShopBridgeMapper
{
    public class ShopbridgeMappings : Profile
    {
        public ShopbridgeMappings()
        {
            CreateMap<Inventory, InventoryDTO>().ReverseMap();
           
        }
    }
}
