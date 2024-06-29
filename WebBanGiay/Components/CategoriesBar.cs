using Microsoft.AspNetCore.Mvc;
using WebBanGiay.Data;

namespace WebBanGiay.Components
{
    public class CategoriesBar:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CategoriesBar(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(_context.Categories.ToList());
        }
    }
}
