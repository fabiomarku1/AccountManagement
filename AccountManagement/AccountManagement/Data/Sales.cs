using AccountManagement.Data.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManagement.Data
{
    public class Sales
    {
        [Key] public int Id { get; set; }
        public virtual BankAccount BankAccount { get; set; }
        [ForeignKey("BankAccountId")]
        public int BankAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public List<ProductCheckoutDTO> ListOfProducts { get; set; }
    }
}
