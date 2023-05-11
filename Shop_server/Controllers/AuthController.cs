using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Shop_dblayer;
using shop_models.Models;
using Shop_server.Servises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private Guid Token => Guid.Parse(Request.Headers["Token"] != string.Empty ?
            Request.Headers["Token"]! : Guid.Empty.ToString());
        private LocalAuthService _localAuthServerce = LocalAuthService.GetInstance();
        readonly EntityGateway _db = new();

        [HttpPost]
        public IActionResult AuthPost(string login, string password)
        {
            try
            {
                var token = _localAuthServerce.Auth(login, password);
                return Ok(new
                {
                    status = "Ok",
                    token
                });

            }
            catch (Exception E)
            {
                return Unauthorized(new
                {
                    status = "fail",
                    message = E.Message
                });
                throw;
            }
        }
        [HttpPost]
        [Route("signup")]
        public IActionResult SignUp([FromBody] JObject json)
        {
            try
            {
                if (_db.GetClients(x => x.Login == json["login"]?.ToString()).Any())
                    throw new Exception("User with this login exists");
                Client potentialUser = new()
                {
                    Login = json["login"]?.ToString() ?? throw new Exception("Login is missing"),
                    Password = Extensions.ComputeSHA256(json["password"]?.ToString() ?? throw new Exception("Password is missing")),
                    Name = json["name"]?.ToString() ?? throw new Exception("Name is missing"),
                };
                _db.AddOrUpdate(potentialUser);
                return Ok(new
                {
                    status = "ok"
                });
            }
            catch (Exception E)
            {
                return BadRequest(new
                {
                    status = "fail",
                    message = E.Message
                });
            }
        }
    }
}
