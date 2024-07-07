using ApiProjectPRN231.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient _client;
        private string _orderApiUrl = "https://localhost:8000/api/Orders";

        public OrderController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        // GET: OrderController
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _client.GetAsync(_orderApiUrl);
            response.EnsureSuccessStatusCode();
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Order> listOrders = JsonSerializer.Deserialize<List<Order>>(strData, options);
            return View(listOrders);
        }
        // GET: OrderController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonSerializer.Serialize(order), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(_orderApiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Log response content for debugging
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API Error: " + responseContent);
                }
            }
            return View(order);
        }
        // GET: OrderController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{_orderApiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Order order = JsonSerializer.Deserialize<Order>(strData, options);
            return View(order);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonSerializer.Serialize(order), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync($"{_orderApiUrl}/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Log response content for debugging
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API Error: " + responseContent);
                }
            }
            return View(order);
        }
        // GET: OrderController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{_orderApiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Order order = JsonSerializer.Deserialize<Order>(strData, options);
            return View(order);
        }

        // POST: OrderController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"{_orderApiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        // GET: OrderController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{_orderApiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Order order = JsonSerializer.Deserialize<Order>(strData, options);
            return View(order);
        }





    }
}
