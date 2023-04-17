using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace shop_models.Models
{
    public class Provider
    {
        public Guid Id { get; set; }
        [Required] [StringLength(50)] public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
