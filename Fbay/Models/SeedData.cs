using Fbay.Data;
using Fbay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fbay.Migrations
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FbayContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<FbayContext>>()))
            {

                if (context.Listings.Any())
                {
                    return;
                }


                //Example listings
                context.Listings.AddRange(

                    new Listings
                    {
                        Item = "Keyboard",
                        User_id = 1,
                        Amount = 12,
                        Image = "https://www.logitechg.com/content/dam/gaming/en/products/pro-keyboard/pro-clicky-hero.png"
                    },

                     new Listings
                     {
                         Item = "Monitor",
                         User_id = 2,
                         Amount = 120,
                         Image = "https://images.samsung.com/is/image/samsung/p6pim/fi/lu28r550uqrxen/gallery/fi-ur55-312872-lu28r550uqrxen-450344623?$720_576_PNG$"
                     },

                     new Listings
                     {
                         Item = "Desk",
                         User_id = 3,
                         Amount = 3,
                         Image = "https://cdn.moooi.com/tmp/image-thumbnails/Collection/Paper/Paper%20Desk/Paper%20Desk%20180/image-thumb__1846__height_500/Paper-Desk-180.png"
                     },

                     new Listings
                     {
                         Item = "PC",
                         User_id = 1,
                         Amount = 12,
                         Image = "https://www.gigantti.fi/image/dv_web_D180001002478661/193183/pcspecialist-tornado-r9-gaming-pc.jpg?$prod_all4one$"
                     }
                     );
                context.SaveChanges();

            }
        }
    }
}
    

