using ConsoleShopper.Domain;
using ConsoleShopper.Repository.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleShopper.Tests
{
    public class CustomerCRUDTests
    {
        
        [Fact]
        public async Task AddsCustomerToDb()
        {
            
            //Arrange - create an object to configure your inmemory DB.
            var options = new DbContextOptionsBuilder<ConsoleShopperDbContext>()
                .UseInMemoryDatabase(databaseName: "AddsCustomerToDb")
                .Options;


            //Act - send in the configure object to the DbContext constructor to be used in configuring the DbContext
            using (var db = new ConsoleShopperDbContext(options))
            {
                CustomerAddress customerAddress = new CustomerAddress { Id = 1, Street = "8286 Clay Ave.", City = "Spokane", State = "WA", Zip ="11111" };
                Customer customer = new Customer { Id = 6, FirstName = "Maribeth", LastName = "Fontenot",Email = "test@test.com", PhoneNo = "1234112233", Password = "password", UserTypeId = 2 ,CustomerAddress = customerAddress };
                db.Add(customer);
                db.SaveChanges();
            }

            //Assert
            using (var context = new ConsoleShopperDbContext(options)) 
            {
                Assert.Equal(1, context.Customers.Count());
                var customer1 = await context.Customers.Where(x => x.FirstName == "Maribeth")
                    .AsNoTracking().FirstOrDefaultAsync();
                var customer1Address = await context.Customers.
                    Include(c => c.CustomerAddress).AsNoTracking().FirstOrDefaultAsync();
                Assert.Equal("Maribeth", customer1.FirstName);
                Assert.Equal("11111", customer1Address.CustomerAddress.Zip);
            }
        }
    }
}
