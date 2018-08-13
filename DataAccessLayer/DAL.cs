using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DAL
    {
        static ProtonEntities DbContext;

        static DAL()
        {
            DbContext = new ProtonEntities();
        }

        public static List<Product> GetAllProducts()
        {
            return DbContext.Products.ToList();
        }

        public static List<Product> GetFilteredProducts(string searchString)
        {
            return DbContext.Products.Where(p => p.Name.Contains(searchString)).ToList();
        }

        public static Product GetProduct(string Code)
        {
            return DbContext.Products.Where(p => p.Code.Equals(Code)).FirstOrDefault();
        }
        public static bool InsertProduct(Product productItem)
        {
            bool status;
            try
            {
                productItem.LastUpdated = DateTime.Now;
                if (productItem.Photo == null)
                     productItem.Photo = "sample.png";
                DbContext.Products.Add(productItem);
                DbContext.SaveChanges();
                status = true;
            }
            catch (UpdateException ex)
            {
                DbContext.Products.Remove(productItem);
                status = false;
            }
            catch (DbEntityValidationException ex)
            {
                DbContext.Products.Remove(productItem);
                status = false;
            }
            catch (SqlException ex)
            {
                status = false;
            }

            return status;
        }

        public static bool UpdateProduct(Product productItem)
        {
            bool status;
            try
            {
                Product prodItem = DbContext.Products.Where(p => p.Id == productItem.Id).FirstOrDefault();
                if (prodItem != null)
                {
                    prodItem.Code = productItem.Code;
                    prodItem.Name = productItem.Name;

                    if(productItem.Photo != null)
                       prodItem.Photo = productItem.Photo;

                    prodItem.Price = productItem.Price;
                    prodItem.LastUpdated = DateTime.Now;
                    DbContext.SaveChanges();
                }
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        public static bool DeleteProduct(string Code)
        {
            bool status;
            try
            {
                Product prodItem = DbContext.Products.Where(p => p.Code.Equals(Code)).FirstOrDefault();
                if (prodItem != null)
                {
                    DbContext.Products.Remove(prodItem);
                    DbContext.SaveChanges();
                }
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
    }
}

