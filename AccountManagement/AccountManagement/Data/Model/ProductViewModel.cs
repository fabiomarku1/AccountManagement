﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace AccountManagement.Data.Model
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        //public byte[] Image { get; set; }

    }
}
