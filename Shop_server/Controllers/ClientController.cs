using Microsoft.AspNetCore.Mvc;
using Shop_dblayer;
using System;
using System.Linq;

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

        /// <summary>
        /// Purchase search by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/purchases")]
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