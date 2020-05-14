using ConsoleShopper.Domain;
using ConsoleShopper.Repository;
using ConsoleShopper.Repository.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleShopper.Tests
{
    public class InventoryCRUDTests
    {
        [Fact]
        public async Task AddsProductInventoryIntoDatabase()
        {

            //Arrange - create an object to configure your inmemory DB.
            var options = new DbContextOptionsBuilder<ConsoleShopperDbContext>()
                .UseInMemoryDatabase(databaseName: "AddsCustomerToDb")
                .Options;


            //Act - send in the configure object to the DbContext constructor to be used in configuring the DbContext
            using (var db = new ConsoleShopperDbContext(options))
            {

                // Put Name in Product entity 

                Product product = new Product {Id = 1, Name = "Guitar", ProductCode ="P00007", Price = 155.50M  };
                Store store = new Store { Id = 1, Name = "Texas" };
                InventoryItem inventory = new InventoryItem { 
                    Id = 1, 
                    ProductId = 1,
                    Quantity = 1, 
                    Product = product, 
                    Store = store, 
                    LoggedUserId = 1,
                    ChangedDate = DateTime.Now.ToLocalTime()
                };
                db.Add(inventory);
                db.SaveChanges();
            }

            //Assert
            using (var context = new ConsoleShopperDbContext(options))
            {
                Assert.Equal(1, context.Products.Count());
                var inventory1 = await context.Inventory
                    .Include(i=>i.Product)
                    .Include(i=>i.Store)
                    .Where(i=>i.Product.Name == "Guitar")
                    .AsNoTracking().FirstOrDefaultAsync();


                Assert.Equal("Guitar", inventory1.Product.Name);
            }

        }
    }
}
