using System;
using System.Collections.Generic;
using AccountManagement.Data.Model;

namespace AccountManagement.Data.DTO
{
    public class SalesDTO
    {
        public int Id { get; set; }
        public List<ProductCheckoutDTO> ListOfProduct { get; set; }
        public int BankAccountId { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateCreated { get; set; }



    }
}
