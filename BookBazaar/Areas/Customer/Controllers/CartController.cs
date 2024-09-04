using BookBazaar.Models;
using BookBazaar.Models.ViewModels;
using BookBazaar.Repository.IRepository;
using BookBazaar.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace BookBazaar.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM shoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCartVM = new()
            {
                CartList = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == userId, includeProperties: "book"),
                orderHeader = new()
            };

            foreach (var cart in shoppingCartVM.CartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                shoppingCartVM.orderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(shoppingCartVM);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCartVM = new()
            {
                CartList = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == userId, includeProperties: "book"),
                orderHeader = new()
            };

            shoppingCartVM.orderHeader.user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userId);
            shoppingCartVM.orderHeader.Name = shoppingCartVM.orderHeader.user.Name;
            shoppingCartVM.orderHeader.PhoneNumber = shoppingCartVM.orderHeader.user.PhoneNumber;
            shoppingCartVM.orderHeader.StreetAddress = shoppingCartVM.orderHeader.user.StreetAddress;
            shoppingCartVM.orderHeader.City = shoppingCartVM.orderHeader.user.City;
            shoppingCartVM.orderHeader.State = shoppingCartVM.orderHeader.user.State;
            shoppingCartVM.orderHeader.PostalCode = shoppingCartVM.orderHeader.user.PostalCode;

            foreach (var cart in shoppingCartVM.CartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                shoppingCartVM.orderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(shoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCartVM.CartList = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == userId, includeProperties: "book");

            shoppingCartVM.orderHeader.OrderDate = System.DateTime.Now;
            shoppingCartVM.orderHeader.UserId = userId;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userId);

            foreach (var cart in shoppingCartVM.CartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                shoppingCartVM.orderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            if (applicationUser.companyId.GetValueOrDefault() == 0)
            {
                //it is a customer account
                shoppingCartVM.orderHeader.PaymentStatus = SD.PaymentStatusPending;
                shoppingCartVM.orderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                //it is a company account
                shoppingCartVM.orderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                shoppingCartVM.orderHeader.OrderStatus = SD.StatusApproved;
            }

            _unitOfWork.OrderHeader.Add(shoppingCartVM.orderHeader);
            _unitOfWork.Save();
            foreach (var cart in shoppingCartVM.CartList)
            {
                OrderDetail orderDetail = new()
                {
                    bookId = cart.bookId,
                    OrderId = shoppingCartVM.orderHeader.Id,
                    count = cart.Count,
                    totalAmount = cart.Price
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            if (applicationUser.companyId.GetValueOrDefault() == 0)
            {
                //it is a customer account we need to capture payment
                var domain = "http://localhost:5025/";
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={shoppingCartVM.orderHeader.Id}",
                    CancelUrl = domain + "Customer/Cart/Index",
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                    Mode = "payment",
                };

                foreach (var item in shoppingCartVM.CartList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount =(long)(item.Price*100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.book.title,
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new Stripe.Checkout.SessionService();
                Session session = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStriprPaymentId(shoppingCartVM.orderHeader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }

            return RedirectToAction(nameof(OrderConfirmation), new { id = shoppingCartVM.orderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == id, includeProperties: "user");
            if(orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                //this is an order by customer
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if(session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStriprPaymentId(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
                HttpContext.Session.Clear();
            }

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == orderHeader.UserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(id);
        }

        public IActionResult Plus(int? cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.shoppingCartId == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Minus(int? cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.shoppingCartId == cartId, tracked: true);
            if (cartFromDb.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.UserId == cartFromDb.UserId).Count() - 1);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int? cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.shoppingCartId == cartId, tracked: true);
            _unitOfWork.ShoppingCart.Remove(cartFromDb);

            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.UserId == cartFromDb.UserId).Count() - 1);

            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.book.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.book.Price50;
                }
                else
                {
                    return shoppingCart.book.Price100;
                }
            }
        }
    }
}
