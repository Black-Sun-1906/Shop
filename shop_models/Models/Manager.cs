using System;
using System.ComponentModel.DataAnnotations;

namespace shop_models.Models
{
    public class Manager :IAuth
    {
        public Guid Id { get; set; }
        [Required] [StringLength(100)] public string Name { get; set; }
        [Required] [StringLength(50)] public string Login { get; set; }
        [Required] public string Password { get; set; }
    }
}