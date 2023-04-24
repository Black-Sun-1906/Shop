using Shop_Dblayer;
using shop_models.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop_dblayer
{
    public partial class EntityGateway
    {
        public IEnumerable<Product> GetProducts(Func<Product, bool> predicate) =>
            ShopContext.Products.Where(predicate).ToArray();
        
    }
}