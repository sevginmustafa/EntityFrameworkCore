﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P03_SalesDatabase.Data.Models
{
    public class Customer
    {
        public Customer()
        {
            Sales = new HashSet<Sale>();
        }

        public int CustomerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [DataType("varchar(80)")]
        public string Email { get; set; }

        [Required]
        public string CreditCardNumber { get; set; }


        public ICollection<Sale> Sales { get; set; }
    }
}
