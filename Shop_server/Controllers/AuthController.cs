using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            catch(Exception E)
            {
                return Unauthorized(new
                {
                    status = "fail",
                    message = E.Message
                });
                throw;
            }
        }
    }
}
