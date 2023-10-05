using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Product> GetAllProducts();
        Order? GetOrderById(int id);
        IEnumerable<Product> GetProductsByCategory(string category);
        bool SaveAll();
    }
}