using ApiProjectPRN231.Models;
using Microsoft.AspNetCore.Http;
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
    public class OrderDetailsController : Controller
    {
        private readonly HttpClient _client;
        private string _orderDetailsApiUrl = "https://localhost:8000/api/OrderDetails";

        public OrderDetailsController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        // GET: OrderDetailsController
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _client.GetAsync(_orderDetailsApiUrl);
            response.EnsureSuccessStatusCode();
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<OrderDetails> listOrderDetails = JsonSerializer.Deserialize<List<OrderDetails>>(strData, options);
            return View(listOrderDetails);
        }

        // GET: OrderDetailsController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderDetailsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonSerializer.Serialize(orderDetails), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(_orderDetailsApiUrl, content);
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
            return View(orderDetails);
        }

        // GET: OrderDetailsController/Edit/5
        public async Task<IActionResult> Edit(int orderId, int productId)
        {
            HttpResponseMessage response = await _client.GetAsync($"{_orderDetailsApiUrl}/{orderId}/{productId}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            OrderDetails orderDetails = JsonSerializer.Deserialize<OrderDetails>(strData, options);
            return View(orderDetails);
        }

        // POST: OrderDetailsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int orderId, int productId, OrderDetails orderDetails)
        {
            if (orderId != orderDetails.OrderId || productId != orderDetails.ProductId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonSerializer.Serialize(orderDetails), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync($"{_orderDetailsApiUrl}/{orderId}/{productId}", content);
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
            return View(orderDetails);
        }

        // GET: OrderDetailsController/Delete/5
        public async Task<IActionResult> Delete(int orderId, int productId)
        {
            HttpResponseMessage response = await _client.GetAsync($"{_orderDetailsApiUrl}/{orderId}/{productId}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            OrderDetails orderDetails = JsonSerializer.Deserialize<OrderDetails>(strData, options);
            return View(orderDetails);
        }

        // POST: OrderDetailsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int orderId, int productId)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"{_orderDetailsApiUrl}/{orderId}/{productId}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet("{orderId:int}/{productId:int}")]
        public async Task<IActionResult> Details(int orderId, int productId)
        {
            HttpResponseMessage response = await _client.GetAsync($"{_orderDetailsApiUrl}/{orderId}/{productId}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            OrderDetails orderDetails = JsonSerializer.Deserialize<OrderDetails>(strData, options);
            return View(orderDetails);
        }
    }
}
