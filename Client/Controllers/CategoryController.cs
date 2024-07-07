using ApiProjectPRN231.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Client.Controllers
{
    public class CategoryController : Controller
    {
        private readonly HttpClient _client;
        private readonly string _categoryApiUrl;

        public CategoryController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _categoryApiUrl = "https://localhost:8000/api/Categories"; 
        }

        // GET: CategoryController
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _client.GetAsync(_categoryApiUrl);
            response.EnsureSuccessStatusCode();
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Category> listCategories = JsonSerializer.Deserialize<List<Category>>(strData, options);
            return View(listCategories);
        }

        // GET: CategoryController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{_categoryApiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Category category = JsonSerializer.Deserialize<Category>(strData, options);
            return View(category);
        }

        // GET: CategoryController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var content = JsonContent.Create(category);
                HttpResponseMessage response = await _client.PostAsync(_categoryApiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(category);
        }

        // GET: CategoryController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{_categoryApiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Category category = JsonSerializer.Deserialize<Category>(strData, options);
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var content = JsonContent.Create(category);
                HttpResponseMessage response = await _client.PutAsync($"{_categoryApiUrl}/{id}", content);
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
            return View(category);
        }

        // GET: CategoryController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{_categoryApiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Category category = JsonSerializer.Deserialize<Category>(strData, options);
            return View(category);
        }

        // POST:CategoryController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"{_categoryApiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
