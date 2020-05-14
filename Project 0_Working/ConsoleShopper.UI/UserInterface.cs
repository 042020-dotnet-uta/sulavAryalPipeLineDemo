using Pastel;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace ConsoleShopper.UI
{
    public class UserInterface
    {

        CustomerCRUD customerCRUD = new CustomerCRUD();
        ProductCRUD productCRUD = new ProductCRUD();
        OrderCRUD orderCRUD = new OrderCRUD();
        /// <summary>
        /// Entry point to the application. 
        /// </summary>
        /// <returns></returns>
        public async Task MainMenu()
        {
            //System.Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            

         
            const string title = @"##     ## ##     ##  ######  ####  ######      ######  ########  #######  ########  ########         
###   ### ##     ## ##    ##  ##  ##    ##    ##    ##    ##    ##     ## ##     ## ##               
#### #### ##     ## ##        ##  ##          ##          ##    ##     ## ##     ## ##               
## ### ## ##     ##  ######   ##  ##           ######     ##    ##     ## ########  ######           
##     ## ##     ##       ##  ##  ##                ##    ##    ##     ## ##   ##   ##               
##     ## ##     ## ##    ##  ##  ##    ##    ##    ##    ##    ##     ## ##    ##  ##       ### ### 
##     ##  #######   ######  ####  ######      ######     ##     #######  ##     ## ######## ### ### ";
            while (true)
            {
                System.Console.Title = "We can be instrumental on your instrument purchase.";
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n"+title+"\n");

                System.Console.Write("\nPress 1 to get to the main menu ");
                var input = System.Console.ReadLine();
                if (input != "1")
                {

                    //Console.ForegroundColor = ConsoleColor.Red;
                    //System.Console.WriteLine("Aborting Shopping now...!");
                    //"ENTER".Pastel(Color.FromArgb(165, 229, 250));

                    //System.Console.WriteLine($"Press {"ENTER".Pastel(Color.FromArgb(165, 229, 250))} to exit");
                    System.Console.ReadKey();
                    continue;
                    
                    
                }
                else
                {
                    System.Console.WriteLine("\n********************Welcome to the Main menu************************");

                    System.Console.WriteLine("\nPress 1 to Get to the Search Menu \nPress 2 to Register as a Customer \nPress 3 to Get to the Update Menu \nPress 4 to Delete the Customer\nPress 5 to Place an Order\nPress 6 to see the Order list.\n");
                    System.Console.Write("\nEnter your Choice: ");

                    input = System.Console.ReadLine();

                    if (input == "1")
                    {
                        //  Search menu for Id wise or name wish search. 
                        Console.WriteLine(title);
                        System.Console.WriteLine("\n*****************************Welcome to the Search menu ********************************* ");
                        System.Console.WriteLine($"\nPress 1 to search for Customer by Id,\nPress 2 to search Customers by Name\nPress 3 to search Product by Id");
                        System.Console.WriteLine($"Press 4 to Search Product by name");

                        System.Console.Write("\nEnter your Choice: ");

                        input = System.Console.ReadLine();
                        if (input == "1")
                        {
                            try
                            {
                                System.Console.Clear();
                                Console.WriteLine(title);
                                await customerCRUD.GetCustomerByIdAsync();
                            }
                            catch (Exception e)
                            {
                                System.Console.WriteLine(e.Message);
                            }
                        }
                        else if (input == "2")
                        {
                            try
                            {
                                System.Console.Clear();
                                Console.WriteLine(title);
                                await customerCRUD.GetCustomerBySearchStringAsync();
                            }
                            catch (Exception e)
                            {

                                System.Console.WriteLine(e.Message);
                            }
                        }
                        else if (input == "3")
                        {

                            try
                            {
                                System.Console.Clear();
                                Console.WriteLine(title);
                                await productCRUD.GetProductByIdAsync();
                            }
                            catch (Exception e)
                            {

                                System.Console.WriteLine(e.Message);
                            }
                        }
                        else if (input == "4")
                        {
                            try
                            {
                                //System.Console.Clear();
                                //Console.WriteLine(title);
                                await productCRUD.GetProductBySearchStringAsync();
                            }
                            catch (Exception e)
                            {

                                System.Console.WriteLine(e.Message);
                            }
                        }
                        else { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Invalid Input"); continue; }


                    }
                    else if (input == "2")
                    {
                        // Creates a Customer
                        try
                        {
                            //System.Console.Clear();
                            //Console.WriteLine(title);
                            await customerCRUD.CreateACustomerAsync();
                        }
                        catch (Exception e)
                        {
                            System.Console.WriteLine(e.Message);

                        }

                    }
                    else if (input == "3")
                    {
                        System.Console.Clear();
                        Console.WriteLine(title);
                        System.Console.Write("\n *********************Welcome to the Update menu *************** \n");
                        System.Console.Write("\nPress 1 to get to the Customer update menu ");
                        System.Console.Write("\nPress 2 to get to the Product Inventory update price menu");
                        System.Console.Write("\nPress 3 to get to the Product Inventory update quantity menu \n");
                        System.Console.Write("\nEnter your Choice: ");
                        input = Console.ReadLine();
                        if (input == "1")
                        {
                            // Updates a Customer 
                            try
                            {
                                System.Console.Clear();
                                Console.WriteLine(title);
                                await customerCRUD.UpdateTheCustomerAsync();
                            }
                            catch (Exception e)
                            {
                                System.Console.WriteLine(e.Message);

                            }
                        }
                        else if (input == "2")
                        {
                            // Updates a Product Inventory 
                            try
                            {
                                System.Console.Clear();
                                Console.WriteLine(title);
                                await productCRUD.UpdateProductPriceAsync();
                            }
                            catch (Exception e)
                            {
                                System.Console.WriteLine(e.Message);

                            }
                        }
                        else if (input == "3")
                        {
                            try
                            {
                                System.Console.Clear();
                                Console.WriteLine(title);
                                await productCRUD.AddProductInventoryToStoreAsync();
                            }
                            catch (Exception e)
                            {

                                System.Console.WriteLine(e.Message);
                            }
                        }
                        else { continue; }

                    }
                    else if (input == "4")
                    {
                        // Deletes a Customer
                        try
                        {
                            System.Console.Clear();
                            Console.WriteLine(title);
                            await customerCRUD.DeleteCustomerAsync();
                        }
                        catch (Exception e)
                        {

                            System.Console.WriteLine(e.Message);
                        }

                    }
                    else if (input == "5")
                    {
                        try
                        {
                            await orderCRUD.AddOrderToStoreAsync();
                        }
                        catch (Exception e)
                        {

                            System.Console.WriteLine(e.Message);
                        }
                    }
                    else if (input == "6")
                    {
                        Console.WriteLine(title);
                        System.Console.WriteLine("\n*****************************Welcome to the Order Details menu ********************************* ");
                        System.Console.WriteLine($"\nPress 1 to list all the orders,\nPress 2 to List Orders by Store Location\nPress 3 to list Order by Id\nPress 4 to list order by Customer Id");

                        System.Console.Write("\nEnter your Choice: ");

                        input = System.Console.ReadLine();
                        if (input == "1")
                        {
                            try
                            {
                                await orderCRUD.GetAllOrdersAsync();
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine(e.Message);
                            }

                        }
                        else if (input == "2")
                        {
                            try
                            {

                                await orderCRUD.GetItemizedOrdersByStore();
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine(e.Message);
                            }

                        }
                        else if (input == "3")
                        {
                            try
                            {

                                //await orderCRUD.GetItemizedOrdersByStore();
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine(e.Message);
                            }

                        }
                        else if (input == "4") 
                        {
                            try
                            {
                                await orderCRUD.GetItemizedOrdersByCustomerId();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                throw;
                            }
                            
                        }
                        else { continue; }

                    }
                    else 
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Input\n"); continue;
                    }

                }
            }
        }

    }
}
