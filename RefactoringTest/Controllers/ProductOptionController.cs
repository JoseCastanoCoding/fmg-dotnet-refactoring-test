using refactor_me.Models;
using refactor_me.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateProductOptionByProductOptionId(Guid productId, Guid id, ProductOption productOption)
        {
            if (productOption == null)
                return BadRequest("Product option data is required");

            productOption.ProductId = productId;
            productOption.Id = id;

            if (productOption.Id != id)
                return BadRequest("Product option ID mismatch between URL and body.");

            var existingProductOption = _productOptionRepository.GetById(productId, id);
            if (existingProductOption == null)
                return NotFound();

            _productOptionRepository.Update(productOption);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("{productid}/options/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteProductOptionById(Guid productId, Guid id)
        {
            var existingProductOption = _productOptionRepository.GetById(productId, id);

            if (existingProductOption == null)
                return NotFound();

            _productOptionRepository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}