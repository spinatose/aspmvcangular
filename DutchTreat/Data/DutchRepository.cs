using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext context;
        private readonly ILogger<DutchRepository> logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void AddEntity(object model)
        {
            this.context.Add(model);
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems = true)
        {
            try
            {
                this.logger.LogInformation("GetAllOrders was called!");

                if (includeItems)
                    return this.context.Orders
                        .Include(o => o.Items).ThenInclude(i => i.Product) // the ThenInclude is on the indiv "Item"
                        .OrderBy(o => o.OrderDate).ToList();
                else 
                    return this.context.Orders
                        .OrderBy(o => o.OrderDate).ToList();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get all orders: {ex}");
            }

            return Enumerable.Empty<Order>();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                this.logger.LogInformation("GetAllProducts was called!");
                return this.context.Products.OrderBy(p => p.Title).ToList();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get all products: {ex}");
            }

            return Enumerable.Empty<Product>();
        }

        public Order? GetOrderById(int id)
        {
            try
            {
                return this.context.Orders                   
                    .Include(o => o.Items).ThenInclude(i => i.Product) // the ThenInclude is on the indiv "Item"
                    .Where(o => o.Id == id)
                    .FirstOrDefault();

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get order by id [{id}] ex: {ex}");
            }

            return null;
        }

        public IEnumerable<Product> GetProductsByCategory(string category) => this.context.Products.Where(p => p.Category == category).ToList();

        public bool SaveAll() => this.context.SaveChanges() > 0;
    }
}
