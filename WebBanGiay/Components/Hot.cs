using Microsoft.AspNetCore.Mvc;
using WebBanGiay.Data;

namespace WebBanGiay.Components
{
    public class Hot : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public Hot(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(_context.Products.Where(p => p.IsHot == true).ToList());
        }
    }
}
