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

        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Product>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart = new List<Product>();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            ViewBag.cart = cart;
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
                
                if (SessionHelper.GetObjectFromJson<List<Product>>(HttpContext.Session, "cart") == null)
                {
                    List<Product> cart = new List<Product>();
                    Product p = new Product();
                    p.Product_id = listing.Listing_id;
                    p.Amount = boughtAmount;
                    p.Product_name = listing.Item;
                    p.Image = listing.Image;
                    cart.Add(p);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                    listing.InNumberOfCarts += 1;
                }
                else
                {
                    List<Product> cart = SessionHelper.GetObjectFromJson<List<Product>>(HttpContext.Session, "cart");
                    for (int i = 0; i < cart.Count; i++)
                    {
                        if (listing.Listing_id == cart[i].Product_id)
                        {
                            cart[i].Amount += boughtAmount;
                            productAlreadyInCart = true;
                            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                        }
                    }
                    if (!productAlreadyInCart)
                    {
                        Product p = new Product();
                        p.Product_id = listing.Listing_id;
                        p.Amount = boughtAmount;
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
        public async Task<IActionResult> Checkout()
        {
            List<Product> cart = SessionHelper.GetObjectFromJson<List<Product>>(HttpContext.Session, "cart");
            int itemsInCart = cart.Count;
            for (int i = 0; i < itemsInCart; i++)
            {
                var listing = await _context.Listings.FindAsync(cart[i].Product_id);
                listing.InNumberOfCarts -= 1;
                await _context.SaveChangesAsync();

                //After transaction, if there is no stock left, delete listing
                //Possibly have an "old listings" page for a user's own posts where you can add stock
                if (listing.Amount == 0)
                {
                    _context.Listings.Remove(listing);
                    await _context.SaveChangesAsync();
                }
            }
            cart.Clear();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }
    }
}
