using System;
using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Data
{
    public class Category
    {
     [Key]  public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }=DateTime.Now;
        public DateTime? DateModified { get; set; } = null;
    }
}
