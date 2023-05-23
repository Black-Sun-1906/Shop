using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Shop_dblayer;
using shop_models.Models;
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


        /// <summary>
        /// Save purchase
        /// </summary>
        /// <param name="purchaseJson"></param>
        /// <param name="noteJson"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostPurchase([FromBody] JObject purchaseJson)
        {
            try
            {
                Purchase purchase = purchaseJson.ToObject<Purchase>();
                Client client;
                if ((client = LocalAuthService.GetInstance().GetClient(Token)) is null)
                    return Unauthorized(new
                    {
                        status = "fail",
                        message = "Session is not valid"
                    });
                purchase.Client = client;
                _db.AddOrUpdate(purchase);
                return Ok(new
                {
                    status = "ok",       
                    id = purchase.Id
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