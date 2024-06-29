using Microsoft.AspNetCore.Mvc;
using WebBanGiay.Data;

namespace WebBanGiay.Components
{
    public class UserInfo : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public UserInfo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(_context.User.ToList());
        }
    }
}
