using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {
        void AddEntity(object model);
        IEnumerable<Order> GetAllOrders(bool includeItems);
        object GetAllOrdersByUser(string username, bool includeItems);
        IEnumerable<Product> GetAllProducts();
        Order? GetOrderById(string username, int id);
        IEnumerable<Product> GetProductsByCategory(string category);
        bool SaveAll();
    }
}