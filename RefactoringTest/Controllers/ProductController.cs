using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;
using refactor_me.Repositories;

namespace refactor_this.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : ApiController
    {
        private readonly ProductRepository _productRepository = new ProductRepository();

        [Route]
        [HttpGet]
        public IHttpActionResult GetAllProducts(string name = null)
        {
            var products = _productRepository.GetAll(name);
            return Ok(products);
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetProductById(Guid id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [Route]
        [HttpPost]
        public IHttpActionResult CreateProduct(Product product)
        {
            if(product == null)
            {
                return BadRequest("Product data is required");
            }

            _productRepository.Add(product);
            return Created(new Uri(Request.RequestUri, $"{product.Id}"), product);
         }

        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult UpdateProduct(Guid id, Product product)
        {
            var existingProduct = _productRepository.GetById(id);
            if (existingProduct == null)
                return NotFound();

            if (product == null)
                return BadRequest("Product data is required");

            product.Id = id;

            if (id != product.Id)
                return BadRequest("Product ID mismatch between URL and body.");

            _productRepository.Update(product);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteProduct(Guid id)
        {
            var existingProduct = _productRepository.GetById(id);
            if (existingProduct == null)
                return NotFound();

            _productRepository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
