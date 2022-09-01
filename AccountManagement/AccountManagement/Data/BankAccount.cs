using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManagement.Data
{
    public class BankAccount
    {
        [Key] public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

     [ForeignKey("CurrencyId")]  
     public Currency RequestingCurrency { get; set; }
     public int CurrencyId { get; set; }


     public decimal Balance { get; set; }

     [ForeignKey("ClientId")]
     public Client RequestingClient { get; set; }
     public int ClientId { get; set; }

     public bool IsActive { get; set; }

     public DateTime DateCreated { get; set; }= DateTime.Now;
     public DateTime? DateModified { get; set; } = null;

    }
}
