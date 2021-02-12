﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoLotDALCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AutoLotMVCCore.ViewComponents
{
    public class InventoryViewComponent : ViewComponent
    {
        private readonly string baseUrl;
        
        public InventoryViewComponent(IConfiguration configuration)
        {
            baseUrl = configuration.GetSection("ServiceAddress").Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"{baseUrl}");
            if (response.IsSuccessStatusCode)
            {
                var items = JsonConvert.DeserializeObject<List<Inventory>>(
                    await response.Content.ReadAsStringAsync());
                return View("InventoryPartialView", items);
            }

            return new ContentViewComponentResult("Unable to return records.");
        }
    }
}
