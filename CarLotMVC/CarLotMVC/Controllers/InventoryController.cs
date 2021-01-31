using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoLotDAL.Models;
using Newtonsoft.Json;

namespace CarLotMVC.Controllers
{
    public class InventoryController : Controller
    {
        private string apiUrl = "http://localhost:55785/api/Inventory";

        // GET: Inventory
        public async Task<ActionResult> Index()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var items = JsonConvert.DeserializeObject<List<Inventory>>(
                    await response.Content.ReadAsStringAsync());
                return View(items);
            }

            return HttpNotFound();
        }

        // GET: Inventory/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{apiUrl}/{id.Value}");

            if (response.IsSuccessStatusCode)
            {
                var itemDetails = JsonConvert.DeserializeObject<Inventory>(
                    await response.Content.ReadAsStringAsync());
                return View(itemDetails);
            }

            return HttpNotFound();
        }

        public ActionResult Create()
        {
            return View();
        }

        // GET: Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "Make, Color, PetName")] Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return View(inventory);
            }

            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(inventory);
                var stringContent = new StringContent(json, Encoding.UTF8,
                    "application/json");
                var response = await client.PostAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to create" +
                    $"record: {ex.Message}");
            }

            return View(inventory);
        }

        // GET: Inventory/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{apiUrl}/{id.Value}");

            if (response.IsSuccessStatusCode)
            {
                var itemDetails = JsonConvert.DeserializeObject<Inventory>(
                    await response.Content.ReadAsStringAsync());
                return View(itemDetails);
            }

            return HttpNotFound();
        }

        // POST: Inventory/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, Make, Color, PetName, Timestamp")] Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return View(inventory);
            }

            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(inventory);
            var stringContent = new StringContent(json, Encoding.UTF8,
                "application/json");
            var response = await client.PutAsync($"{apiUrl}/{inventory.Id}",
                stringContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(inventory);
        }

        // GET: Inventory/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{apiUrl}/{id.Value}");

            if (response.IsSuccessStatusCode)
            {
                var inventory = JsonConvert.DeserializeObject<Inventory>(
                    await response.Content.ReadAsStringAsync());
                return View(inventory);
            }

            return HttpNotFound();
        }

        // POST: Inventory/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(
            [Bind(Include = "Id, Timestamp")] Inventory inventory)
        {
            try
            {
                var client = new HttpClient();
                var requestMessage = new HttpRequestMessage(
                    HttpMethod.Delete, $"{apiUrl}/{inventory.Id}")
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(inventory),
                        Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(requestMessage);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to delete" +
                    $"record: {ex.Message}");
            }

            return View(inventory);
        }

        protected override void Dispose(bool disposing)
        {
             base.Dispose(disposing);
        }
    }
}
