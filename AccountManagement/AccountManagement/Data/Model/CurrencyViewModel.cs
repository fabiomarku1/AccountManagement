using System.ComponentModel.DataAnnotations;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Data.Model
{
    public class CurrencyViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
