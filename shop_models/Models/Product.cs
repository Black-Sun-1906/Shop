using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace shop_models.Models
{
    public class Product : IEntity
    {
        public Guid Id { get; set; }
        [Required] [StringLength(50)] public string Name { get; set; }
        [Required] [StringLength(50)] public string Description { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        public virtual Provider Provider { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
