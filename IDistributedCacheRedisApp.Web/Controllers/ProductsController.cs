using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            //await _distributedCache.SetStringAsync("name","Berkay",cacheEntryOptions);

            Product product = new Product() {Id = 1, Name = "Kalem2", Price = 100};

            string jsonProduct = JsonConvert.SerializeObject(product);

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

             await _distributedCache.SetAsync("product:1",byteProduct);

            // await  _distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions);

            return View();
        }

        public IActionResult Show()
        {

            Byte[] byteProduct = _distributedCache.Get("product:1");

            
            //string name = _distributedCache.GetString("name");


            //string jsonProduct = _distributedCache.GetString("product:1");

            string jsonProduct = Encoding.UTF8.GetString(byteProduct);


            Product product = JsonConvert.DeserializeObject<Product>(jsonProduct);

            ViewBag.name = product;

            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name");

            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] resimByte = _distributedCache.Get("resim");

            return File(resimByte, "image/jpg");

        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/p7zz5dxZ.jpg");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("resim",imageByte);

            return View();
        }
    }
}
