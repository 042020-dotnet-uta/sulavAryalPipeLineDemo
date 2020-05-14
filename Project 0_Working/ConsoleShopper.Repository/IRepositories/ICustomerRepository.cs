using ConsoleShopper.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleShopper.Repository
{
    /// <summary>
    /// Interface for CustomerRepository
    /// </summary>
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<IEnumerable<Customer>> GetCustomerBySearchStringAsync(string searchString);
        Task<Customer> GetCustomerByIdAsync(int id);
        Task InsertCustomerAsync(Customer customerToInsert);
        Task UpdateCustomerAsync(Customer customerToUpdate);
        Task DeleteCustomerAsync(int id);
        Task<(bool,int)> IsAdmin(string username, string password);
        Task<(bool,int)>IsCustomer(string username, string password);
    }
}