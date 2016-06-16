using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TeduShop.Common;
using TeduShop.Model.Models;
using TeduShop.Service;
using TeduShop.Web.App_Start;
using TeduShop.Web.Infrastructure.Extensions;

using TeduShop.Web.Infrastructure.NganLuongAPI;
using TeduShop.Web.Models;

namespace TeduShop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        IProductService _productService;
        IOrderService _orderService;
        private ApplicationUserManager _userManager;

        private string merchantId = ConfigHelper.GetByKey("MerchantId");
        private string merchantPassword = ConfigHelper.GetByKey("MerchantPassword");
        private string merchantEmail = ConfigHelper.GetByKey("MerchantEmail");

        public ShoppingCartController(IOrderService orderService, IProductService productService, ApplicationUserManager userManager)
        {
            this._productService = productService;
            this._userManager = userManager;
            this._orderService = orderService;
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return View();
        }

        public ActionResult CheckOut()
        {
            if (Session[CommonConstants.SessionCart] == null)
            {
                return Redirect("/gio-hang.html");
            }
            return View();
        }
        public JsonResult GetUser()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);
                return Json(new
                {
                    data = user,
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }
        public ActionResult CreateOrder(string orderViewModel)
        {
            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);

            var orderNew = new Order();

            orderNew.UpdateOrder(order);

            if (Request.IsAuthenticated)
            {
                orderNew.CustomerId = User.Identity.GetUserId();
                orderNew.CreatedBy = User.Identity.GetUserName();
            }

            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            bool isEnough = true;
            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.ProductId;
                detail.Quantity = item.Quantity;
                detail.Price = item.Product.Price;
                orderDetails.Add(detail);

                isEnough = _productService.SellProduct(item.ProductId, item.Quantity);
                break;
            }
            if (isEnough)
            {
                var orderReturn = _orderService.Create(ref orderNew, orderDetails);
                _productService.Save();

                if (order.PaymentMethod == "CASH")
                {
                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                  
                    var currentLink = ConfigHelper.GetByKey("CurrentLink");
                    RequestInfo info = new RequestInfo();
                    info.Merchant_id = merchantId;
                    info.Merchant_password = merchantPassword;
                    info.Receiver_email = merchantEmail;



                    info.cur_code = "vnd";
                    info.bank_code = order.BankCode;

                    info.Order_code = orderReturn.ID.ToString();
                    info.Total_amount = orderDetails.Sum(x => x.Quantity * x.Price).ToString();
                    info.fee_shipping = "0";
                    info.Discount_amount = "0";
                    info.order_description = "Thanh toán đơn hàng tại TeduShop";
                    info.return_url = currentLink + "xac-nhan-don-hang.html";
                    info.cancel_url = currentLink + "huy-don-hang.html";

                    info.Buyer_fullname = order.CustomerName;
                    info.Buyer_email = order.CustomerEmail;
                    info.Buyer_mobile = order.CustomerMobile;

                    APICheckoutV3 objNLChecout = new APICheckoutV3();
                    ResponseInfo result = objNLChecout.GetUrlCheckout(info, order.PaymentMethod);
                    if (result.Error_code == "00")
                    {
                        return Json(new
                        {
                            status = true,
                            urlCheckout = result.Checkout_url,
                            message = result.Description
                        });
                    }
                    else
                        return Json(new
                        {
                            status = false,
                            message = result.Description
                        });
                }

            }
            else
            {
                return Json(new
                {
                    status = false,
                    message = "Không đủ hàng."
                });
            }

        }
        public JsonResult GetAll()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            return Json(new
            {
                data = cart,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Add(int productId)
        {
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            var product = _productService.GetById(productId);
            if (cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }
            if (product.Quantity == 0)
            {
                return Json(new
                {
                    status = false,
                    message = "Sản phẩm này hiện đang hết hàng"
                });
            }
            if (cart.Any(x => x.ProductId == productId))
            {
                foreach (var item in cart)
                {
                    if (item.ProductId == productId)
                    {
                        item.Quantity += 1;
                    }
                }
            }
            else
            {
                ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                newItem.ProductId = productId;
                newItem.Product = Mapper.Map<Product, ProductViewModel>(product);
                newItem.Quantity = 1;
                cart.Add(newItem);
            }

            Session[CommonConstants.SessionCart] = cart;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(int productId)
        {
            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if (cartSession != null)
            {
                cartSession.RemoveAll(x => x.ProductId == productId);
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new
                {
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }

        [HttpPost]
        public JsonResult Update(string cartData)
        {
            var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);

            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            foreach (var item in cartSession)
            {
                foreach (var jitem in cartViewModel)
                {
                    if (item.ProductId == jitem.ProductId)
                    {
                        item.Quantity = jitem.Quantity;
                    }
                }
            }

            Session[CommonConstants.SessionCart] = cartSession;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return Json(new
            {
                status = true
            });
        }

        public ActionResult ConfirmOrder()
        {
            string token = Request["token"];
            RequestCheckOrder info = new RequestCheckOrder();
            info.Merchant_id = merchantId;
            info.Merchant_password = merchantPassword;
            info.Token = token;
            APICheckoutV3 objNLChecout = new APICheckoutV3();
            ResponseCheckOrder result = objNLChecout.GetTransactionDetail(info);
            if (result.errorCode == "00")
            {
                //update status order
                _orderService.UpdateStatus(int.Parse(result.order_code));
                _orderService.Save();
                ViewBag.IsSuccess = true;
                ViewBag.Result = "Thanh toán thành công. Chúng tôi sẽ liên hệ lại sớm nhất.";
            }
            else
            {
                ViewBag.IsSuccess = true;
                ViewBag.Result = "Có lỗi xảy ra. Vui lòng liên hệ admin.";
            }
            return View();
        }
        public ActionResult CancelOrder()
        {
            return View();
        }
    }
}