using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanGiay.Data;
using WebBanGiay.Models;
using WebBanGiay.Models.ViewModels;

namespace WebBanGiay.Controllers
{
    public class ProductsPageController : Controller
    {
        private readonly ApplicationDbContext _context;
        public int PageSize = 9;
        public ProductsPageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(int productpage = 1)
        {
            ViewBag.ProductQuantity = _context.Products.Count();
            return View("Index",
                new ProductsListViewModel
                {
                    Products = _context.Products
                    .Skip((productpage - 1) * PageSize)
                    .Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        ItemPerPage = PageSize,
                        CurrentPage = productpage,
                        TotalItems = _context.Products.Count()
                    }
                }
                );
        }

        public async Task<IActionResult> productsbycat(int categoryid, int productpage = 1)
        {
            ViewBag.ProductQuantity = _context.Products.Where(p => p.CategoryId == categoryid).Include(p => p.Category).Include(p => p.Color).Include(p => p.Size).Count();
            var applicationdbcontext = await _context.Products.Where(p => p.CategoryId == categoryid).Include(p => p.Category).Include(p => p.Color).Include(p => p.Size).ToListAsync();
            return View("index", new ProductsListViewModel
            {
                Products = applicationdbcontext
                    .Skip((productpage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemPerPage = PageSize,
                    CurrentPage = productpage,
                    TotalItems = _context.Products.Count()
                }
            });
        }


        [HttpPost]
        public async Task<IActionResult> Search(string keyWords, int productpage = 1)
        {
            if (keyWords == null || keyWords.Length == 0)
            {
                ViewBag.ProductQuantity = _context.Products.Count();
                return View("Index",
                    new ProductsListViewModel
                    {
                        Products = _context.Products
                        .Skip((productpage - 1) * PageSize)
                        .Take(PageSize),
                        PagingInfo = new PagingInfo
                        {
                            ItemPerPage = PageSize,
                            CurrentPage = productpage,
                            TotalItems = _context.Products.Count()
                        }
                    }
                    );
            }
            else
            {
                ViewBag.KeyWords = keyWords;
                ViewBag.ProductQuantity = _context.Products.Where(p => p.ProductName.Contains(keyWords))
                        .Skip((productpage - 1) * PageSize)
                        .Take(PageSize).Count();
                return View("Index",
                    new ProductsListViewModel
                    {
                        Products = _context.Products
                        .Where(p => p.ProductName.Contains(keyWords))
                        .Skip((productpage - 1) * PageSize)
                        .Take(PageSize),
                        PagingInfo = new PagingInfo
                        {
                            ItemPerPage = PageSize,
                            CurrentPage = productpage,
                            TotalItems = _context.Products.Count()
                        }
                    }
                    );
            }
        }

        public class PriceRange
        {
            public int Min { get; set; }
            public int Max { get; set; }
        }

        public IActionResult GetFilteredProducts([FromBody] FilterData filter)
        {
            var filterProduct = _context.Products.ToList();
            var filterColor = _context.Colors.ToList();
            var filterSize = _context.Sizes.ToList();
            if (filter.PriceRange != null && filter.PriceRange.Count > 0 && !filter.PriceRange.Contains("All"))
            {
                List<PriceRange> priceRanges = new List<PriceRange>();
                foreach (var range in filter.PriceRange)
                {
                    var value = range.Split('-').ToArray();
                    PriceRange priceRange = new PriceRange();
                    priceRange.Min = int.Parse(value[0]);
                    priceRange.Max = int.Parse(value[1]);
                    priceRanges.Add(priceRange);
                }
                filterProduct = filterProduct.Where(p => priceRanges.Any(
                    r => (p.ProductPrice * (1 - p.ProductDiscount)) >= r.Min && 
                    (p.ProductPrice * (1 - p.ProductDiscount)) <= r.Max)
                ).ToList();
            }
            if (filter.Colors != null && filter.Colors.Count > 0 && !filter.Colors.Contains("All"))
            {
                filterColor = filterColor.Where(p => p.ColorId != null && filter.Colors.Contains(p.ColorName)).ToList();
                var filteredColorIds = filterColor.Select(c => c.ColorId).ToList();

                filterProduct = filterProduct.Where(p => p.Color != null && filteredColorIds.Contains(p.ColorId)).ToList();
            }
            if (filter.Sizes != null && filter.Sizes.Count > 0 && !filter.Sizes.Contains("All"))
            {
                filterSize = filterSize.Where(p => p.SizeId != null && filter.Sizes.Contains(p.SizeName)).ToList();
                var filteredSizeIds = filterSize.Select(s => s.SizeId).ToList();

                filterProduct = filterProduct.Where(p => p.Size != null && filteredSizeIds.Contains(p.SizeId)).ToList();
            }

            return PartialView("_ReturnProducts", filterProduct);

        }


        // GET: ProductsPage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
