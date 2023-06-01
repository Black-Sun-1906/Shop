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
    public class ProvidersController : ControllerBase
    {
        private EntityGateway _db = new();
        private Guid Token => Guid.Parse(Request.Headers["Token"] != string.Empty ?
            Request.Headers["Token"]! : Guid.Empty.ToString());

        [HttpGet]
        public IActionResult GetAll() =>

            Ok(new
            {
                status = "ok",
                product = _db.GetProviders()
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
            var potentialTitle = _db.GetProviders(x => x.Id == id).FirstOrDefault();
            if (potentialTitle is not null)
                return Ok(new
                {
                    status = "ok",
                    provider = potentialTitle
                });
            else
                return NotFound(new
                {
                    status = "fail",
                    message = $"There is no product with {id} id"
                });
        }

        /// <summary>
        /// Search for a product provider by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/products")]
        public IActionResult GetProvidersProducts(Guid id)
        {
            var potentialProvider = _db.GetProviders(x => x.Id == id).FirstOrDefault();
            return potentialProvider is null ?
                NotFound(new
                {
                    status = "fail",
                    message = $"There is no provider with {id} id"
                }) :
            Ok(new
            {
                status = "ok",
                products = potentialProvider.Products
            });
        }

        [HttpPost]
        public IActionResult PostProvider([FromBody] JObject providerJson)
        {
            try
            {
                Provider provider = providerJson.ToObject<Provider>();
                if (LocalAuthService.GetInstance().IsManager(Token))
                    return Unauthorized(new
                    {
                        status = "fail",
                        message = "Session is not valid"
                    });                
                _db.AddOrUpdate(provider);
                return Ok(new
                {
                    status = "ok",
                    id = provider.Id
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