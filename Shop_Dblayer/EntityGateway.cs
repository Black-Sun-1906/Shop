using Shop_Dblayer;
using shop_models;
using System;
using System.Linq;

namespace Shop_dblayer
{
    public partial class EntityGateway
    {
        internal ShopContext ShopContext { get; set; }
        public void AddOrUpdate (params IEntity[] entities)
        {
            var toAdd = entities.Where(x => x.Id == Guid.Empty);
            var toUpdate = entities.Except(toAdd);
            ShopContext.AddRange(toAdd);
            ShopContext.UpdateRange(toUpdate);
            ShopContext.SaveChanges();
        }
        public void Delete(params IEntity[] entities)
        {
            ShopContext.RemoveRange(entities);
            ShopContext.SaveChanges();
        }

    }
}
