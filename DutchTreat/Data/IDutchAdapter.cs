using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public interface IDutchAdapter
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
        IEnumerable<Order> GetAllOrders(bool includeItems);
        bool SaveAll();
        Order GetOrderById(string username, int id);
        void AddEntity(object model);
        IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);
    }
}
