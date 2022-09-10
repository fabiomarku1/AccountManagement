using System;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Data.Model
{
    public class ClientViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
    }
}
