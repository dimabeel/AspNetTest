using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoLotDAL.Models;
using AutoLotDAL.Repos;
using AutoMapper;

namespace CatLotWebAPI.Controllers
{
    [RoutePrefix("api/Inventory")]
    public class InventoryController : ApiController
    {
        public InventoryController()
        {
            var mapperConfiguration = new MapperConfiguration(
                cfg => cfg.CreateMap<Inventory, Inventory>()
                .ForMember(x => x.Orders, opt => opt.Ignore()));
            mapper = new Mapper(mapperConfiguration);
        }

        //[HttpGet, Route("")]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //[HttpGet, Route("{id}")]
        //public string Get(int id)
        //{
        //    return id.ToString();
        //}

        [HttpGet, Route("")]
        public IEnumerable<Inventory> GetInventory()
        {
            var inventories = repo.GetAll();
            var mappedInventory = mapper
                .Map<List<Inventory>, List<Inventory>>(inventories);
            return mappedInventory;
        }

        [HttpGet, Route("{id}", Name = "DisplayRoute")]
        [ResponseType(typeof(Inventory))]
        public async Task<IHttpActionResult> GetInventory(int id)
        {
            Inventory inventory = repo.GetOne(id);
            if (inventory == null)
            {
                return NotFound();
            }
            else
            {
                var mappedInventory = mapper
                    .Map<Inventory, Inventory>(inventory);
                return Ok(mappedInventory);
            }
        }

        [HttpPut, Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInventory(int id, Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != inventory.Id)
            {
                return BadRequest();
            }

            try
            {
                repo.Save(inventory);
            }
            catch (Exception _)
            {
                // Some exception handlers if model wasn't save
                throw;
            }

            // OK
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost, Route("")]
        [ResponseType(typeof(Inventory))]
        public IHttpActionResult PostInventory(Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                repo.Add(inventory);
            }
            catch
            {
                // Some err handlers
                throw;
            }

            return CreatedAtRoute("DisplayRoute", new { id = inventory.Id }, inventory);
        }

        [HttpDelete, Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteInventory(int id, Inventory inventory)
        {
            if (inventory.Id != id)
            {
                return BadRequest();
            }

            try
            {
                repo.Delete(inventory);
            }
            catch
            {
                throw;
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private readonly InventoryRepo repo = new InventoryRepo();

        private Mapper mapper;
    }
}