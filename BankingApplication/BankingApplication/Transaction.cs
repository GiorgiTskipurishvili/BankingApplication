using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApplication
{
    public class Transaction
    {
        public DateTime transactionDate {  get; set; }
        public string transactionType {  get; set; }
        public double amountGEL {  get; set; }
        public double amountUSD {  get; set; }
        public double amountEUR { get; set; }


    }
}
