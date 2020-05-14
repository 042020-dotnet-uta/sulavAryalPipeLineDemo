using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;


/// <summary>
/// link : https://stackoverflow.com/questions/3404421/password-masking-console-application
/// author : https://stackoverflow.com/users/892926/shermy
/// </summary>
namespace Orb.App
{
    public static class Console
    {
        /// <summary>
        /// Like System.Console.ReadLine(), only with a mask.
        /// </summary>
        /// <param name="mask">a <c>char</c> representing your choice of console mask</param>
        /// <returns>the string the user typed in </returns>
        public static string ReadPassword(char mask)
        {
            //const int ENTER = 13;
            const int BACKSP = 8, CTRLBACKSP = 127;
            int[] FILTERED = { 0, 27, 9, 10,32 /*, 32 space, if you care */ }; // const

            // creates a stack 
            var pass = new Stack<char>();

            //initilizes a char type. 
            char chr = (char)0;
            
            //  (chr = System.Console.ReadKey(true).KeyChar)

            while (!Environment.NewLine.Contains(chr = System.Console.ReadKey(true).KeyChar))
            {
                if (chr == BACKSP)
                {
                    if (pass.Count > 0)
                    {
                        System.Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (chr == CTRLBACKSP)
                {
                    while (pass.Count > 0)
                    {
                        System.Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (FILTERED.Count(x => chr == x) > 0) { }
                else
                {
                    pass.Push((char)chr);
                    System.Console.Write(mask);
                }
            }

            System.Console.WriteLine();

            return new string(pass.Reverse().ToArray());
        }

        /// <summary>
        /// Like System.Console.ReadLine(), only with a mask.
        /// </summary>
        /// <returns>the string the user typed in </returns>
        public static string ReadPassword()
        {
            return Orb.App.Console.ReadPassword('*');
        }
    }

}
