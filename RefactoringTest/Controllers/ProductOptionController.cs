using refactor_me.Models;
using refactor_me.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Web.Http;


namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductOptionController : ApiController
    {
        private readonly ProductOptionRepository _productOptionRepository = new ProductOptionRepository();

        [Route("{id}/options")]
        [HttpGet]
        public IHttpActionResult GetAllProductOptionsByProductId(Guid id)
        {
            var productOptions = _productOptionRepository.GetAll(id);
            if (productOptions == null || !productOptions.Any())
                return NotFound();
            return Ok(productOptions);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public IHttpActionResult GetProductOptionByProductId(Guid productId, Guid id)
        {
            var productionOptionId = _productOptionRepository.GetById(productId, id);
            if (productionOptionId == null)
                return NotFound();
            return Ok(productionOptionId);
        }

        [Route("{productId}/options")]
        [HttpPost]
        public IHttpActionResult AddProductOptionByProductId(Guid productId, ProductOption productOption)
        {
            if (productOption == null)
                return BadRequest("Product option data is required");

            productOption.ProductId = productId;
            _productOptionRepository.Add(productOption);

            return Created(new Uri(Request.RequestUri, $"{productOption.Id}"), productOption);

        }
    }
}