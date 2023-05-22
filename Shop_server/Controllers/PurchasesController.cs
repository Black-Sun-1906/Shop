using Microsoft.AspNetCore.Mvc;
using Shop_dblayer;
using Shop_server.Servises;
using System;
using System.Linq;

namespace Shop_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private EntityGateway _db = new();
        private Guid Token => Guid.Parse(Request.Headers["Token"] != string.Empty ?
            Request.Headers["Token"]! : Guid.Empty.ToString());

        [HttpGet]
        public IActionResult GetAll() =>

            Ok(new
            {
                status = "ok",
                purchases = _db.GetPurchases()
            });
        /// <summary>
        /// Get purchase name by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(Guid id)
        {
            var potentialTitle = _db.GetPurchases(x => x.Id == id).FirstOrDefault();
            if (potentialTitle is not null)
                return Ok(new
                {
                    status = "ok",
                    purchas = potentialTitle
                });
            else
                return NotFound(new
                {
                    status = "fail",
                    message = $"There is no product with {id} id"
                });
        }

        /// <summary>
        /// Client ahd product search by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/products")]
        public IActionResult GetPurchasesProducts(Guid id)
        {
            var potentialPurchas = _db.GetPurchases(x => x.Id == id).FirstOrDefault();
            return potentialPurchas is null ?
                NotFound(new
                {
                    status = "fail",
                    message = $"There is no product with {id} id"
                }) :
            Ok(new
            {
                status = "ok",
                product = potentialPurchas.Products
            });
        }
        /*
       [HttpPost]
        public IActionResult PostPurchases([FromBody] Purchases purchase)
        {
            if (!LocalAuthService.GetInstance().IsManager(Token))
                return Unauthorized(new
                {
                    status = "fail",
                    message = "You saved this product."
                });
            _db.AddOrUpdate(purchase);
            return Ok(new
            {
                status = "ok",
                id = purchase.Id
            });
        
        }
        */
    }
}