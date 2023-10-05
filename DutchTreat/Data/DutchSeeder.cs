using DutchTreat.Data.Entities;
using System.Text.Json;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext context;
        private readonly IWebHostEnvironment env;

        public DutchSeeder(DutchContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        
        public void Seed()
        {
            this.context.Database.EnsureCreated();

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
                };

                this.context.Orders.Add(order);

                this.context.SaveChanges();
            }
        }
    }
}
