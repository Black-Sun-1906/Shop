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
    public class PductsController : ControllerBase
    {
        private EntityGateway _db = new();
        private Guid Token => Guid.Parse(Request.Headers["Token"] != string.Empty ?
            Request.Headers["Token"]! : Guid.Empty.ToString());

        [HttpGet]
        public IActionResult GetAll() =>

            Ok(new
            {
                status = "ok",
                product = _db.GetProducts()
            });
        /// <summary>
        /// Get product name by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(Guid id)
        {
            var potentialTitle = _db.GetProducts(x => x.Id == id).FirstOrDefault();
            if (potentialTitle is not null)
                return Ok(new
                {
                    status = "ok",
                    product = potentialTitle
                });
            else
                return NotFound(new
                {
                    status = "fail",
                    message = $"There is no product with {id} id"
                });
        }


        [HttpPost]
        public IActionResult PostProduct([FromBody] Product product)
        {
            if (!LocalAuthService.GetInstance().IsManager(Token))
                return Unauthorized(new
                {
                    status = "fail",
                    message = "You have no rights for that action."
                });
            _db.AddOrUpdate(product);
            return Ok(new
            {
                status = "ok",
                id = product.Id
            });
        }
    }
}
