using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMapp.UI;

namespace ATMapp.UI
{
    public static class Validator
    {
       public static T Convert<T>(string prompt)
        {
            bool valid = false;
            string userInput;

            while (!valid)
            {
                userInput = UI.Utility.GetUserInput(prompt);
                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if(converter != null)
                    {
                        return (T)converter.ConvertFromString(userInput);
                    }
                    else
                    {
                        return default;
                    }
                }
                catch
                {
                    UI.Utility.PrintMessage("Invalid input. Try again.", false);
                }
            }
            return default;
        }
    }
}
