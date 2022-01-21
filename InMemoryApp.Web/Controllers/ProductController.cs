using System;
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
            if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }

            // 2.yol
            // alabilirse hem geriye true dönecek hem de zamancache' e zaman keyine sahip valueyi atayacak
            if (_memoryCache.TryGetValue("zaman", out string zamancache))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }

           
            return View();
        }

        public IActionResult Show()
        {
            _memoryCache.GetOrCreate<string>("zaman", entry =>
            {
                return DateTime.Now.ToString();
            });

            ViewBag.zaman =  _memoryCache.Get<string>("zaman");
            return View();
        }

    }
}
