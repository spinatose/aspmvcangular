using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext context;
        private readonly IWebHostEnvironment env;
        private readonly UserManager<StoreUser> userManager;

        public DutchSeeder(DutchContext context, IWebHostEnvironment env, UserManager<StoreUser> userManager)
        {
            this.context = context;
            this.env = env;
            this.userManager = userManager;
        }
        
        public async Task SeedAsync()
        {
            this.context.Database.EnsureCreated();

            StoreUser user = await this.userManager.FindByEmailAsync("spinatose@gmail.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "scot"
                    ,
                    LastName = "pfuntner"
                    ,
                    Email = "spinatose@gmail.com"
                    ,
                    UserName = "spinatose@gmail.com"
                };

                var result = await this.userManager.CreateAsync(user, "P@ssword1!");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder....");
                }
            }

            if (!this.context.Products.Any())
            {
                // create sample data
                var filefPath = Path.Combine(this.env.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filefPath);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

                this.context.Products.AddRange(products);

                var order = new Order()
                {
                    OrderDate = DateTime.Today,
                    OrderNumber = "10000",
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    }
                    , User = user
                };

                this.context.Orders.Add(order);

                this.context.SaveChanges();
            }
        }
    }
}
