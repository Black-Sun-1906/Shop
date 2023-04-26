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
        public IEnumerable<Product> GetProducts() =>
            GetProducts(x => true);

        public IEnumerable<Purchase> GetPurchases(Func<Purchase, bool> predicate) =>
            ShopContext.Purchases.Where(predicate).ToArray();
        public IEnumerable<Purchase> GetPurchases() =>
            GetPurchases(x => true);

        public IEnumerable<Client> GetClients(Func<Client, bool> predicate) =>
            ShopContext.Clients.Where(predicate).ToArray();
        public IEnumerable<Client> GetClients() =>
            GetClients (x => true);

        public IEnumerable<Provider> GetProviders(Func<Provider, bool> predicate) =>
            ShopContext.Providers.Where(predicate).ToArray();
        public IEnumerable<Provider> GetProviders() =>
            GetProviders(x => true);
        
    }
}