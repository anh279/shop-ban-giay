using Microsoft.AspNetCore.Mvc;
using WebBanGiay.Data;

namespace WebBanGiay.Components
{
    public class TestNav:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public TestNav(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(_context.Categories.ToList());
        }
    }
}
