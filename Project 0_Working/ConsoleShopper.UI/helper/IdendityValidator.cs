using System;
using System.Threading.Tasks;

namespace ConsoleShopper.UI
{
    /// <summary>
    /// Validates identity of user
    /// </summary>
    public static class IdendityValidator
    {
       
        static string[]  UserInput() 
        {
            string[] inputs = new string[2];
            Console.Write("\nEnter your username: ");
            inputs[0] = Console.ReadLine();
            Console.Write("Enter your password: ");
            inputs[1] = Orb.App.Console.ReadPassword(); ;
            return inputs;
        }
        /// <summary>
        /// Check if the user is Admin
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> IsAdmin() 
        {
            var inputs = UserInput();
            CustomerCRUD CustomerCRUD = new CustomerCRUD();
            if (!await CustomerCRUD.IsAdmin(inputs[0], inputs[1]))
            {
                Console.WriteLine("Sorry you don't have the authority to do this.");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Checks if the user is Customer 
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> IsCustomer()
        {
            var inputs = UserInput();
            CustomerCRUD CustomerCRUD = new CustomerCRUD();
            if (!await CustomerCRUD.IsCustomer(inputs[0], inputs[1]))
            {
                Console.WriteLine("Sorry, Please login from your customer account to place an order.");
                return false;
            }
            return true;
        }

    }
}
