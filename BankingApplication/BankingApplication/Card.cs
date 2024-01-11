using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BankingApplication
{
    public class Card
    {
        public string firstName {  get; set; }
        public string lastName {  get; set; }
        public CardDetails cardDetails;
        public int pinCode { get; set; }
        public double balance {  get; set; }
        public List<Transaction> transactionHistory { get; set; }

        public Card(string firstName, string lastName, string cardNumber, string expirationDate, string CVC, int pinCode)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.cardDetails = new CardDetails { cardNumber = cardNumber, expirationDate = expirationDate, CVC = CVC };
            this.pinCode = pinCode;
            this.balance = 0;
            this.transactionHistory = new List<Transaction>();
        }


    }
}
