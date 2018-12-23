using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoffeeShopProject.Controllers
{
    public class TestController : Controller
    {
        private readonly CoffeeShopContext db;
        public TestController(CoffeeShopContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            var cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "cart");
            ViewBag.Cart = cart;
            return View();
        }
        public List<CartItem> GetGioHang
        {
            get
            {
                List<CartItem> myCart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "cart");
                if (myCart == default(List<CartItem>))
                {
                    myCart = new List<CartItem>();
                }
                return myCart;
            }
        }
        public IActionResult Buy(int product_id)
        {
            //lấy giỏ hàng đang có
            var gioHang = GetGioHang;
            //kiểm tra xem hàng đã có trong giỏ chưa
            CartItem item = gioHang.SingleOrDefault(p => p.MaThucDon == product_id);
            //nếu có
            if (item != null)
            {
                item.SoLuong++;//tăng số lượng
            }
            else
            {
                ThucDon hh = db.ThucDon.SingleOrDefault(p => p.MaThucDon == product_id);
                item = new CartItem
                {
                    MaThucDon = product_id,
                    SoLuong = 1,
                    TenThucDon = hh.TenThucDon,
                    HinhAnh = hh.HinhAnh,
                    GiaBan = hh.GiaKhuyenMai
                };
                //gioHang.Add(item);
            }
            //lưu session
            SessionHelper.Set(HttpContext.Session, "cart", gioHang);
            //chuyển tới trang giỏ hàng để xem
            return RedirectToAction("Index");
        }

        public IActionResult NextPage()
        {
            return View();
        }
    }
}