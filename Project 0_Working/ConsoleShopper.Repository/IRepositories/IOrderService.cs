using ConsoleShopper.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShopper.Repository
{
    /// <summary>
    /// Interface for OrderService
    /// </summary>
    public interface IOrderService
    {
        Task CreateOrder(Order order);
        Task <IEnumerable<Order>> GetOrdersByCustomerName(string searchString);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task <IEnumerable<Order>> GetOrdersByCustomerId(int id);
        Task <IEnumerable<Order>> GetOrderByStoreId(int id);
        Task<Order> GetOrderById(int id);

    }
}
