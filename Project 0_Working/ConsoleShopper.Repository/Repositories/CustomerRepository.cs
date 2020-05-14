using ConsoleShopper.Domain;
using ConsoleShopper.Repository.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleShopper.Repository
{
    public class CustomerRepository : ICustomerRepository
    {


        private readonly ConsoleShopperDbContext _dbContext;


        public CustomerRepository(ConsoleShopperDbContext dbContext)
        {

            _dbContext = dbContext;

        }


        /// <summary>
        /// Gets All Customers form the data base and lists them.
        /// </summary>
        /// <returns></returns>
        #region Get Customer Data (DQL)
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _dbContext.Customers.Include(x => x.CustomerAddress).Select(x => x).AsNoTracking().ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Customers.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return null;
            }

        }

        /// <summary>
        /// Get Customer By Search String 
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Customer>> GetCustomerBySearchStringAsync(string searchString)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    var result = await _dbContext.Customers.Where(c => c.LastName.ToLower().Contains(searchString.ToLower()) ||
                       c.FirstName.ToUpper().Contains(searchString.ToLower()))
                    .Select(x => x).AsNoTracking().ToListAsync();
                    return result;

                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }
        #endregion

        #region Insert, Update, Delete Data (DML)

        public async Task InsertCustomerAsync(Customer customerToInsert)
        {
            try
            {
                await _dbContext.Customers.AddAsync(customerToInsert);
                await _dbContext.SaveChangesAsync();
                //await _dbContext.DisposeAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public async Task UpdateCustomerAsync(Customer customerToUpdate)
        {
            try
            {
                _dbContext.Customers.Update(customerToUpdate);
                var result = await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customerToDelete = await _dbContext.Customers.
                Include(c => c.CustomerAddress)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            try
            {
                _dbContext.Customers.Remove(customerToDelete);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        #endregion

        /// <summary>
        /// Checks if the user is Admin.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<(bool, int)> IsAdmin(string email, string password)
        {
            var customer = await _dbContext.Customers.Where(x => x.Email == email &&
            x.Password == password && x.UserTypeId == 1).AsNoTracking().FirstOrDefaultAsync();

            var isAdmin = false;
            var validatedCustomerId = 0;
            if (customer != null)
            {
                isAdmin = true;
                // Acts as a session holder. 
                validatedCustomerId = customer.Id;
                return (isAdmin, validatedCustomerId);
            }
            return (isAdmin, validatedCustomerId);
        }

        /// <summary>
        /// Checks if user is Customer type. 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<(bool, int)> IsCustomer(string email, string password)
        {
            var customer = await _dbContext.Customers.Where(x => x.Email == email &&
            x.Password == password && x.UserTypeId == 2).AsNoTracking().FirstOrDefaultAsync();

            var isCustomer = false;
            var validatedCustomerId = 0;
            if (customer != null)
            {
              
                isCustomer = true;
                // Acts as a session holder. 
                validatedCustomerId = customer.Id;
                return (isCustomer, validatedCustomerId);
            }
            return (isCustomer, validatedCustomerId);
        }
    }
}
