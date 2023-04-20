using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using shop_models;
using shop_models.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shop_Dblayer
{
    internal class ShopContext: DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!File.Exists("db_setting.json"))
                throw new FileNotFoundException("Config file is not found!");
            var json = File.ReadAllText("db_setting.json");
            var jObj = JObject.Parse(json);
            var connectionString = jObj["connectionString"]?.ToString() ??
                throw new KeyNotFoundException("connectionString if missing");

            optionsBuilder
                .UseSqlServer(connectionString)
                .UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var type in typeof(IEntity)
                .Assembly
                .GetTypes()
                .Where(x => typeof(IEntity).IsAssignableFrom(x) && x.IsClass))
                modelBuilder.Entity(type)
                    .Property(nameof(IEntity.Id))
                    .HasDefaultValueSql("NEWSEQUENTIALID()");

        }
    }
}
