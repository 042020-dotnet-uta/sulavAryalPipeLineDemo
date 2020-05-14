using ConsoleShopper.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShopper.Repository
{
    /// <summary>
    /// Interface for Product Repository.
    /// </summary>
    public interface IProductRepository
    {
        Task<IEnumerable<InventoryItem>> GetAllInventoryAsync();
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<InventoryItem>> GetProductsBySearchStringAsync(string searchString);
        Task<IEnumerable<InventoryItem>> GetProductsByStoreAsync(int storeId);
        Task<InventoryItem> GetProductByIdAsync(int id);
        Task<InventoryItem> GetProductByCodeAsync(string code);
        Task CreateProductAsync(InventoryItem inventoryItemToCreate);
        Task UpdateProductAsync(InventoryItem inventoryItemToUpdate);
        Task DeleteProductAsync(int id);
    }
}