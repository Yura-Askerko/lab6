using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplication.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        [Display(Name = "Full name")]
        public string FullName { get; set; }
        public string Position { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
