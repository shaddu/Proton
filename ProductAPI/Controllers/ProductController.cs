using DataAccessLayer;
using ProductAPI.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace ProductAPI.Controllers
{
    public class ProductController : ApiController
    {
        // GET: Product
        [HttpGet]
        public JsonResult<List<Models.Product>> GetAllProducts()
        {
            EntityMapper<DataAccessLayer.Product, Models.Product> mapObj = new EntityMapper<DataAccessLayer.Product, Models.Product>();
            List<DataAccessLayer.Product> prodList = DAL.GetAllProducts();
            List<Models.Product> products = new List<Models.Product>();
            foreach (var item in prodList)
            {
                products.Add(mapObj.Translate(item));
            }
            return Json<List<Models.Product>>(products);
        }

        // GET: Product
        [HttpGet]
        public JsonResult<List<Models.Product>> GetFilteredProducts(string searchString)
        {
            EntityMapper<DataAccessLayer.Product, Models.Product> mapObj = new EntityMapper<DataAccessLayer.Product, Models.Product>();
            List<DataAccessLayer.Product> prodList = DAL.GetFilteredProducts(searchString);
            List<Models.Product> products = new List<Models.Product>();
            foreach (var item in prodList)
            {
                products.Add(mapObj.Translate(item));
            }
            return Json<List<Models.Product>>(products);
        }

        [HttpGet]
        public JsonResult<Models.Product> GetProduct(string code)
        {
            EntityMapper<DataAccessLayer.Product, Models.Product> mapObj = new EntityMapper<DataAccessLayer.Product, Models.Product>();
            DataAccessLayer.Product dalProduct = DAL.GetProduct(code);
            Models.Product products = new Models.Product();
            products = mapObj.Translate(dalProduct);
            return Json<Models.Product>(products);
        }
        [HttpPost]
        public bool InsertProduct(Models.Product product)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                EntityMapper<Models.Product, DataAccessLayer.Product> mapObj = new EntityMapper<Models.Product, DataAccessLayer.Product>();
                DataAccessLayer.Product productObj = new DataAccessLayer.Product();
                productObj = mapObj.Translate(product);
                if (productObj.Photo == null)
                    productObj.Photo = "sample.png";
                status = DAL.InsertProduct(productObj);
            }
            return status;

        }
        [HttpPut]
        public bool UpdateProduct(Models.Product product)
        {
            EntityMapper<Models.Product, DataAccessLayer.Product> mapObj = new EntityMapper<Models.Product, DataAccessLayer.Product>();
            DataAccessLayer.Product productObj = new DataAccessLayer.Product();
            productObj = mapObj.Translate(product);
            var status = DAL.UpdateProduct(productObj);
            return status;

        }
        [HttpDelete]
        public bool DeleteProduct(string code)
        {
            var status = DAL.DeleteProduct(code);
            return status;
        }

        [HttpGet]
        public bool VerifyProductCode(string code)
        {
            EntityMapper<DataAccessLayer.Product, Models.Product> mapObj = new EntityMapper<DataAccessLayer.Product, Models.Product>();
            DataAccessLayer.Product dalProduct = DAL.GetProduct(code);

            if (dalProduct != null)
                return true;
           
            return false;

        }
    }
}