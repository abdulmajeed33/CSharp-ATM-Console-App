using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ATMApp.Domain.Entities
{
    public class InternalTransfer
    {
        public decimal TransferAmount { get; set; }
        public long ReceipeintBankAccountNumber { get; set; }
        public string ReceipeintBankAccountName { get; set; }
    }
}