using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Proton.Helper;
using Proton.Repository;

namespace Proton.Controllers
{
    public class ProductController : Controller
    {

        /// <summary>
        ///  GET: Product List
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public ActionResult GetAllProducts(string searchString)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response;

                //Check for filtering
                if (!String.IsNullOrEmpty(searchString))
                    response = serviceObj.GetResponse("api/product/getfilteredproducts?searchString=" + searchString);
                else
                    response = serviceObj.GetResponse("api/product/getallproducts");

                response.EnsureSuccessStatusCode();
                List<Models.Product> products = response.Content.ReadAsAsync<List<Models.Product>>().Result;
                ViewBag.Title = "Products";
                ViewBag.SearchString = searchString;
                return View(products);
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        //[HttpGet]  
        public ActionResult EditProduct(string code)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("api/product/GetProduct?code=" + code);
            response.EnsureSuccessStatusCode();
            Models.Product products = response.Content.ReadAsAsync<Models.Product>().Result;
            ViewBag.Title = "All Products";
            return View(products);
        }
        //[HttpPost]  
        public ActionResult Update(Models.Product product)
        {
            var imageFiles = Request.Files;
            product.Photo = ImageHelper.SaveImage(imageFiles);
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/product/UpdateProduct", product);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("GetAllProducts");
        }
        public ActionResult Details(string code)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("api/product/GetProduct?code=" + code);
            response.EnsureSuccessStatusCode();
            Models.Product products = response.Content.ReadAsAsync<Models.Product>().Result;
            ViewBag.Title = "All Products";
            return View(products);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Models.Product product)
        {
            var imageFiles = Request.Files;
            product.Photo = ImageHelper.SaveImage(imageFiles);
            product.LastUpdated = DateTime.Now;
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PostResponse("api/product/InsertProduct", product);

            response.EnsureSuccessStatusCode();
            return RedirectToAction("GetAllProducts");
        }

        public ActionResult Delete(string code)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/product/DeleteProduct?code=" + code);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("GetAllProducts");
        }

        [HttpPost]
        public JsonResult VerifyProductCode(int? Id, string code)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage responseCode = serviceObj.GetResponse("api/product/VerifyProductCode?code=" + code);
            HttpResponseMessage responseProduct = serviceObj.GetResponse("api/product/GetProduct?code=" + code);
            Models.Product product = responseProduct.Content.ReadAsAsync<Models.Product>().Result;

            if (responseCode.Content.ReadAsAsync<bool>().Result && product != null)
            {
                if (Id != product.Id)
                    return Json(false);
            }
            return Json(true);
        }

        [ValidateInput(false)]
        public FileContentResult ExportToexcel(string searchString)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response;

            if (!String.IsNullOrEmpty(searchString))
                response = serviceObj.GetResponse("api/product/getfilteredproducts?searchString=" + searchString);
            else
                response = serviceObj.GetResponse("api/product/getallproducts");

            List<Models.Product> products = response.Content.ReadAsAsync<List<Models.Product>>().Result;
            string[] columns = { "Code","Name", "Price" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(products, "Product", false, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "ProductList.xlsx");
        }
    }
}