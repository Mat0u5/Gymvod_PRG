using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassPlayground
{
    internal class BankAccount
    {
        public long accountNumber;
        public string holderName;
        public string currency;
        public long balance;
        public BankAccount(string holderName, string currency)
        {
            this.holderName = holderName;
            this.currency = currency;
            balance = 0;

            Random rnd = new Random();
            string accNumStr = Convert.ToString(rnd.Next(100000)) + Convert.ToString(rnd.Next(1000000));
            accountNumber = long.Parse(accNumStr);
        }
        public void writeBalance()
        {
            Console.WriteLine($"The account \"{accountNumber}\" has {balance} {currency}");
        }
        public void Deposit(long amount)
        {
            balance += amount;
        }
        public void Withdraw(long amount)
        {
            balance -= amount;
            if (balance < 0)
            {
                Console.WriteLine($"The account \"{accountNumber}\" does not have {amount} {currency}");
                balance = 0;
            }
        }

        public void Transfer(long amount, BankAccount transferTo)
        {
            if (balance < amount)
            {
                Console.WriteLine($"The account \"{accountNumber}\" does not have {amount} {currency}");
                return;
            }
            this.Withdraw(amount);
            transferTo.Deposit(amount);
        }
        public static double GetConversionRate(string fromCurrency, string toCurrency)
        {
            if (fromCurrency.Equals(toCurrency)) return 1;




            return 0;
        }
    }
}
