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
        /// Get product name by id
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
        [HttpGet]
        public IActionResult GetEmployeesPurchases(Guid id)
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

            return potentialPurchas is null ?
                NotFound(new
                {
                    status = "fail",
                    message = $"There is no product with {id} id"
                }) :
            Ok(new
            {
                status = "ok",
                client = potentialPurchas.Client
            });
        }
    }
}
