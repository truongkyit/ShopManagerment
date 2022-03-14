using MyShop.Business;
using MyShop.Common;
using MyShop.UserControls.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAO
{
    public class ProductDAO
    {
        private MyShopEntities conn;
        public ProductDAO()
        {
            conn = dbConnection.getInstance();
        }

        public IQueryable<Product> getAll()
        {
            var query = conn.Products.Where(tg => tg.DeletedAt == null && tg.Category.DeletedAt == null);
            return (IQueryable<Product>)query;
        }

        public ProductPagination getAll(int skip, int limit)
        {
            var query = conn.Products.Where(tg => tg.DeletedAt == null && tg.Category.DeletedAt == null);
            var count = query.Count();
            query = query.OrderBy(t => t.CreatedAt).Skip(skip).Take(limit);
            return new ProductPagination(count, query);
        }

        public ProductPagination getProducts(int skip, int limit, string name)
        {
            var query = conn.Products.Where(tg => tg.DeletedAt == null && tg.Category.DeletedAt == null && tg.Name_Slug.Contains(name));
            var count = query.Count();
            query = query.OrderBy(t => t.CreatedAt).Skip(skip).Take(limit);
            return new ProductPagination(count, query);
        }

        public ProductPagination getStatusQuantity(int skip, int limit, int option)
        {
            var query = conn.Products.Where(tg => tg.DeletedAt == null && tg.Category.DeletedAt == null);
            var count = query.Count();
            if (option == (int)Option.BelowQuantity)
            {
                query = query.OrderBy(t => t.Quantity);
            }
            else
            {
                query = query.OrderByDescending(t => t.Quantity);
            }
            query = query.Skip(skip).Take(limit);
            return new ProductPagination(count, query);
        }

        public List<ProductReport> getProductsTopSaleByAmount(ReportProductQuery param)
        {
            var query = conn.OrderProducts.Where(item => item.DeletedAt == null);
            if (param.filterDate)
            {
                query = query.Where(item => item.CreatedAt < param.dateEnd && item.CreatedAt > param.dateStart);
            }

            var result = query.GroupBy(item => item.ProductID)
            .Select(item => new
            {
                ID = item.Key,
                Products = item.Select(item1 => item1.Product).FirstOrDefault(),
                Sum = item.Select(item1 => item1.TotalAmount).Sum(),
                totalQuantity = item.Select(item1 => item1.Quantity).Sum()
            }).OrderBy(item => item.Sum).Skip(param.skip).Take(param.limit);

            var count = query.Count();
            List<ProductReport> list = new List<ProductReport>();
            foreach (var item in result)
            {
                list.Add(new ProductReport()
                {
                    Products = item.Products,
                    Sum = item.Sum,
                    totalQuantity = item.totalQuantity
                });
            }
            return list;
        }

        public List<ProductReport> getProductsTopSaleByQuantity(ReportProductQuery param)
        {
            var query = conn.OrderProducts.Where(item => item.DeletedAt == null);
            if (param.filterDate)
            {
                query = query.Where(item => item.CreatedAt < param.dateEnd && item.CreatedAt > param.dateStart);
            }
            var result = query.GroupBy(item => item.ProductID)
            .Select(item => new
            {
                ID = item.Key,
                Products = item.Select(item1 => item1.Product).FirstOrDefault(),
                Sum = item.Select(item1 => item1.TotalAmount).Sum(),
                totalQuantity = item.Select(item1 => item1.Quantity).Sum()
            }).OrderBy(item => item.totalQuantity).Skip(param.skip).Take(param.limit);

            var count = query.Count();
            List<ProductReport> list = new List<ProductReport>();
            foreach (var item in result)
            {
                list.Add(new ProductReport()
                {
                    Products = item.Products,
                    Sum = item.Sum,
                    totalQuantity = item.totalQuantity
                });
            }
            return list;
        }

        public List<ProductReportByMonth> getTotalAmountProductByMonth(int year)
        {
            List<ProductReportByMonth> list = new List<ProductReportByMonth>();

            var query = conn.Orders.Where(item => item.DeletedAt == null && item.CreatedAt.Year == year).GroupBy(item => item.CreatedAt.Month)
                   .Select(item => new
                   {
                       Month = item.Key,
                       Sum = item.Select(item1 => item1.TotalAmount).Sum()
                   }).OrderBy(item => item.Month);
            foreach (var item in query)
            {
                list.Add(new ProductReportByMonth()
                {
                    Month = item.Month,
                    Amount = item.Sum
                });
            }
            return list;
        }

        public ProductPagination getProducts(int skip, int limit, string name, int categoryId)
        {
            var query = conn.Products.Where(tg => tg.DeletedAt == null && tg.Category.DeletedAt == null && tg.Name_Slug.Contains(name) && tg.Category.DeletedAt == null && tg.CatID == categoryId);
            var count = query.Count();
            query = query.OrderBy(t => t.CreatedAt).Skip(skip).Take(limit);
            return new ProductPagination(count, query);
        }

        public Product checkNameExist(string name)
        {
            var nameSlug = FunctionHelper.ConvertToSlug(name);
            var query = conn.Products.Where(co => co.Name_Slug.Equals(nameSlug) && co.DeletedAt == null && co.Category.DeletedAt == null).FirstOrDefault();
            return query;
        }

        public ProductPagination getProductsByCategoryId(int skip, int limit, int id)
        {
            var query = conn.Products.Where(tg => tg.DeletedAt == null && tg.CatID == id && tg.Category.DeletedAt == null);
            var count = query.Count();
            query = query.OrderBy(t => t.CreatedAt).Skip(skip).Take(limit);

            return new ProductPagination(count, query);
        }

        public Product insert(Product product)
        {
            var query = conn.Products.Add(product);
            conn.SaveChanges();
            return query;
        }

        public bool update(int id, Product product)
        {
            try
            {
                var query = conn.Products.Where(co => co.ID == id).FirstOrDefault();
                query.Name = product.Name;
                query.ImagePath = product.ImagePath;
                query.Price = product.Price;
                query.Quantity = product.Quantity;
                query.CatID = product.CatID;

                conn.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool updateQuantityById(int id, int quantity)
        {
            var product = conn.Products.Where(item => item.ID == id).FirstOrDefault();
            if (product != null)
            {
                product.Quantity = product.Quantity - quantity;
                conn.SaveChanges();
            }
            return true;
        }

        public bool delete(Product product)
        {
            product.DeletedAt = DateTime.Now;
            conn.SaveChanges();
            return true;
        }
    }

    public class ProductReport
    {
        public Product Products;
        public double Sum { get; set; }
        public int totalQuantity { get; set; }

        public ProductReport(Product products, double totalAmount, int totalQuantity)
        {
            this.Products = products;
            this.Sum = totalAmount;
            this.totalQuantity = totalQuantity;
        }

        public ProductReport()
        {
        }
    }

    public class ProductReportByMonth
    {
        public int Month { get; set; }
        public double Amount { get; set; }

        public ProductReportByMonth() { }
    }

    public class ProductPagination
    {
        public int count;
        public IQueryable<Product> products;

        public ProductPagination(int count, IQueryable<Product> products)
        {
            this.count = count;
            this.products = products;
        }
    }
}
