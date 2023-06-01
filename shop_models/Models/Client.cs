using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace shop_models.Models
{
    public class Client : IAuth
    {
        public Guid Id { get; set; }
        [Required] [StringLength(50)] public string Name { get; set; }
        [Required] [StringLength(100)] public string Login { get; set; }
        [Required] public string Password { get; set; }

        [JsonIgnore]public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
