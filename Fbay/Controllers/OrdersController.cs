using Fbay.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Fbay.Controllers
{
    public class OrdersController : Controller
    {
        private readonly FbayContext _context;
        public OrdersController(FbayContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.ToListAsync());
        }
        public async Task<IActionResult> OrderDetailed(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            string[] items = order.ItemNames.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            string[] itemAmounts = order.ItemAmounts.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            string[] itemImages = order.ItemImages.Split(", ", StringSplitOptions.RemoveEmptyEntries);

            dynamic dynamicmodel = new ExpandoObject();
            dynamicmodel.Order = order;
            dynamicmodel.Items = items;
            dynamicmodel.ItemAmounts = itemAmounts;
            dynamicmodel.ItemImages = itemImages;

            return View(dynamicmodel);
        }
    }
}
