using ATMApp.Domain.Entities;
using ATMapp.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using ATMApp.Domain.Interfaces;
using ATMapp;
using ATMApp.Domain.Enums;

namespace ATMapp
{
   public class ATMapp : IUserLogin, IUserAccountActions,ITransaction
    {
        private static List<UserAccount> userAccountList;
        private UserAccount selectedAccount;
        private List<Transaction> _listOfTransactions;
        private const decimal minimumKeptAmount = 500;

        public void Run()
        {
            UI.AppScreen.Welcome();
            CheckUserCardNumAndPassword();
            UI.AppScreen.WelcomeCustomer(selectedAccount.FullName);
            UI.AppScreen.DisplayAppMenu();
            ProcessMenuOption();
        }
        

        public void InitializeData()
        {
            userAccountList = new List<UserAccount>
            {
                new UserAccount{Id=1, FullName = "Obinna Ezeh", AccountNumber=123456,CardNumber =321321, CardPin=123123,AccountBalance=50000.00m,IsLocked=false},
                new UserAccount{Id=2, FullName = "Amaka Hope", AccountNumber=456789,CardNumber =654654, CardPin=456456,AccountBalance=4000.00m,IsLocked=false},
                new UserAccount{Id=3, FullName = "Femi Sunday", AccountNumber=123555,CardNumber =987987, CardPin=789789,AccountBalance=2000.00m,IsLocked=true},
            };
            _listOfTransactions = new List<Transaction>();

        }

        public void CheckUserCardNumAndPassword()
        {
            bool isCorrectLogin = false;

            while (!isCorrectLogin)
            {
               
                UserAccount inputAccount = UI.AppScreen.UserLoginForm();
                UI.AppScreen.LoginProgress();
                foreach(UserAccount account in userAccountList)
                {
                    selectedAccount = account;
                    if(inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                    {
                        selectedAccount.TotalLogin++;

                        if (inputAccount.CardPin.Equals(selectedAccount.CardPin))
                        {
                            selectedAccount = account;

                            if(selectedAccount.IsLocked || selectedAccount.TotalLogin > 3 )
                            {
                                UI.AppScreen.PrintLockScreen();
                            }else
                            {
                                selectedAccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                break;
                            }
                        }
                        else
                        {
                            UI.Utility.PrintMessage("Invalid card pin", false);
                            break;
                        }
                    }
                    if(!isCorrectLogin)
                    {
                        UI.Utility.PrintMessage("\n Invalid card number or pin", false);
                        selectedAccount.IsLocked = selectedAccount.TotalLogin == 3;
                        if(selectedAccount.IsLocked)
                        {
                            UI.AppScreen.PrintLockScreen();
                        }
                    }
                    Console.Clear();
                }

            }
        }

        private void ProcessMenuOption()
        {
            int option = UI.Validator.Convert<int>("an option:");
            switch (option)
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;
                case (int)AppMenu.MakeWithdrawal:
                    MakeWithDrawal();
                    break;
                case (int)AppMenu.InternalTransfer:
                    Console.WriteLine("Enter the amount you want to transfer:");
                    break;
                case (int)AppMenu.ViewTransaction:
                    Console.WriteLine("Enter the amount you want to view:");
                    break;
                case (int)AppMenu.Logout:
                    LogOutProgress();
                    Utility.PrintMessage("You have sucessfully log out. Please collect your ATM card", true);
                    Run();
                    break;
                default:
                    UI.Utility.PrintMessage("Invalid option", false);
                    break;
            }
        }

        internal static void LogOutProgress()
        {
            Console.WriteLine("Thank you for using our ATM App. Logging out...");
            UI.Utility.PrintDotAnimation();
            Console.Clear();
        }

        public void CheckBalance()
        {
           UI.Utility.PrintMessage($"Your account balance is {Utility.FormatAmount(selectedAccount.AccountBalance)}", true);
        }

