namespace WebBanGiay.Models
{
    public class CheckoutVM
    {
        public bool GiongKhachHang { get; set; }
        public string? NguoiNhanHang { get; set; }
        public string? SoDienThoai { get; set; }
        public string? DiaChiGiaoHang { get; set; }

        public decimal TongTien { get; set; }
        public string? payment { get; set; }
    }
}
