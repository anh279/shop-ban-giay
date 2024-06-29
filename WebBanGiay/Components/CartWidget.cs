using Microsoft.AspNetCore.Mvc;
using WebBanGiay.Data;
using WebBanGiay.Infastructure;
using WebBanGiay.Models;

namespace WebBanGiay.Components
{
    public class CartWidget:ViewComponent
    {
        

        public IViewComponentResult Invoke()
        {
            return View(HttpContext.Session.GetJson<Cart>("cart"));
        }
    }
}