        public void PlaceDeposit()
        {
            Console.WriteLine("\nonly multiples of 500 , 1000 and 5000 pakistan rupees are allowed");
            var transaction_amt = UI.Validator.Convert<int>($"amount {UI.AppScreen.currency}to deposit:");

            // simulate counting
            Console.WriteLine("\nChecking and Counting bank notes...");
            UI.Utility.PrintDotAnimation();
            Console.WriteLine("\nDepositing cash...");

            // some guard clause
            if(transaction_amt <=0)
            {
                UI.Utility.PrintMessage("Invalid amount. Please enter multiples of 500, 1000 and 5000", false);
                return;
            }

            if (transaction_amt % 500 != 0)
            {
                UI.Utility.PrintMessage("Invalid amount. Please enter multiples of 500, 1000 and 5000", false);
                return;
            }

            if (!PreviewBankNotesCount(transaction_amt))
            {
                UI.Utility.PrintMessage("Transaction cancelled", false);
                return;
            }

            // bind transaction details to the transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Deposit, transaction_amt, "Deposit cash");

            // update account balance
            selectedAccount.AccountBalance += transaction_amt;

            // print success message
            UI.Utility.PrintMessage($"You have successfully deposited {UI.Utility.FormatAmount(transaction_amt)}", true);
        }

        public void MakeWithDrawal()
        {
            var transaction_amt = 0;
            int selectedAmount = UI.AppScreen.SelectAmount();
            if (selectedAmount == -1)
            {
              MakeWithDrawal();
              return;
            }
            else if(selectedAmount != 0)
            {
                transaction_amt = selectedAmount;
            }
            else
            {
                transaction_amt = UI.Validator.Convert<int>($"amount {UI.AppScreen.currency}to withdraw:");
            }

            // input validation
            if (transaction_amt <= 0)
            {
                UI.Utility.PrintMessage("Amount needs to be greater than zero. Try again", false);
                return;
            }

            if(transaction_amt % 500 != 0)
            {
                UI.Utility.PrintMessage("Invalid amount. Please enter multiples of 500, 1000 and 5000 PKR. Try again", false);
                return;
            }

            // Business logic validation
            if(transaction_amt > selectedAccount.AccountBalance)
            {
                UI.Utility.PrintMessage($"Withdrawal failed, Your balance is To low to wothdrawal"+
                 $"{UI.Utility.FormatAmount(transaction_amt)}", false);
                return;
            }

            if(selectedAccount.AccountBalance - transaction_amt < minimumKeptAmount)
            {
                UI.Utility.PrintMessage($"Withdrawal failed, Your balance is To low to wothdrawal"+
                 $"{UI.Utility.FormatAmount(transaction_amt)}", false);
                return;
            }

            // Bind withdrawal transaction details to the transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Withdrawal, transaction_amt, "Withdrawal cash");

            // Update account balance
            selectedAccount.AccountBalance -= transaction_amt;

            // Print success message
            UI.Utility.PrintMessage($"You have successfully withdrawn {UI.Utility.FormatAmount(transaction_amt)}", true);

        }

        private bool PreviewBankNotesCount(int amount)
        {
            int fiveThousandNotesCount = amount / 5000;
            int thousandNotesCount = amount / 1000;
            int fiveHundredNotesCount = (amount % 1000) / 500;

            Console.WriteLine("\nSummary:");
            Console.WriteLine("-----------");
            Console.WriteLine($"{UI.AppScreen.currency}5000 X {fiveThousandNotesCount} = {5000 * fiveThousandNotesCount}");
            Console.WriteLine($"{UI.AppScreen.currency}1000 X {thousandNotesCount} = {1000 * thousandNotesCount}");
            Console.WriteLine($"{UI.AppScreen.currency}500 X {fiveHundredNotesCount} = {500 * fiveHundredNotesCount}");
            Console.WriteLine($"Total amount: {UI.Utility.FormatAmount(amount)}\n\n");

            int option = UI.Validator.Convert<int>("1. Confirm 2. Cancel");
            return option.Equals(1);
        }

        public void InsertTransaction(long _UserBankAccountId, TransactionType _TransactionType, decimal _tranAmount, string _desc)
        {
            var transaction = new Transaction
            {
                TransactionId = UI.Utility.GetTransactionId(),
                UserBankAccountId = _UserBankAccountId,
                TransactionDate = DateTime.Now,
                TransactionType = _TransactionType,
                TransactionAmount = _tranAmount,
                Descriprion = _desc
            };
            _listOfTransactions.Add(transaction);
        }
        

        public void ViewTransaction()
        {
            throw new NotImplementedException();
        }

     }

}

