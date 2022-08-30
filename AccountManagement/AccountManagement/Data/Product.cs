using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManagement.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        [ForeignKey("CategoryId")]
        public Category RequestingCategory { get; set; }
        public int CategoryId { get; set; }

        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }= DateTime.Now;
        public DateTime DateModified { get; set; }

       // public <??> Image { get; set; }

    }
}
