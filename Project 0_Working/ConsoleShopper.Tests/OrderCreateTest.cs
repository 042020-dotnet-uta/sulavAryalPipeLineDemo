using ConsoleShopper.Domain;
using ConsoleShopper.Repository.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleShopper.Tests
{
    public class OrderCreateTest
    {
        [Fact]
        public async Task AddsOrderIntoDatabase()
        {

            //Arrange - create an object to configure your inmemory DB.
            var options = new DbContextOptionsBuilder<ConsoleShopperDbContext>()
                .UseInMemoryDatabase(databaseName: "AddsCustomerToDb")
                .Options;


            //Act - send in the configure object to the DbContext constructor to be used in configuring the DbContext
            using (var db = new ConsoleShopperDbContext(options))
            {

                /*
                    * Create order and orderline objects. 
                    *  1. Customer :- should be (logged in) -> from UI. 
                    *      => take customer id from here and put it in order object.
                    *  2. Ask Customer to choose the Store location.
                    *      => take Store location from here and put it in order object. 
                    *  3. Ask Customer about which product/products and what quantites they want to order. 
                    *      => Show the list of products, aviliable quantites and their respective prices to the user. 
                    *  4. Allow customers to choose multiple products with multiple quantities. 
                    *      => hold the product code + quantity in a dictionary. 
                    *  5. Ask user to confirm the purchase.
                    *  6. Insert into order the 
                    *      => the userId
                    *      => the storeLocation
                    *      => and time of purchase.
                    *  7. Take out productCode and quantity one by one
                    *      => Insert into the orderline object
                    *          => InventoryItemId(i.e: product Id)
                    *          => Quantity
                    *          => Price
                    *          => OrderId
                    *      => Decrease the Quantity in InventoryItem after purchase.
                    *       
                */

                // Make sure its in same store. 
                Customer cust = new Customer { Id = 1 };
                // Ask user list of products 
                InventoryItem i1 = new InventoryItem { StoreId = 1, ProductId = 1, Quantity = 20 };
                InventoryItem i2 = new InventoryItem { StoreId = 1, ProductId = 2, Quantity = 20 };

                // hold product chosen by user and quantity specified by user
                Dictionary<int,int> pairs = new Dictionary<int, int> 
                {
                    { i1.ProductId, 5},
                    { i2.ProductId, 4}
                };
                // Oh no ! user wants to add another  2 items !
                // have no fear !
                pairs[i1.ProductId] += 2;

                Order order = new Order { Id = 1, CustomerId = cust.Id, StoreId = i1.StoreId, OrderDate = DateTimeOffset.Now.LocalDateTime };
                
                OrderLineItem oLI1 = new OrderLineItem 
                { 
                    Id = 1,  
                    OrderId = 1, 
                    InventoryItemId = i1.ProductId,
                    Quantity =pairs[i1.ProductId]
                    
                };
                OrderLineItem oLI2 = new OrderLineItem
                {
                    Id = 2,
                    OrderId = 1,
                    InventoryItemId = i2.ProductId,
                    Quantity = pairs[i2.ProductId]
                };
                db.Add(order);
                db.Add(oLI1);
                db.Add(oLI2);
                db.SaveChanges();
            }

            //Assert
            using (var context = new ConsoleShopperDbContext(options))
            {

                //Assert.Equal("Guitar", inventory1.Product.Name);
                var test = await context.OrderLineItems.Include(x => x.Order).CountAsync();
                Assert.Equal(2, test);
                var order1 = await context.OrderLineItems
                    .Where(x=>x.InventoryItemId == 1 )
                    .FirstOrDefaultAsync(x=>x.Id == 1);
                Assert.Equal(1, order1.Id);

            }

        }
    }
}
