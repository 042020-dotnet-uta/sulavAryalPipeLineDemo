using ConsoleShopper.Domain;
using ConsoleShopper.Repository.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleShopper.Repository
{
    public class OrderService : IOrderService
    {
        private readonly ConsoleShopperDbContext _dbContext;
        /// <summary>
        /// Constructor for OrderService injects dbContxt from DiContainer and initializes it. 
        /// </summary>
        /// <param name="dbContext"></param>
        public OrderService(ConsoleShopperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates order from customer
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task CreateOrder(Order order)
        {
            try
            {


                // adds the order including all the orderlineitems 
                _dbContext.Add(order);
                // gets order
                var storeId = order.StoreId;
                var userId = order.CustomerId;
                foreach (var o in order.OrderLineItems)
                {
                    var id = storeId;
                    var pid = o.InventoryItemId;
                    var inventoryItems = _dbContext.Inventory
                   .Include(i => i.Product)
                   .Include(i => i.Store)
                   .AsNoTracking()
                   .Where(i => i.StoreId == storeId && i.ProductId == o.InventoryItemId).FirstOrDefault();
                    var newQuantity = inventoryItems.Quantity - o.Quantity;
                    var inventoryToUpdate = new InventoryItem
                    {
                        Id = inventoryItems.Id,
                        ProductId = pid,
                        StoreId = storeId,
                        Quantity = newQuantity,
                        ChangedDate = DateTimeOffset.Now.LocalDateTime,
                        LoggedUserId = userId
                    };
                    _dbContext.Update(inventoryToUpdate);

                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }


        /// <summary>
        /// Retreives all the orders. 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {

            var result = await _dbContext.Orders
                .Include(o => o.OrderLineItems)
                .Include(o=>o.Store)
                .Include(o=>o.Customer)
                .AsNoTracking()
                .ToListAsync();
            return result;


        }

        /// <summary>
        /// Retreives Order by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Order> GetOrderById(int id)
        {
            try
            {
                return await _dbContext.Orders
              .Include(o => o.OrderLineItems)
              .Include(o => o.Store)
              .AsNoTracking()
              .Where(o => o.Id == id)
              .FirstOrDefaultAsync();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return null;
            }
         
        }

        /// <summary>
        /// Retrives Order by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Order>> GetOrderByStoreId(int id)
        {
            try
            {
                return await _dbContext.Orders
               .Include(o => o.OrderLineItems)
               .Include(o=>o.Customer)
               .Include(o => o.Store)
               .AsNoTracking()
               .Where(o => o.StoreId == id)
               .ToListAsync();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return null;
            }
           
        }

        /// <summary>
        /// Reteives orders by store names
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Order>> GetOrderByStoreName(string name)
        {
            try
            {
                return await _dbContext.Orders
               .Include(o => o.OrderLineItems)
               .Include(o => o.Store)
               .AsNoTracking()
               .Where(o => o.Store.Name.ToLower() == name.ToLower().Trim())
               .ToListAsync();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return null;
            }

        }

        /// <summary>
        /// Retreives order by Customer Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Order>> GetOrdersByCustomerId(int id)
        {
            try
            {
                return await _dbContext.Orders
               .Include(o => o.OrderLineItems)
               .Include(o=>o.Customer)
               .Include(o => o.Store)
               .AsNoTracking()
               .Where(o => o.CustomerId == id)
               .ToListAsync();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrives Orders by Customer Name
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Order>> GetOrdersByCustomerName(string searchString)
        {
            try
            {
                return await _dbContext.Orders
               .Include(o => o.OrderLineItems)
               .Include(o => o.Store)
               .AsNoTracking()
               .Where(o => o.Customer.FirstName.ToLower().Contains(searchString.ToLower()))
               .ToListAsync();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
