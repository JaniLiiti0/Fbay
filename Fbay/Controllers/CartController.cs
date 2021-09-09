using Fbay.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Fbay.Models;
using Fbay.Helpers;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fbay.Controllers
{
    public class CartController : Controller
    {
        private readonly FbayContext _context;

        public CartController(FbayContext context)
        {
            _context = context;
        }

        public bool ListingExists(int id)
        {
            return _context.Listings.Any(e => e.Listing_id == id);
        }

        public async Task<IActionResult> Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Product>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart = new List<Product>();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            ViewBag.cart = cart;

            var users = await _context.Users.ToListAsync();
            ViewBag.users = users;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AddToCart/{id}")]
        public async Task<IActionResult> AddToCart(int? id, int boughtAmount)
        {
            bool productAlreadyInCart = false;
            var listing = await _context.Listings.FindAsync(id);
            if (ModelState.IsValid)
            {

                try
                {
                    //Making sure an item can't be sold more than there is stock
                    //Replace with a system that verifies without leaving the page?
                    if ((listing.Amount - boughtAmount) >= 0)
                        listing.Amount -= boughtAmount;
                    else
                        return BadRequest(new { Message = "Not enough stock" });
                    _context.Update(listing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListingExists(listing.Listing_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                //If there is no cart yet, make one
                if (SessionHelper.GetObjectFromJson<List<Product>>(HttpContext.Session, "cart") == null)
                {
                    List<Product> cart = new List<Product>();
                    Product p = new Product();
                    p.Product_id = listing.Listing_id;
                    p.Amount = boughtAmount;
                    p.Price = listing.Price;
                    p.Product_name = listing.Item;
                    p.Image = listing.Image;
                    cart.Add(p);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                    listing.InNumberOfCarts += 1;
                }

                //Add new items to existing cart
                else
                {
                    List<Product> cart = SessionHelper.GetObjectFromJson<List<Product>>(HttpContext.Session, "cart");
                    for (int i = 0; i < cart.Count; i++)
                    {
                        //If the item is already in cart, just raise the amount
                        if (listing.Listing_id == cart[i].Product_id)
                        {
                            cart[i].Amount += boughtAmount;
                            productAlreadyInCart = true;
                            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                        }
                    }
                    //Adding item to cart
                    if (!productAlreadyInCart)
                    {
                        Product p = new Product();
                        p.Product_id = listing.Listing_id;
                        p.Amount = boughtAmount;
                        p.Price = listing.Price;
                        p.Product_name = listing.Item;
                        p.Image = listing.Image;
                        cart.Add(p);
                        SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                        listing.InNumberOfCarts += 1;
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(listing); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            //Remove item from cart
            List<Product> cart = SessionHelper.GetObjectFromJson<List<Product>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product_id == id)
                {
                    var listing = await _context.Listings.FindAsync(id);
                    listing.InNumberOfCarts -= 1;
                    listing.Amount += cart[i].Amount;
                    await _context.SaveChangesAsync();

                    cart.RemoveAt(i);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            List<Product> cart = SessionHelper.GetObjectFromJson<List<Product>>(HttpContext.Session, "cart");

            //Calculate the total price of all items in the cart
            int itemsInCart = cart.Count;
            var cartTotal = 0m; ;
            for (int i = 0; i < itemsInCart; i++)
            {
                cartTotal += cart[i].Price * cart[i].Amount;
            }

            //Check whether the user has enough money
            if (user.Money >= cartTotal)
            {
                Orders order = new Orders();
                order.BuyerName = user.Fname + " " + user.Lname;
                for (int i = 0; i < itemsInCart; i++)
                {
                    //Make strings from items, amounts, images
                    order.ItemNames += cart[i].Product_name + ", ";
                    order.ItemAmounts += cart[i].Amount + ", ";
                    order.ItemImages += cart[i].Image + ", ";
                    order.TotalPrice = cartTotal;
                    var listing = await _context.Listings.FindAsync(cart[i].Product_id);
                    listing.InNumberOfCarts -= 1;

                    //After transaction, if there is no stock left, delete listing
                    //Possibly have an "old listings" page for a user's own posts where you can add stock
                    if (listing.Amount == 0)
                    {
                        //_context.Listings.Remove(listing);
                    }
                }
                user.Money -= cartTotal;
                _context.Add(order);
                await _context.SaveChangesAsync();
                cart.Clear();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                return Content("Not enough money!");
            }
            return RedirectToAction("Index");
        }
    }
}
