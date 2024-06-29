using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Xml.Linq;
using WebBanGiay.Data;
using WebBanGiay.Helpers;
using WebBanGiay.Infastructure;
using WebBanGiay.Models;
using WebBanGiay.Services;

namespace WebBanGiay.Controllers
{
    public class CartController : Controller
    {
        public Cart? Cart { get; set; }

        private readonly PaypalClient _paypalClient;
        private readonly ApplicationDbContext _context;
        private readonly IVnPayService _vnPayService;

        public CartController(ApplicationDbContext context, PaypalClient paypalClient, IVnPayService vnPayService)
        {
            _paypalClient = paypalClient;
            _context = context;
            _vnPayService = vnPayService;
        }

        public IActionResult Index()
        {
            return View("Cart", HttpContext.Session.GetJson<Cart>("cart"));
        }

        public IActionResult AddToCart(int productId)
        {
            Product? product = _context.Products.FirstOrDefault(x => x.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, 1);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);
        }

        public IActionResult UpdateCart(int productId)
        {
            Product? product = _context.Products.FirstOrDefault(x => x.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, -1);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);
        }

        public IActionResult RemoveFromCart(int productId)
        {
            Product? product = _context.Products.FirstOrDefault(x => x.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart");
                Cart.RemoveLine(product);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            if (Cart.Lines.Count == 0)
            {
                return Redirect("/");
            }
            ViewBag.PaypalClientId = _paypalClient.ClientId;
            return View(Cart);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckoutVM model,string paymentType = "COD")
        {
            HttpContext.Session.SetJson("checkoutModel", model);
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            if (ModelState.IsValid)
            {
                var user = new User();
                var userName = HttpContext.User.Claims.SingleOrDefault(p => p.Type == "Name").Value;


                var userFullName = HttpContext.User.Claims.SingleOrDefault(p => p.Type == ClaimTypes.Name).Value;


                if (model.GiongKhachHang)
                {
                    user = _context.User.SingleOrDefault(kh => kh.UserName == userName);
                }
                if (paymentType == "Thanh toán VNPay")
                {
                    var vnPayModel = new VnPaymentRequestModel
                    {
                        Amount = (double)model.TongTien,
                        CreatedDate = DateTime.Now,
                        Description = $"{model.NguoiNhanHang ?? user.UserFullName} {model.SoDienThoai}",
                        FullName = model.NguoiNhanHang ?? user.UserFullName,
                        OrderId = new Random().Next(1000, 100000)
                    };
                    return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
                }
                
                var hoadon = new Invoke
                {
                    Username = userName,
                    CustomerName = model.NguoiNhanHang ?? user.UserFullName,
                    PhoneNumber = model.SoDienThoai ?? user.UserPhoneNumber,
                    Address = model.DiaChiGiaoHang ?? user.UserAddress,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(3),
                    DaysToDeliver = 3,
                    InvokeTotalAmount = model.TongTien,
                    PaymentMethod = model.payment,
                    ShippingMethod = "Siêu tốc",
                    ShippingFee = 10000,
                    StatusId = 1,
                };
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Thêm hóa đơn vào cơ sở dữ liệu
                        _context.Add(hoadon);
                        _context.SaveChanges();
                        var invokeDetails = new List<InvokeDetail>();
                        // Thêm chi tiết hóa đơn
                        foreach (var item in Cart.Lines)
                        {
                            invokeDetails.Add(new InvokeDetail
                            {
                                InvokeId = hoadon.InvokeId,
                                ProductId = item.Product.ProductId,
                                Quantity = item.Quantity,
                                Price = item.Product.ProductPrice,
                            });
                        }
                        _context.AddRange(invokeDetails);
                        _context.SaveChanges();

                        // Commit giao dịch
                        transaction.Commit();

                        // Xóa giỏ hàng trong session
                        HttpContext.Session.SetJson("cart", new Cart());

                        return View("Success");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Debug.WriteLine(ex.Message);
                        
                    }
                }

            }
            return View("Checkout", Cart);
        }
        [Authorize]
        public IActionResult PaymentSuccess()
        {
            return View("Success");
        }
        //Thanh toán PAYPAL
        #region
        Paypal payment;
        [Authorize]
        [HttpPost("/Cart/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
        {
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            var tongTien = Cart.ComputeTotalValue().ToString();
            var donViTienTe = "USD";
            var maDonHangThamChieu = "DH" + DateTime.Now.Ticks.ToString();

            try
            {
                var response = await _paypalClient.CreateOrder(tongTien, donViTienTe, maDonHangThamChieu);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }
        [Authorize]
        [HttpPost("/Cart/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken,CheckoutVM model)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);

                // Lưu database đơn hàng của mình
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                if (ModelState.IsValid)
                {
                    var userName = HttpContext.User.Claims.SingleOrDefault(p => p.Type == "Name").Value;

                    var user = new User();
                    var userFullName = HttpContext.User.Claims.SingleOrDefault(p => p.Type == ClaimTypes.Name).Value;

                    if (model.GiongKhachHang)
                    {
                        user = _context.User.SingleOrDefault(kh => kh.UserName == userName);
                    }
                    var hoadon = new Invoke
                    {
                        Username = userName,
                        CustomerName = model.NguoiNhanHang ?? user.UserFullName,
                        PhoneNumber = model.SoDienThoai ?? user.UserPhoneNumber,
                        Address = model.DiaChiGiaoHang ?? user.UserAddress,
                        OrderDate = DateTime.Now,
                        PaymentMethod = model.payment,
                        ShippingMethod = "Siêu tốc",
                        StatusId = 1,
                    };
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            // Thêm hóa đơn vào cơ sở dữ liệu
                            _context.Add(hoadon);
                            _context.SaveChanges();
                            var invokeDetails = new List<InvokeDetail>();
                            // Thêm chi tiết hóa đơn
                            foreach (var item in Cart.Lines)
                            {
                                invokeDetails.Add(new InvokeDetail
                                {
                                    InvokeId = hoadon.InvokeId,
                                    ProductId = item.Product.ProductId,
                                    Quantity = item.Quantity,
                                    Price = item.Product.ProductPrice,
                                });
                            }
                            _context.AddRange(invokeDetails);
                            _context.SaveChanges();

                            // Commit giao dịch
                            transaction.Commit();

                            // Xóa giỏ hàng trong session
                            HttpContext.Session.SetJson("cart", new Cart());

                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Debug.WriteLine(ex.Message);

                        }
                    }

                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        #endregion
        //Thanh toán VNPAY
        #region
        
        [Authorize]
        public IActionResult PaymentFail()
        {
            return View();
        }
        [Authorize]
        public IActionResult PaymentCallBack()
        {
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            var model = HttpContext.Session.GetJson<CheckoutVM>("checkoutModel");
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }
            var user = new User();
            var userName = HttpContext.User.Claims.SingleOrDefault(p => p.Type == "Name").Value;


            var userFullName = HttpContext.User.Claims.SingleOrDefault(p => p.Type == ClaimTypes.Name).Value;


            if (model.GiongKhachHang)
            {
                user = _context.User.SingleOrDefault(kh => kh.UserName == userName);
            }
            var hoadon = new Invoke
            {
                Username = userName,
                CustomerName = model.NguoiNhanHang ?? user.UserFullName,
                PhoneNumber = model.SoDienThoai ?? user.UserPhoneNumber,
                Address = model.DiaChiGiaoHang ?? user.UserAddress,
                OrderDate = DateTime.Now,
                DeliveryDate = DateTime.Now.AddDays(3),
                DaysToDeliver = 3,
                InvokeTotalAmount = model.TongTien,
                PaymentMethod = model.payment,
                ShippingMethod = "Siêu tốc",
                ShippingFee = 10,
                StatusId = 1,
            };
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Thêm hóa đơn vào cơ sở dữ liệu
                    _context.Add(hoadon);
                    _context.SaveChanges();
                    var invokeDetails = new List<InvokeDetail>();
                    // Thêm chi tiết hóa đơn
                    foreach (var item in Cart.Lines)
                    {
                        invokeDetails.Add(new InvokeDetail
                        {
                            InvokeId = hoadon.InvokeId,
                            ProductId = item.Product.ProductId,
                            Quantity = item.Quantity,
                            Price = item.Product.ProductPrice,
                        });
                    }
                    _context.AddRange(invokeDetails);
                    _context.SaveChanges();

                    // Commit giao dịch
                    transaction.Commit();

                    // Xóa giỏ hàng trong session
                    HttpContext.Session.SetJson("cart", new Cart());
                    TempData["Message"] = $"Thanh toán VNPay thành công";
                    return RedirectToAction("PaymentSuccess");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Debug.WriteLine(ex.Message);
                    return RedirectToAction("PaymentFail");
                }

            }
        }
        #endregion
    }
}
