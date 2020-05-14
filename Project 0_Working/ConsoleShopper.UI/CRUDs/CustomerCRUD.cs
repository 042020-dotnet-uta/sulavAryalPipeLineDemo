using ConsoleShopper.Domain;
using ConsoleShopper.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleShopper.UI
{
    public class CustomerCRUD
    {
        // Bringing in DI container built from ContainerBuilder.cs. 
        static readonly IServiceProvider Container = ContainerBuilder.Build();

        // flag to allow or disallow GetCustomerByIdAsync to display message. 
        // disallow if GetCustomerByIdAsync is called from other methods in this class
        bool flag = true;

        // flag to check before setting UserType. 
        bool isAdmin = false;

        #region Get Customer By Id
        /// <summary>
        /// Gets Customer by Id asynchronously 
        /// </summary>
        /// <param name="customerIdStringParm">Optional</param>
        /// <returns>Customer or null if customer not found</returns>
        public async Task<Customer> GetCustomerByIdAsync(string customerIdStringParm = "")
        {
            // to set customerId after conversion down at line no 40. 
            int customerId = 0;
            // checks if have anything coming from parameter
            if (!string.IsNullOrEmpty(customerIdStringParm))
            {

                // static helper function ParseString.ToInt
                // returns 0 if it fails to Parse String to Int .
                int customerIdInt = ParseString.ToInt(customerIdStringParm);

                // check for that 0
                if (customerIdInt != 0)
                {
                    customerId = customerIdInt;
                }

            }

            // if not asks for user's input
            else
            {
                Console.Write("\nEnter the Id of customer: ");
                var customerIdString = Console.ReadLine();

                // static helper function Converts.ToInt
                // returns 0 if it fails to Parse string into Int .
                int customerIdInt = ParseString.ToInt(customerIdString);

                if (customerIdInt != 0)
                {
                    customerId = customerIdInt;
                }
            }

            // Brings in Interface for CustomerServer through DI Container
            var customerService = Container.GetService<ICustomerRepository>();

            // brings-in Customer specific to the Id provided awaits till then. 
            var customer = await customerService.GetCustomerByIdAsync(customerId);
            // if the customer with provided Id does exits
            if (customer != null)
            {
                // Prints the Customer's Details
                Console.WriteLine(customer);
                if (!string.IsNullOrEmpty(customerIdStringParm))
                {
                    return customer;
                }
                return null;
            }
            // if the customer with provided Id does not exit
            else
            {
                // calling code sets this flag false if they want to suppress
                // the message coming in from here. 
                if (flag)
                {
                    // Prints Customer not found message to the console 
                    //Console.WriteLine("Customer not found");
                    // and returns null 
                    Console.WriteLine("Customer not found");
                    flag = false;
                    return null;
                }
                return null;
            }
        }
        #endregion

        #region Get Customer By Search String (Name)
        /// <summary>
        /// Returns Customer by first name or last name. 
        /// </summary>
        /// <param name="searchString">Optional parameter, matches any letter in the table</param>
        /// <returns></returns>
        public async Task<IEnumerable<Customer>> GetCustomerBySearchStringAsync(string searchString = "")
        {
            Console.Write("\nEnter Customer's First Name or Last Name: ");
            var searchTerm = Console.ReadLine();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var customerService = Container.GetService<ICustomerRepository>();
                var customers = await customerService.GetCustomerBySearchStringAsync(searchTerm);
                
                Console.Write("******************************************\n");
                int count = 0;
                foreach (Customer c in customers)
                {
                    count++;
                    Console.Write($"\n{c}\n");

                }
                Console.WriteLine(" ");
                if (count == 0)
                {
                    string text = "\nSorry this search yeilded no result, no customer with that First Name or Last Name found in the record\n";
                    foreach (char c in text)
                    {
                        Console.Write(c);
                        Thread.Sleep(40);
                    }
                   
                    Console.Write("\n******************************************");
                    return null;
                }

                Console.Write("******************************************");

            }

            return null;
        }
        #endregion

        #region Create a Customer

        /// <summary>
        /// Lets user to create a account, if user has further authorization, user can create an account of admin type. 
        /// </summary>
        /// <returns>void</returns>
        public async Task CreateACustomerAsync()
        {
            Console.WriteLine("************************* Welcome to the Customer Creation menu ******************************\n");
            
            Console.WriteLine("Your email address will be used as the username.");
            // initalizes the variable for holding user inputs. 
            var firstName = "";
            var lastName = "";
            var street = "";
            var city = "";
            var state = "";
            var zip = "";
            // Check for empty inputs and prompts user again to correct if invalid of empty entries attempted. 
            while (true) 
            {
                Console.Write("\nEnter your first name: ");
                firstName = Console.ReadLine();
                Console.Write("Enter your last name: ");
                lastName = Console.ReadLine();
                Console.Write("Enter your Street Address: ");
                street = Console.ReadLine();
                Console.Write("Enter your City: ");
                city = Console.ReadLine();
                Console.Write("Enter your State: ");
                state = Console.ReadLine();
                Console.Write("Enter your Zip: ");
                zip = Console.ReadLine();

                // Sanitize the inputs 
                if (firstName.Sanitize() == "empty" || lastName.Sanitize() == "empty" || street.Sanitize() == "empty" ||
                    city.Sanitize() == "empty" || state.Sanitize() == "empty" || zip.Sanitize() == "empty" ) 
                {
                    Console.WriteLine("System doesn't accept empty entries \nPlease enter again, or press Q to return to the main menu ");
                    var keyinfo = Console.ReadKey();
                   
                    if (keyinfo.Key == ConsoleKey.Q) { Console.Clear(); return; }
                    continue;
                }
                firstName.Sanitize();
                lastName = lastName.Sanitize();
                street = street.Sanitize();
                city = city.Sanitize();
                state = state.Sanitize();
                zip = zip.Sanitize();
                break;
            }
           

            var email = "";
            var phoneNumber = "";
            // password must atleast contain one digit, one upper case and one lower case with no whitespace
            var password = "";
            bool fromPhoneNumber = false;
            bool fromPassword = false;
            while (true) 
            {
                if (fromPhoneNumber == false || fromPassword == false) 
                {
                    Console.Write("Enter your email: ");
                    email = Console.ReadLine();
                    // Sanitize email 
                    email = email.Sanitize();
                    if (!email.IsValidEmail())
                    {
                        Console.WriteLine("Invalid Email format");
                        continue;
                    }
                }

                if (fromPassword == false) 
                {
                    Console.Write("Enter your phone no: ");
                    phoneNumber = Console.ReadLine();
                    // Sanitize phone number
                    phoneNumber = phoneNumber.Sanitize();
                    if (!phoneNumber.IsValidPhoneNumber())
                    {
                        Console.WriteLine("Invalid Phone number format");
                        Console.Write($"\t123-456-7890\n\t(123) 456-7890\n\t123 456 7890\n\t123.456.7890\n\t+91 (123) 456-7890\t");
                        fromPhoneNumber = true;
                        continue;
                    }
                }
               
                Console.Write("Password must contain atleast contain one digit, one upper case and one lower case and no whitespace\n");
                Console.Write("Enter your preferred password: ");
                // Orb.App.Console.ReadPassword(), masks intput characters with * 
                password = Orb.App.Console.ReadPassword();
                // masks intput characters with*
                Console.Write("Enter your password again for confirmation: ");
                var password2 = Orb.App.Console.ReadPassword();

                if (password.IsValidPassword())
                {
                    Console.Clear();
                    var origRow = Console.CursorTop;
                    var origCol = Console.CursorLeft;
                    Console.Clear();
                    if (password != password2)
                    {
                        Console.WriteLine("Sorry your password didn't match");
                       
                        continue;
                    }
                    else { break; }
                }
                else
                {
                    Console.WriteLine("Enter valid password...");
                    continue;
                }
            }

            // Check if user has Admin privilege 
            Console.Write("Are you one of our staff members? \nType yes and press enter if you are, otherwise press any other key to continue : ");
            string staffCheck = "";
            staffCheck = Console.ReadLine();
            if (staffCheck.Trim().ToLower() == "yes")
            {
                if (!await IdendityValidator.IsAdmin())
                {
                    // loops through the character of string on by one and display it in the console. 
                    // link : https://stackoverflow.com/questions/27718901/writing-one-character-at-a-time-in-a-c-sharp-console-application
                    // author : https://stackoverflow.com/users/22656/jon-skeet
                    string text = "\nUsername and or password failed...\nPlease enter your username and password carefully next time.\nExiting to main menu now....\nAny unauthorized attempts to subvert the system will be logged and reported to the relevant authority.\n";
                    foreach (char c in text)
                    {
                        Console.Write(c);
                        Thread.Sleep(40);
                    }
                    return;
                }
                else
                { isAdmin = true; }
            }

            if (!string.IsNullOrEmpty(firstName) &&
                !string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(password))
            {

                // Create a customer from user inserted strings

                var customerAddress = new CustomerAddress { Street = street, City = city, State = state, Zip = zip };
                var customer = new Customer { FirstName = firstName, LastName = lastName, Password = password,Email = email,PhoneNo = phoneNumber, UserTypeId = isAdmin ? 1 : 2, CustomerAddress = customerAddress };


                // conjure up an interface to service layer 
                var insertCustomer = Container.GetService<ICustomerRepository>();

                // pass the customer up the chain through DI injected dbcontext to the service layer
                await insertCustomer.InsertCustomerAsync(customer);
                Console.WriteLine("Customer Creation successful");
                Console.WriteLine($"Welcome to the Console Shopper family {firstName} {lastName}");
                if (isAdmin) { isAdmin = false; }
            }
            else { return; }

        }
        #endregion

        #region Update the Customer info
        /// <summary>
        /// Lets you update Customer's details if you have necessary authorization. 
        /// </summary>
        /// <returns></returns>
        public async Task UpdateTheCustomerAsync()
        {
            // Customer updatedCustomer, string updatedFirstName, string updatedLastName

            Console.WriteLine("\n************************* Welcome to the  Customer Update menu ******************************\n");

            // Check if the current user is admin or not 
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

            Console.Write("\nEnter Id of the Customer you want update: ");
            var customerId = Console.ReadLine();
            Program p = new Program();

            var customerToUpdate = await GetCustomerByIdAsync(customerId);
            if (customerToUpdate != null)
            {
                Console.WriteLine($"You sure want to Update {customerToUpdate.FirstName} {customerToUpdate.LastName} 's details from db? ");
                Console.Write("Write yes and press enter to continue: ");
                var result = Console.ReadLine();
                if (result.ToLower().Trim() != "yes")
                {
                    return;
                }
                else
                {
                    Console.Write("Write the updated first name: ");
                    var updatedFirstName = Console.ReadLine();
                    Console.Write("Write the updated last name: ");
                    var updatedLastName = Console.ReadLine();

                    customerToUpdate.FirstName = updatedFirstName;
                    customerToUpdate.LastName = updatedLastName;
                    var customerService = ContainerBuilder.Build().GetService<ICustomerRepository>();
                    await customerService.UpdateCustomerAsync(customerToUpdate);
                    Console.Write("Record Succesfully Updated");
                    Console.Write("\nReturning to the main menu...");
                    Thread.Sleep(3000);
                }
            }
        }
        #endregion

        #region Delete A customer

        /// <summary>
        /// Deletes Customer form the database if the user has authorization. 
        /// </summary>
        /// <returns></returns>
        public async Task DeleteCustomerAsync()
        {
            Console.WriteLine("\n************************* Welcome to the Customer delete menu ******************************\n");

            // Check if the current user is admin or not 
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

            // Asks for the Customer's Id whom should be deleted. 
            Console.Write("Enter Id of the Customer you want delete: ");

            var customerId = Console.ReadLine();

            // flag to disallow GetCustomerByIdAsync method to display message. 
            flag = false;
            // Note : customerId of type string gets converted to int inside GetCustomerByIdAsync method 
            var customerToDelete = await GetCustomerByIdAsync(customerId);

            var result1 = customerToDelete;
            var customerService = ContainerBuilder.Build().GetService<ICustomerRepository>();
            if (customerToDelete != null)
            {
                Console.WriteLine($"You sure want to delete {customerToDelete.FirstName} {customerToDelete.LastName} with Id {customerToDelete.Id} from db? ");
                var result = Console.ReadLine();
                if (result.ToLower() == "yes")
                {
                    await customerService.DeleteCustomerAsync(customerToDelete.Id);
                    Console.WriteLine($"{customerToDelete.FirstName} {customerToDelete.LastName} deleted.");
                }
            }
            else
            {
                Console.WriteLine("Customer not found");
            }
        }
        #endregion

        #region Check for user privelege

        /// <summary>
        /// Tests for users authorization level 
        /// </summary>
        /// <param name="username">string</param>
        /// <param name="password">string</param>
        /// <returns>bool</returns>
        public async Task<bool> IsAdmin(string username, string password)
        {
            var validCustomer = Container.GetService<ICustomerRepository>();
            var validity = await validCustomer.IsAdmin(username, password);
            SessionHolder.UserId = validity.Item2;
            return (validity.Item1);
        }
        #endregion

        /// <summary>
        /// Checks for Customer privilege
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> IsCustomer(string username, string password)
        {
            var validCustomer = ContainerBuilder.Build().GetService<ICustomerRepository>();
            var validity = await validCustomer.IsCustomer(username, password);
            SessionHolder.UserId = validity.Item2;
            return (validity.Item1);
        }


    }

}
