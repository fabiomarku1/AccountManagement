using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using AutoMapper.Configuration.Annotations;

namespace AccountManagement.Data.DTO
{
    public class BankTransactionGetDto
    {
        public int Id { get; set; }
        public ActionCall Action { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public int BankAccountId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
