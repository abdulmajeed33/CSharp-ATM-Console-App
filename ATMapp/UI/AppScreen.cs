using ATMapp.UI;
using ATMApp.Domain.Entities;

namespace ATMapp.UI;

public static class AppScreen
{
   internal const string currency = "PKR "; 
   internal static void Welcome()
        {
            //clears the console screen
            Console.Clear();
            //sets the title of the console window
            Console.Title = "My ATM App";
            //sets the text color or foreground color to white
            Console.ForegroundColor = ConsoleColor.White;

            //set the welcome message 
            Console.WriteLine("\n\n-----------------Welcome to My ATM App-----------------\n\n");
            //prompt the user to insert atm card
            Console.WriteLine("Please insert your ATM card");
            Console.WriteLine("Note: Actual ATM machine will accept and validate" +
                " a physical ATM card, read the card number and validate it.");
            //prompt the user to press enter to continue
            UI.Utility.PressEnterToContinue();
        }

    internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new UserAccount();

            tempUserAccount.CardNumber = UI.Validator.Convert<long>("your card number");
            tempUserAccount.CardPin = Convert.ToInt32(UI.Utility.GetSecretInput("your card pin"));

            return tempUserAccount;
        }

    internal static void LoginProgress()
    {
        Console.WriteLine("\nChecking your card number and pin...");
        UI.Utility.PrintDotAnimation();
    }

    internal static void PrintLockScreen()
    {
       Console.Clear();
       UI.Utility.PrintMessage("Your account is locked, please contact your bank", true);
       UI.Utility.PressEnterToContinue();
       Environment.Exit(1);
    }

    internal static void WelcomeCustomer(string fullName)
        {
            Console.WriteLine($"Welcome back, {fullName}");
            UI.Utility.PressEnterToContinue();
        }

    internal static void DisplayAppMenu()
        {
            Console.Clear();
            Console.WriteLine("-----------------ATM App Menu-----------------");
            Console.WriteLine("1. Account Balance                           :");
            Console.WriteLine("2. Cash Deposit                              :");
            Console.WriteLine("3. Cash Withdrawal                           :");
            Console.WriteLine("4. Transfer Funds                            :");
            Console.WriteLine("5. Transaction                               :");
            Console.WriteLine("6. Logout                                    :");
        }
}
