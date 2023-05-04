using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class ClientController : ControllerBase
    {
        private EntityGateway _db = new();
        private Guid Token => Guid.Parse(Request.Headers["Token"] != string.Empty ?
            Request.Headers["Token"]! : Guid.Empty.ToString());

        [HttpGet]
        public IActionResult GetAll() =>

            Ok(new
            {
                status = "ok",
                clients = _db.GetClients()
            });
        /// <summary>
        /// Get clients by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(Guid id)
        {
            var potentialTitle = _db.GetClients(x => x.Id == id).FirstOrDefault();
            if (potentialTitle is not null)
                return Ok(new
                {
                    status = "ok",
                    clients = potentialTitle
                });
            else
                return NotFound(new
                {
                    status = "fail",
                    message = $"There is no product with {id} id"
                });
        }
        public IActionResult PostClient([FromBody] Client clients)
        {
            _db.AddOrUpdate(clients);
            return Ok(new
            {
                status = "ok",
                id = clients.Id
            });
        }
        public IActionResult GetEmployeesClient(Guid id)
        {
            var potentialClient = _db.GetClients(x => x.Id == id).FirstOrDefault();
            return potentialClient is null ?
                NotFound(new
                {
                    status = "fail",
                    message = $"There is no product with {id} id"
                }) :
            Ok(new
            {
                status = "ok",
                provider = potentialClient.Purchases
            });
        }
    }
}
