using Shop_dblayer;
using shop_models;
using shop_models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_server.Servises
{
    internal class LocalAuthService
    {
        class Session
        {
            public IAuth User { get; set; }
            public DateTime LastOp { get; set; }
            public Guid Token { get; set; }
            public bool IsAction => DateTime.Now - LastOp < TimeSpan.FromHours(1);
               
        }
        private static LocalAuthService? _instance;
        public static LocalAuthService GetInstance() => _instance ??= new();
        private LocalAuthService() { }
        private HashSet<Session> Sessions { get; set; } = new();
        private readonly EntityGateway _db = new();

        public Guid Auth(string login, string password)
        {
            var passhash = Extensions.ComputeSHA256(password);
            IAuth potencialUser = _db.GetClients(x => x.Login == login & x.Password == passhash).FirstOrDefault() as IAuth ?? _db.GetManagers(x => x.Login == login & x.Password == passhash).FirstOrDefault() as IAuth ?? throw new Exception("User is not found");

            var Token = Guid.NewGuid();
            Sessions.Add(new()
            {
                LastOp = DateTime.Now,
                Token = Token,
                User = potencialUser
            });
            return Token;
        }

        public bool IsManager(Guid token)
        {
            var potentialSession = Sessions.FirstOrDefault(x => x.Token == token) ?? throw new UnauthorizedAccessException("Session is not valied.");
            potentialSession.LastOp = DateTime.Now;
            return potentialSession.User is Manager;
        }

        internal Client GetClient(Guid token)
        {
            throw new NotImplementedException();
        }
    }
}
