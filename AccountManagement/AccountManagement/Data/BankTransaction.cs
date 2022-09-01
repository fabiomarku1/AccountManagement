using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManagement.Data
{
    public class BankTransaction
    {
        public int Id { get; set; }

        [ForeignKey("BankAccountId")]
        public BankAccount RequestingBankAccount { get; set; }
        public int BankAccountId { get; set; }

        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        //public int Action {get; set; }

        /*
        public enum MyEnum
        {
            Depostim,
            Terheqje ,

        }
        */
    }
}
