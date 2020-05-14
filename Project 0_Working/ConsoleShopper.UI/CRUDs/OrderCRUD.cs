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
    // link - https://stackoverflow.com/questions/141088/what-is-the-best-way-to-iterate-over-a-dictionary
    public class OrderCRUD
    {
        // Bringing in DI container built from ContainerBuilder.cs. 
        static readonly IServiceProvider Container = ContainerBuilder.Build();


        /// <summary>
        /// Adds order from the customer to the database. 
        /// </summary>
        /// <returns></returns>
        public async Task AddOrderToStoreAsync()
        {
            #region List of things done in this method. 
            /*
             *   * 1. Check for users Cusomer's Credentials. 
                 * Create order and orderline objects. 

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
            #endregion

            Console.WriteLine("************************* Welcome to the  Order menu ******************************\n");

            #region 1. Customer check (Username Susy: Password: password)
            //Check if the current user is admin or not
            if (!await IdendityValidator.IsCustomer())
            {
                string text = "\nYou will need to login from your Customer account to place an order....\nExiting to main menu now....\nAny unauthorized attempts to subvert the system will be logged and reported to the relevant authority.\n";
                foreach (char c in text)
                {
                    Console.Write(c);
                    Thread.Sleep(40);
                }
                return;
            }
            #endregion

            #region 2. StordId Setter
            // Ask user to choose the store location. 
            string[] locations = { "Florida", "New York", "Texas", "Washington", "California" };

            var storeLocation = "";
            int storeId = 0;
            while (true)
            {
                Console.Write("Please choose a store location to place the order from\n");
                Console.WriteLine("\nPress 1 for Florida\nPress 2 for New York\nPress 3 for Texas\nPress 4 for Washington\nPress 5 for California\n");
                Console.Write("Enter your choice: ");
                storeLocation = Console.ReadLine();
                if (storeLocation == "1" || storeLocation == "2" || storeLocation == "3" || storeLocation == "4" || storeLocation == "5")
                {

                    storeId = ParseString.ToInt(storeLocation);
                    if (storeId == 0) { Console.ForegroundColor = ConsoleColor.Red;  Console.WriteLine("Invalid input"); continue; }
                    break;
                }
                continue;
            }
            #endregion StoreId setter

            #region Get All the products from Inventory By StoreId Set above and loop through and display those. 
            var productService = Container.GetService<IProductRepository>();
            var result = await productService.GetProductsByStoreAsync(storeId);
            // Create an empty dictionary
            Dictionary<int, int> dbValuePairs = new Dictionary<int, int>();
            Dictionary<int, int> productIdQuantityPair = new Dictionary<int, int>();
            Dictionary<int, int> productCart = new Dictionary<int, int>();
            List<string> productNames = new List<string>();

            int count = 0;
            Console.WriteLine($"Code*********Name*******Store****Quantity*****************************************");
            foreach (InventoryItem i in result)
            {
                count++;
                dbValuePairs.Add(i.ProductId, i.Quantity);
                // inititalize second dict with same key but with values 0
                productIdQuantityPair.Add(i.ProductId, 0);
                productNames.Add(i.Product.Name);
                Console.WriteLine($"{i.Product.ProductCode.PadRight(5)} \t{i.Product.Name.PadLeft(10)}  {i.Store.Name.PadLeft(10)} {i.Quantity.ToString().PadLeft(7)}");
            }
            #endregion


            #region Creates a Cart that holds the user inputted purchases for a time being. 
            // Allow users to choose multiple products with multiple quantity.
            int productCount = productNames.Count;
            var productCode = "";
            int userProductCode = 0;
            int userQuantity = 0;
            bool flag = false;
            while (true)
            {

                Console.Write("\nEnter Product code of product to purchase: ");
                productCode = Console.ReadLine();

                Regex regex = new Regex(@"^[P][0]{4}[1-9]|10$");
                Match match = regex.Match(productCode);
                if (!match.Success)
                {
                    Console.WriteLine("Product code did not match: ");
                    continue;
                }
                else
                {
                    Console.WriteLine("Refer to the product inventory table above before entering amount\nYour purchase cannot exceed quantity available at the store\n");
                    Console.Write("Enter the amount you want to purchase: ");
                    var amount = Console.ReadLine();
                    userQuantity = ParseString.ToInt(amount);
                    if (userQuantity == 0) 
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input");
                        continue;
                    }
                    productCode = productCode.Substring(4, 2);
                    userProductCode = ParseString.ToInt(productCode);

                    if (userQuantity != 0 || userProductCode != 0)
                    {
                        if (productCart.ContainsKey(userProductCode))
                        {
                            flag = true;
                        }

                        productIdQuantityPair[userProductCode] += userQuantity;
                        //Console.WriteLine($" added : {productIdQuantityPair[userProductCode]}");
                        foreach (var key in dbValuePairs.Keys)
                        {
                            if ((dbValuePairs[userProductCode] >= productIdQuantityPair[userProductCode]))
                            {
                                if (flag == false)
                                {
                                    productCart.Add(userProductCode, productIdQuantityPair[userProductCode]);
                                    //Console.WriteLine($"Added: { productIdQuantityPair[userProductCode]}");
                                    Console.WriteLine("\nProduct added to the cart successfully\n");
                                    flag = true;
                                    break;
                                }
                                else
                                {
                                    productCart[userProductCode] = productIdQuantityPair[userProductCode];
                                    Console.WriteLine("\nProduct added to the cart successfully\n");
                                    break;
                                }

                            }
                            else
                            {
                                Console.WriteLine("\nSorry cart cannot hold more items than that can be purchased...\n");
                                flag = false;
                                break;
                            }
                        }

                        flag = false;
                        Console.Write("\nDo you want to continue adding items to the cart?\n");
                        Console.Write("\nType \"yes\" if you want to continue, press any other key to go to the purchase menu: ");
                        var input = Console.ReadLine();
                        if (input.Trim().ToLower() == "yes")
                        {
                            continue;
                        }
                        break;
                    }

                    else { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Invalid Input"); continue; }

                }

            }
            #endregion

            #region loops through the cart above and displays the list of choices by user. 
            foreach (KeyValuePair<int, int> entry in productCart)
            {
                Console.WriteLine($"Your cart entries {productNames[entry.Key - 1]} {entry.Value}");
            }
            #endregion

            #region Asks for users confirmation for purchase. Displays total etc. 
            while (true)
            {
                Console.Write("Do you want to purchase these products now ? \nType yes or no to confirm or deny on next prompt\n");
                Console.Write("Enter your choice: ");
                var confirmPurchase = Console.ReadLine();

                #region If confirmed by user we create the objects to be inserted. 

                if (confirmPurchase.ToLower().Trim() == "yes")
                {
                    Console.WriteLine("Excellent!");
                    var userId = SessionHolder.UserId;
                    Order order = new Order
                    {
                        CustomerId = userId,
                        StoreId = storeId,
                        OrderDate = DateTimeOffset.Now.LocalDateTime
                    };

                    decimal total = 0.0M;
                    count = 0;

                    var products = await productService.GetAllProductsAsync();
                    List<OrderLineItem> orderLineItems = new List<OrderLineItem>();
                    Console.WriteLine("****************************Billing********************");
                    foreach (KeyValuePair<int, int> entry in productCart)
                    {

                        foreach (Product i in products)
                        {
                            if (i.Id == entry.Key)
                            {
                                count++;
                                orderLineItems.Add(
                                new OrderLineItem
                                {
                                    Order = order,
                                    InventoryItemId = entry.Key,
                                    Quantity = entry.Value,
                                    Price = 150.55M
                                });
                                Console.WriteLine($"{count}. {productNames[entry.Key - 1]} {entry.Value} {i.Price}");
                                total += entry.Value * i.Price;
                            }

                        }

                    }
                    Console.WriteLine($"The total amount of purchase is : ${total}");
                    order.OrderLineItems = orderLineItems;
                    var createOrder = Container.GetService<IOrderService>();
                    await createOrder.CreateOrder(order);
                    Console.WriteLine("Order dispatched");
                    return;

                }

                #endregion

                #region If not confirmed say bye bye!
                else if (confirmPurchase.ToLower().Trim() == "no")
                {
                    Console.WriteLine("Nice meeting ya, please come again later...");
                    return;
                }
                #endregion
                else { continue; }

            }
            #endregion
        }

        #region Get All Orders
        /// <summary>
        /// Lists all the orders by all the customers. 
        /// </summary>
        /// <returns></returns>
        public async Task GetAllOrdersAsync()
        {
            var orderService = Container.GetService<IOrderService>();
            var orders = await orderService.GetAllOrdersAsync();
            int count = 0;
            var productRepository = Container.GetService<IProductRepository>();

            var products = await productRepository.GetAllProductsAsync();

            // List of Orders 
            Console.WriteLine("************************List of Orders***********************************\n");
            Console.WriteLine("No.*FName***LName**Product***Order*******Date***********Store******Price*");
            foreach (Order o in orders)
            {
                foreach (OrderLineItem oi in o.OrderLineItems)
                {
                    foreach (Product p in products)
                    {

                        if (oi.InventoryItemId % 10 == p.Id)
                        {
                            count++;
                            Console.WriteLine($"{count.ToString().PadLeft(2)} {o.Customer.FirstName.PadRight(8)} {o.Customer.LastName.PadRight(6)} {p.Name.PadRight(9)} {o.OrderDate} {o.Store.Name.PadRight(10)} ${p.Price}");
                        }

                    }
                }
            }
            Console.WriteLine("**************************************************************************\n");
            Thread.Sleep(3000);

        }
        #endregion

        #region Get Orders by Store
        /// <summary>
        /// Gets all the order by Store
        /// </summary>
        /// <returns></returns>
        public async Task GetItemizedOrdersByStore()
        {
            Console.WriteLine("******************************Get Order by Store Name******************************");

            #region 1. StordId Setter
            // Ask user to choose the store location. 
            string[] locations = { "Florida", "New York", "Texas", "Washington", "California" };

            var storeLocation = "";
            int storeId = 0;
            while (true)
            {
                Console.Write("Please choose a store location\n");
                Console.WriteLine("\nPress 1 for Florida\nPress 2 for New York\nPress 3 for Texas\nPress 4 for Washington\nPress 5 for California\n");
                Console.Write("Enter your choice: ");
                storeLocation = Console.ReadLine();
                if (storeLocation == "1" || storeLocation == "2" || storeLocation == "3" || storeLocation == "4" || storeLocation == "5")
                {

                    storeId = ParseString.ToInt(storeLocation);
                    if (storeId == 0) { Console.ForegroundColor = ConsoleColor.Red;  Console.WriteLine("Invalid input"); continue; }
                    break;
                }
                continue;
            }
            #endregion StoreId setter

            var orderService = Container.GetService<IOrderService>();
            var orders = await orderService.GetOrderByStoreId(storeId);
            var productRepository = Container.GetService<IProductRepository>();
            var products = await productRepository.GetAllProductsAsync();

            int count = 0;
            Console.WriteLine("");
            foreach (var o in orders)
            {
                foreach (OrderLineItem oi in o.OrderLineItems)
                {

                    foreach (Product p in products)
                    {

                        if (oi.InventoryItemId % 10 == p.Id)
                        {
                            count++;
                            Console.WriteLine($"{count.ToString().PadLeft(2)}. {o.Store.Name}: Order No: {o.Id} of {p.Name}  purchased by {o.Customer.FirstName} on {o.OrderDate}");
                        }
                    }
                }
            }
            Console.WriteLine("\n");
        }
        #endregion

        #region Get Orders by CustomerId
        /// <summary>
        /// Gets all the orders by Customer Id. 
        /// </summary>
        /// <returns></returns>
        public async Task GetItemizedOrdersByCustomerId()
        {
            Console.WriteLine("******************************Get Order by Customer Id ******************************");

            #region 1. CustomerId Setter
            // Ask user to choose a customer by Id. 

            List<int> custId = new List<int>();
            int customerId = 0;
            var customerRepository = Container.GetService<ICustomerRepository>();
            var customers = await customerRepository.GetAllCustomersAsync();
            if (customers ==  null) 
            {
                Console.WriteLine("Customer not found in the database");
                return;
            }
            while (true)
            {
                Console.Write("Please choose a Customer's Id to Get list of orders from\n");

                foreach (Customer c in customers)
                {
                    custId.Add(c.Id);
                    Console.WriteLine($"Id {c.Id} First Name:  {c.FirstName} Last Name: {c.LastName} Email {c.Email}");
                }
                Console.Write("Enter your choice of Id: ");
                var input = Console.ReadLine();

                customerId = ParseString.ToInt(input);
                if (customerId == 0) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Invalid input"); continue; }
                break;

            }
            #endregion customerId setter

            var orderService = Container.GetService<IOrderService>();
            var orders = await orderService.GetOrdersByCustomerId(customerId);
            var productRepository = Container.GetService<IProductRepository>();
            var products = await productRepository.GetAllProductsAsync();

            int count = 0;
            foreach (var o in orders)
            {
                foreach (OrderLineItem oi in o.OrderLineItems)
                {

                    foreach (Product p in products)
                    {

                        if (oi.InventoryItemId % 10 == p.Id)
                        {
                            count++;
                            Console.WriteLine($"{count.ToString().PadLeft(2)}. {o.Store.Name}: Order No: {o.Id} of {p.Name}  purchased by {o.Customer.FirstName} on {o.OrderDate}");
                        }
                    }
                }
            }
            Console.ReadLine();
        }
        #endregion



    }

}

