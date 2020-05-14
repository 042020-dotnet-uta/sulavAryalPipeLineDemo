using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleShopper.UI
{
    public static class ParseString
    {
        /// <summary>
        /// Takes in a string and outputs int, 
        /// if failed returns 0, which calling caller should check for. 
        /// </summary>
        /// <param name="parseToInt">String</param>
        /// <returns>Int</returns>
        public static int ToInt(string parseToInt) 
        {
            int number;
            bool success = Int32.TryParse(parseToInt, out number);
            if (success)
            {
                return number;
            }
            return 0;
        }
    }
}
