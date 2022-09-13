using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManagement.Data
{
    public class BankTransaction
    {
        [Key] public int Id { get; set; }
        public virtual BankAccount BankAccount { get; set; }
       
        [ForeignKey("BankAccountId")]
        public int BankAccountId { get; set; }
        public ActionCall Action { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
    public enum ActionCall
    {
        Depositim = 1,
        Terheqje = 2,

    }


}
