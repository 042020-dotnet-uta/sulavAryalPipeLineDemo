using ConsoleShopper.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
namespace ConsoleShopper
{

    class Program
    {

       

        public static async Task Main(string[] args)
        {

           

            UserInterface ui = new UserInterface();
            await ui.MainMenu();

        }
    }

}
