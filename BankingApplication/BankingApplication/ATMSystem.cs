using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BankingApplication
{
    public class ATMSystem
    {
        private Dictionary<string, Card> cardDatabase;
        private Card card;
        public ATMSystem() 
        { 
            cardDatabase = new Dictionary<string, Card>(); 
        }

        //private Card VerifyCard(string cardNumber, string expirationDate)
        //{
        //    return cards.Find(c => c.cardDetails.cardNumber == cardNumber && c.cardDetails.expirationDate == expirationDate);
        //}


        public void runSystem()
        {
            while(true)
            {
                Console.WriteLine("choose an option: ");
                Console.WriteLine("1.Create Account");
                Console.WriteLine("2.Access Account");
                Console.WriteLine("3.EXIT");

                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        createAccount();
                        break;
                    case 2:
                        accessAccount();
                        break;
                    case 3:
                        Console.WriteLine("Exit from system");
                        return;
                    default:
                        Console.WriteLine("Invalid operation to choice! try again");
                        break;
                }
            }

        }

        private void createAccount()
        {
            Console.WriteLine("Enter your first name: ");
            string firstName = Console.ReadLine();

            Console.WriteLine("Enter your last name: ");
            string lastName = Console.ReadLine();

            Console.WriteLine("Enter your card number: ");
            string cardNumber = Console.ReadLine();

            Console.WriteLine("Enter expiration date MM/YY");
            string expirationDate = Console.ReadLine();

            Console.WriteLine("Enter card CVC: ");
            string CVC = Console.ReadLine();

            Console.WriteLine("Set your Pin Code: ");
            int pinCode = int.Parse(Console.ReadLine());

            if (cardDatabase.ContainsKey(cardNumber))
            {
                Console.WriteLine("Account with this card number already exists.");
            }
            else
            {
                var newCard = new Card(firstName, lastName, cardNumber, expirationDate, CVC , pinCode);
                cardDatabase.Add(cardNumber, newCard );
                Console.WriteLine("Account created Successfully! ");
            }

        }

        private void accessAccount()
        {
            Console.WriteLine( "enter your card number: ");
            string cardNumber = Console.ReadLine() ;    
            if(cardDatabase.ContainsKey(cardNumber))
            {
                card = cardDatabase[cardNumber];
                Console.WriteLine("enter your card PIN code ");
                int enteredPinCode = int.Parse(Console.ReadLine()) ;

                if(enteredPinCode == card.pinCode)
                {
                    Console.WriteLine($"WELCOME {card.firstName} {card.lastName}, you are in ATM system");
                    startProcessTransaction();
                }
                else
                {
                    Console.WriteLine("Incorrect PIN!");
                }
            }
            else
            {
                Console.WriteLine("Not Found Account!");
            }
        }
       
        private void startProcessTransaction()
        {
            while(true)
            {
                Console.WriteLine("Choose an action:");
                Console.WriteLine("1. Balance");
                Console.WriteLine("2. Withdrawal");
                Console.WriteLine("3. Last 5 Operations");
                Console.WriteLine("4. Deposit");
                Console.WriteLine("5. Change PIN");
                Console.WriteLine("6. Currency Conversion");
                Console.WriteLine("0. EXIT");


                int choiceForProccess = int.Parse(Console.ReadLine() );


                switch (choiceForProccess)
                {
                    case 1:
                        Console.WriteLine($"Balance: {card.balance} GEL");
                        break;
                    case 2:
                        Console.WriteLine("Enter withdrawal amount:");
                        double withdrawalAmount = double.Parse(Console.ReadLine());
                        if (withdrawalAmount > 0 && withdrawalAmount <= card.balance)
                        {
                            card.balance -= withdrawalAmount;
                            addTransaction(card, "GetAmount", withdrawalAmount);
                            Console.WriteLine($"Withdrawal successful. New balance: {card.balance} GEL");
                        }
                        else
                        {
                            Console.WriteLine("Invalid withdrawal amount or insufficient balance.");
                        }
                        break;
                    case 3:
                        displayLastFiveOperations(card);
                        break;
                    case 4:
                        Console.WriteLine("Enter deposit amount:");
                        double depositAmount = double.Parse(Console.ReadLine());
                        if (depositAmount > 0)
                        {
                            card.balance += depositAmount;
                            addTransaction(card, "DepositCheck", depositAmount);
                            Console.WriteLine($"Deposit successful. New balance: {card.balance} GEL");
                        }
                        else
                        {
                            Console.WriteLine("Invalid deposit amount.");
                        }
                        break;
                    case 5:
                        Console.WriteLine("Enter new PIN:");
                        int newPin = int.Parse(Console.ReadLine());
                        card.pinCode = newPin;
                        addTransaction(card, "ChangePin", card.balance);
                        Console.WriteLine("PIN changed successfully.");
                        break;
                    case 6:
                        Console.WriteLine("Enter amount in GEL:");
                        double amountInGEL = double.Parse(Console.ReadLine());
                        convertCurrency(card, amountInGEL);
                        break;
                    case 0:
                        card = null;
                        Console.WriteLine("Logged out successfully.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }

           
        }

        private void addTransaction(Card card, string transactionType, double amountInGEL)
        {
            var transaction = new Transaction
            {
                transactionDate = DateTime.UtcNow,
                transactionType = transactionType,
                amountGEL = amountInGEL,
                amountUSD = convertUSD(amountInGEL),
                amountEUR = convertEUR(amountInGEL)
            };
            card.transactionHistory.Insert(0, transaction);

            if (card.transactionHistory.Count > 5)
            {
                card.transactionHistory.RemoveAt(5);
            }
        }



        public void displayLastFiveOperations(Card card)
        {
            Console.WriteLine("last 5 Operations");
            foreach (var transaction in card.transactionHistory)
            {
                Console.WriteLine($"{transaction.transactionDate}");
            }
        }


        public void convertCurrency(Card card, double amountInGEL)
        {
            double usdRate = 2.62;
            double eurRate = 2.87;

            double amountInUSD = amountInGEL * usdRate;
            double amountInEUR = amountInGEL * eurRate;

            Console.WriteLine($"Converted amounts - USD: {amountInUSD}, EUR: {amountInEUR}");
        }

        private double convertUSD(double amountInGEL)
        {
            double usdRate = 2.62;
            return amountInGEL * usdRate;
        }
        private double convertEUR(double amountInGEL)
        {
            double eurRate = 2.87;
            return (amountInGEL * eurRate);
        }

    }
}
