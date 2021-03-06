using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fbay.Data;
using Fbay.Models;
using System.Diagnostics;
using System.Dynamic;
using System.Net;
using Fbay.Helpers;

namespace Fbay.Controllers
{
    public class ListingsController : Controller
    {
        private readonly FbayContext _context;

        //Image used in a listing if no image or a non-valid image was inserted.
        String defaultImage = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/ac/No_image_available.svg/600px-No_image_available.svg.png";

        public ListingsController(FbayContext context)
        {
            _context = context;
        }

        // GET: Listings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Listings.ToListAsync());
        }

        //Can be used to add stock to out of stock listings
        public async Task<IActionResult> OldListings()
        {
            return View(await _context.Listings.ToListAsync());
        }

        // GET: Listings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listings
                .FirstOrDefaultAsync(m => m.Listing_id == id);
            if (listing == null)
            {
                return NotFound();
            }

            //Dynamic model so the view can use data from both user and listing tables
            dynamic dynamicmodel = new ExpandoObject();
            dynamicmodel.Listings = listing;
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == listing.User_id);
            String seller = user.Fname;
            dynamicmodel.Users = user;
            return View(dynamicmodel);
        }

        // GET: Listings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Listing_id,User_id,Item,Amount,Price,Image")] Listings listing)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == listing.User_id);
                if (user == null)
                {
                    return Content("User not found!");
                }
                else
                {
                    _context.Add(listing);

                    //Using images from the internet is obviously a bad way to implement them, should be replaced with storage
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(listing.Image);
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            //If this passes, the image is valid and will be used in the listing
                            bool exist = response.StatusCode == HttpStatusCode.OK;
                        }
                    }
                    catch
                    {
                        //Inserted image wasn't valid, or the field was left empty, default image will be used
                        listing.Image = defaultImage;
                    }
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
                
            }
            return View(listing);
        }

        // GET: Listings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listings.FindAsync(id);
            if (listing == null)
            {
                return NotFound();
            }
            return View(listing);
        }

        // POST: Listings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Listing_id,User_id,Item,Amount,Price,Image")] Listings listing)
        {
            if (id != listing.Listing_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(listing.Image);
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            //If this passes, the image is valid and will be used in the listing
                            bool exist = response.StatusCode == HttpStatusCode.OK;
                        }
                    }
                    catch
                    {
                        //Inserted image wasn't valid, default image will be used
                        listing.Image = defaultImage;

                    }
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
                return RedirectToAction(nameof(Index));
            }
            return View(listing);
        }

        public async Task<IActionResult> Buy(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listings.FindAsync(id);
            if (listing == null)
            {
                return NotFound();
            }
            //Dynamic model so the view can use data from both user and listing tables
            dynamic dynamicmodel = new ExpandoObject();
            dynamicmodel.Listings = listing;
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == listing.User_id);
            String seller = user.Fname;
            dynamicmodel.Users = user;
            return View(dynamicmodel);
        }

        // GET: Listings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listings
                .FirstOrDefaultAsync(m => m.Listing_id == id);
            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        // POST: Listings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listing = await _context.Listings.FindAsync(id);
            if (listing.InNumberOfCarts <= 0)
            {
                _context.Listings.Remove(listing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Content("Item is in a cart, can't be deleted.");
            }
        }

        public bool ListingExists(int id)
        {
            return _context.Listings.Any(e => e.Listing_id == id);
        }
    }
}
