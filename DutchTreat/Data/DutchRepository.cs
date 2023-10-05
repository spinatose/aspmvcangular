using DutchTreat.Data.Entities;

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

        public IEnumerable<Product> GetProductsByCategory(string category) => this.context.Products.Where(p => p.Category == category).ToList();

        public bool SaveAll() => this.context.SaveChanges() > 0;
    }
}
