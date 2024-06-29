using Microsoft.AspNetCore.Mvc;

namespace WebBanGiay.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
