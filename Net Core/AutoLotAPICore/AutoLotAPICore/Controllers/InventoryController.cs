using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoLotDALCore.EF;
using AutoLotDALCore.Models;
using AutoLotDALCore.Repos;
using AutoMapper;
using Newtonsoft.Json;

namespace AutoLotAPICore.Controllers
{
    [Produces("application/json")]
    [Route("api/Inventory")]
    public class InventoryController : Controller
    {
        private readonly IInventoryRepo repo;
        private Mapper mapper;

        public InventoryController(IInventoryRepo repo)
        {
            this.repo = repo;

            var mapperConfig = new MapperConfiguration(
                cfg => cfg.CreateMap<Inventory, Inventory>()
                .ForMember(x => x.Orders, opt => opt.Ignore()));
        }

        // GET: api/Inventory
        [HttpGet]
        public IEnumerable<Inventory> GetCars()
        {
            var inventories = repo.GetAll();
            return mapper.Map<List<Inventory>, List<Inventory>>(inventories);
        }

        // GET: api/Inventory/5
        [HttpGet("{id}",Name= "DisplayRoute")]
        public async Task<IActionResult> GetInventory([FromRoute] int id)
        {
            Inventory inventory = repo.GetOne(id);

            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<Inventory, Inventory>(inventory));
        }

        // PUT: api/Inventory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventory([FromRoute] int id, [FromBody] Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inventory.Id)
            {
                return BadRequest();
            }
            
            repo.Update(inventory);

            return NoContent();
        }

        // POST: api/Inventory
        [HttpPost]
        public async Task<IActionResult> PostInventory([FromBody] Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

        	repo.Add(inventory);
            return CreatedAtRoute("DisplayRoute", new { id = inventory.Id }, inventory);
        }

        // DELETE: api/Inventory/5
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> DeleteInventory([FromRoute] int id,[FromRoute] string timestamp)
        {
            if (!timestamp.StartsWith("\""))
            {
                timestamp = $"\"{timestamp}\"";
            }

            var ts = JsonConvert.DeserializeObject<byte[]>(timestamp);

            repo.Delete(id, ts);
            return Ok();
        }

    }
}