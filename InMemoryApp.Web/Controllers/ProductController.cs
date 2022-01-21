using System;
using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            // 1.yol
            // ilgili keyin olup olmadığını tespit edebiliriz.
            //if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            //{
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            //}

            // 2.yol
            // alabilirse hem geriye true dönecek hem de zamancache' e zaman keyine sahip valueyi atayacak

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            // 10 saniye sonra düşücek kaybolacak
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);


            // 10 saniye içerisinde erişirsem ömrü artacak
            //options.SlidingExpiration = TimeSpan.FromSeconds(10);

            // bu data benim için önemli
            options.Priority = CacheItemPriority.High;

            // bu data önemsiz sil
            //options.Priority = CacheItemPriority.Low;

            // asla silme, ama memory dolarsa exception fırlatır
            // options.Priority = CacheItemPriority.NeverRemove;

            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}->{value} => sebep:{reason}");
            });

            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

            Product product = new Product() {Id = 1, Name = "Kalem", Price = 200};

            _memoryCache.Set<Product>("product1", product);

            return View();
        }

        public IActionResult Show()
        {
            //_memoryCache.GetOrCreate<string>("zaman", entry =>
            //{
            //    return DateTime.Now.ToString();
            //});

            _memoryCache.TryGetValue("zaman", out string zamancache);
            _memoryCache.TryGetValue("callback", out string callback);

            ViewBag.zaman = zamancache;
            ViewBag.callback = callback;

            ViewBag.product = _memoryCache.Get<Product>("product1");

            return View();
        }

    }
}
