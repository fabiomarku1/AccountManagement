using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountManagement.Data.Model;

namespace AccountManagement.Data
{
    public class Product
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public int CategoryId { get; set; }


        public decimal Price { get; set; }
        public byte[] Image { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; } = DateTime.Now;


    }
}
