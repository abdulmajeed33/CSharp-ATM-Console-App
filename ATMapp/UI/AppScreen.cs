using ATMapp.UI;
using ATMApp.Domain.Entities;

namespace ATMapp.UI;

public class AppScreen
{
   public const string currency = "PKR "; 
   public static void Welcome()
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

    public static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new UserAccount();

            tempUserAccount.CardNumber = UI.Validator.Convert<long>("your card number");
            tempUserAccount.CardPin = Convert.ToInt32(UI.Utility.GetSecretInput("your card pin"));

            return tempUserAccount;
        }

    public static void LoginProgress()
    {
        Console.WriteLine("\nChecking your card number and pin...");
        UI.Utility.PrintDotAnimation();
    }

    public static void PrintLockScreen()
    {
       Console.Clear();
       UI.Utility.PrintMessage("Your account is locked, please contact your bank", true);
       UI.Utility.PressEnterToContinue();
       Environment.Exit(1);
    }

    public static void WelcomeCustomer(string fullName)
        {
            Console.WriteLine($"Welcome back, {fullName}");
            UI.Utility.PressEnterToContinue();
        }

    public static void DisplayAppMenu()
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

    public static int SelectAmount()
        {
            Console.WriteLine("");
            Console.WriteLine(":1.{0}500      5.{0}10,000", currency);
            Console.WriteLine(":2.{0}1000     6.{0}15,000", currency);
            Console.WriteLine(":3.{0}2000     7.{0}20,000", currency);
            Console.WriteLine(":4.{0}5000     8.{0}40,000", currency);
            Console.WriteLine(":0.Other");
            Console.WriteLine("");

            int selectedAmount = UI.Validator.Convert<int>("an option:");
            switch (selectedAmount)
            {
                case 1:
                    return 500;
                    break;
                case 2:
                    return 1000;
                    break;
                case 3:
                    return 2000;
                    break;
                case 4:
                    return 5000;
                    break;
                case 5:
                    return 10000;
                    break;
                case 6:
                    return 15000;
                    break;
                case 7:
                    return 20000;
                    break;
                case 8:
                    return 40000;
                    break;
                case 0:
                    return 0;
                    break;
                default:
                    Utility.PrintMessage("Invalid input. Try again.", false);
                    return -1;
                    break;
            }   
        }

    public InternalTransfer InternalTransferForm()
        {
           var internalTransfer = new InternalTransfer();
           internalTransfer.ReceipeintBankAccountNumber = UI.Validator.Convert<long>("recipient bank account number");
           internalTransfer.TransferAmount = UI.Validator.Convert<decimal>($"transfer {currency}");
           internalTransfer.ReceipeintBankAccountName = UI.Utility.GetUserInput("recipient bank account name");
           return internalTransfer;
        }
}
