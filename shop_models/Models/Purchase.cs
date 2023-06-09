﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace shop_models.Models
{
    public class Purchase : IEntity
    {
        public Guid Id { get; set; }
        [Required] [StringLength(50)] public string Name { get; set; }
        public DateTime Date_Receiving { get; set; }
        public DateTime Date_Sending { get; set; }
        public DateTime Purchases { get; set; }

        public virtual Client Client { get; set; }
        [JsonIgnore]        public virtual ICollection<Product> Products { get; set; }
    }
}
