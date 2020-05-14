using ConsoleShopper.Domain;
using ConsoleShopper.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleShopper.UI
{
    
    public class ProductCRUD
    {
        // Bringing in DI container built from ContainerBuilder.cs. 
        static readonly IServiceProvider Container = ContainerBuilder.Build();

        // flag to allow or disallow GetProductByIdAsync to display message. 
        // disallow if GetProductByIdAsync is called from other methods in this class

        bool flag = true;

        #region Get Product By Id
        /// <summary>
        /// Gets Product by Id asynchronously 
        /// </summary>
        /// <param name="productIdStringParm">Optional</param>
        /// <returns>Product or null if product not found</returns>
        public async Task<InventoryItem> GetProductByIdAsync(string productIdStringParm = "")
        {
            // to set productId after conversion down at line no 40. 
            int productId = 0;
            // checks if have anything coming from parameter
            if (!string.IsNullOrEmpty(productIdStringParm))
            {

                // static helper function ParseString.ToInt
                // returns 0 if it fails to Parse String to Int .
                int productIdInt = ParseString.ToInt(productIdStringParm);

                // check for that 0
                if (productId != 0)
                {
                    productId = productIdInt;
                }

            }

            // if not asks for user's input
            else
            {
                Console.Write("\nEnter the Id of product: ");
                var productIdString = Console.ReadLine();

                // static helper function Converts.ToInt
                // returns 0 if it fails to Parse string into Int .
                int productIdInt = ParseString.ToInt(productIdString);

                if (productIdInt != 0)
                {
                    productId = productIdInt;
                }
                  
            }

            // Brings in Interface for Product Inventory  through DI Container
            var productService = Container.GetService<IProductRepository>();

            // brings-in Product Inventory specific to the Id provided awaits till then. 
            var product = await productService.GetProductByIdAsync(productId);
            // if the product with provided Id does exits
            if (product != null)
            {
                // Prints the Product's Details
                Console.WriteLine($"Product Code: {product.Product.ProductCode}\nProduct: {product.Product.Name}\n");
                if (!string.IsNullOrEmpty(productIdStringParm))
                {
                    return product;
                }
                return null;
            }
            // if the product with provided Id does not exit
            else
            {
                // calling code sets this flag false if they want to suppress
                // the message coming in from here. 
                if (flag)
                {
                    // Prints Product not found message to the console 
                    //Console.WriteLine("Product not found");
                    // and returns null 
                    Console.WriteLine("Product not found");
                    flag = false;
                    return null;
                }
                return null;
            }
        }
        #endregion

        #region Get Product By Search String
        /// <summary>
        /// Returns Products by its name or by product code.
        /// </summary>
        /// <param name="searchString">Optional parameter, matches any letter in the table</param>
        /// <returns></returns>
        public async Task<IEnumerable<InventoryItem>> GetProductBySearchStringAsync(string searchString = "")
        {
            Console.Write("\nEnter Product name: ");
            var searchTerm = Console.ReadLine();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var customerService = Container.GetService<IProductRepository>();
                var products = await customerService.GetProductsBySearchStringAsync(searchTerm);
                Console.Write("**No.*********Product code.********** Name ********* Store ***** Quantity ****\n");
                int count = 0;
                foreach (InventoryItem c in products)
                {
                    count++;
                    Console.Write($" {count.ToString().PadLeft(3)}.  \t\t{c.Product.ProductCode} \t{c.Product.Name.PadLeft(18)} \t{c.Store.Name.PadLeft(10)} {c.Quantity.ToString().PadLeft(10)}\n");
                }
                if (count == 0)
                {
                    string text = "\nSorry this search yeilded no result, no Product with that name or product code found in the record\n";
                    foreach (char c in text)
                    {
                        Console.Write(c);
                        Thread.Sleep(40);
                    }

                    Console.Write("\n*****************************************************************************");
                    return null;
                }

                Console.Write("************************************************************************************");

            }

            return null;
        }
        #endregion

        #region Get Product by Code 
        /// <summary>
        /// Gets all Product by code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<InventoryItem> GetProductByCodeAsync(string code)
        {
            var service = Container.GetService<IProductRepository>();
            var productInventory = await service.GetProductByCodeAsync(code);
            return productInventory;
        }
        #endregion

        #region Update the Product info
        /// <summary>
        /// Lets you update product's price if you have necessary authorization. 
        /// </summary>
        /// <returns></returns>
        public async Task UpdateProductPriceAsync()
        {
            // Customer updatedCustomer, string updatedFirstName, string updatedLastName

            Console.WriteLine("************************* Welcome to the  Product Inventory Update menu ******************************\n");

            //Check if the current user is admin or not
            if (!await IdendityValidator.IsAdmin())
            {
                string text = "\nUsername and or password failed...\nPlease enter your username and password carefully next time.\nExiting to main menu now....\nAny unauthorized attempts to subvert the system will be logged and reported to the relevant authority.\n";
                foreach (char c in text)
                {
                    Console.Write(c);
                    Thread.Sleep(40);
                }
                return;
            }
            Console.WriteLine("*****************Product Inventory Item********************\n");
            Dictionary<string, string> instruments = new Dictionary<string, string>()
            {
                {"P00001", "Piano"},
                {"P00002", "Flute"},
                {"P00003", "Accordion"},
                {"P00004", "Piccolo"},
                {"P00005", "Trombone"},
                {"P00006", "Violin"},
                {"P00007", "Guitar"},
                {"P00008", "Bagpipes"},
                {"P00009", "Ukulele"},
                {"P00010", "Saxophone"}
            };

            Console.Write("*****Product Code************ Item *******\n");
            foreach (KeyValuePair<string, string> entry in instruments)
            {
                Console.WriteLine($"{entry.Key.PadLeft(15)} \t{entry.Value.PadLeft(10)}");
            }

            // Validation for product code. 
            var productCode = "";
            while (true)
            {
                Console.Write("\nEnter Product code of product to update: ");
                productCode = Console.ReadLine();

                Regex regex = new Regex(@"^[P][0]{4}[1-9]|10$");
                Match match = regex.Match(productCode);
                if (!match.Success)
                {
                    Console.WriteLine("Product code did not match: ");
                    continue;
                }
                else { break; }
            }



            var productToUpdate = await GetProductByCodeAsync(productCode);

            if (productToUpdate != null)
            {   
                Console.WriteLine($"Want to change product with Name : {productToUpdate.Product.Name}, Code : {productToUpdate.Product.ProductCode}, Price : {productToUpdate.Product.Price} 's details from db? ");
                Console.Write("Write yes and press enter to continue:");
                var result = Console.ReadLine();
                if (result.ToLower().Trim() != "yes")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Enter the updated price: ");
                    var price = Console.ReadLine();
                    var convertedPrice = 0.0M;
                    if (decimal.TryParse(price, out convertedPrice))
                    {
                        productToUpdate.ChangedDate = DateTimeOffset.Now.LocalDateTime;
                        productToUpdate.LoggedUserId = SessionHolder.UserId;
                        productToUpdate.Product.Price = convertedPrice;

                    }
                    var productService = Container.GetService<IProductRepository>();
                    await productService.UpdateProductAsync(productToUpdate);
                    SessionHolder.UserId = 0;
                    Console.WriteLine("Product Price Updated Successfully");
                    return;
                }
            }
        }
        #endregion

        #region Adds Product to a store.
        /// <summary>
        /// Adds Product Inventory to Store (increases quantity).
        /// </summary>
        /// <returns></returns>
        public async Task AddProductInventoryToStoreAsync()
        {
            // Customer updatedCustomer, string updatedFirstName, string updatedLastName

            Console.WriteLine("************************* Welcome to the  Product Inventory Update menu ******************************\n");

            //Check if the current user is admin or not
            if (!await IdendityValidator.IsAdmin())
            {
                string text = "\nUsername and or password failed...\nPlease enter your username and password carefully next time.\nExiting to main menu now....\nAny unauthorized attempts to subvert the system will be logged and reported to the relevant authority.\n";
                foreach (char c in text)
                {
                    Console.Write(c);
                    Thread.Sleep(40);
                }
                return;
            }
            Console.WriteLine("*****************Product Inventory Item********************\n");
            Dictionary<string, string> instruments = new Dictionary<string, string>()
            {
                {"P00001", "Piano"},
                {"P00002", "Flute"},
                {"P00003", "Accordion"},
                {"P00004", "Piccolo"},
                {"P00005", "Trombone"},
                {"P00006", "Violin"},
                {"P00007", "Guitar"},
                {"P00008", "Bagpipes"},
                {"P00009", "Ukulele"},
                {"P00010", "Saxophone"}
            };

            Console.Write("*****Product Code************ Item *******\n");
            foreach (KeyValuePair<string, string> entry in instruments)
            {
                Console.WriteLine($"{entry.Key.PadLeft(15)} \t{entry.Value.PadLeft(10)}");
            }

            // Validation for product code. 
            var productCode = "";
            while (true)
            {
                Console.Write("\nEnter Product code of product to update: ");
                productCode = Console.ReadLine();

                Regex regex = new Regex(@"^[P][0]{4}[1-9]|10$");
                Match match = regex.Match(productCode);
                if (!match.Success)
                {
                    Console.WriteLine("Product code did not match: ");
                    continue;
                }
                else { break; }
            }

            var productToUpdate = await GetProductByCodeAsync(productCode);

            if (productToUpdate != null)
            {
                Console.WriteLine($"You sure you want to add  {productToUpdate.Product.Name} with ,code {productToUpdate.Product.ProductCode} into the inventory? ");
                Console.Write("Write yes and press enter to continue: ");
                var result = Console.ReadLine();
                if (result.ToLower().Trim() != "yes")
                {
                    return;
                }
                else
                {
                    // Choose store location 

                    /*
                            Florida
                            New York
                            Texas
                            Washington
                            California
                     
                     */

                    string[] locations = { "Florida", "New York", "Texas", "Washington", "California" };

                    var storeLocation = "";
                    int storeId = 0;
                    while (true)
                    {
                        Console.WriteLine("\nPress 1 for Florida\nPress 2 for New York\nPress 3 for Texas\nPress 4 for Washington\nPress 5 for California\n");
                        storeLocation = Console.ReadLine();
                        if (storeLocation == "1" || storeLocation == "2" || storeLocation == "3" || storeLocation == "4" || storeLocation == "5")
                        {
                            storeId = ParseString.ToInt(storeLocation);
                            if (storeId == 0) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Invalid input"); continue; }
                            break;
                        }
                        continue;

                    }
                    var quantity = "";
                    var parsedQuantity = 0;
                    productToUpdate.StoreId = storeId;
                    while (true)
                    {
                        // Checks if StoreId 0 which means input provided was not valid one. 
                        Console.Write($"How many {productToUpdate.Product.Name} do you want to add?: ");
                        quantity = Console.ReadLine();
                        parsedQuantity = ParseString.ToInt(quantity);
                        if (parsedQuantity != 0)
                        {
                            productToUpdate.LoggedUserId = SessionHolder.UserId;
                            productToUpdate.ChangedDate = DateTimeOffset.Now.LocalDateTime;
                            productToUpdate.Quantity += parsedQuantity;
                            break;
                        }
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input");
                        continue;
                    }
                   

                    var productService = Container.GetService<IProductRepository>();
                    await productService.UpdateProductAsync(productToUpdate);
                    // Empty the session userId
                    SessionHolder.UserId = 0;
                    Console.WriteLine("Product Inventory Updated Successfully");
                    return;
                }
            }
        }
        #endregion


    }
}
