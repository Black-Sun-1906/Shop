using System;

namespace shop_models
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }

    public interface IAuth : IEntity
    {
        string Login { get; set; }
        string Password { get; set; }
    }
}
