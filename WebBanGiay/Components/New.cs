using Microsoft.AspNetCore.Mvc;
using WebBanGiay.Data;

namespace WebBanGiay.Components
{
    public class New : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public New(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(_context.Products.Where(p => p.IsNew == true).ToList());
        }
    }
}
