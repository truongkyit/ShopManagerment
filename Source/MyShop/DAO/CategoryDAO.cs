using MyShop.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAO
{
    class CategoryDAO
    {
        private MyShopEntities conn;

        public CategoryDAO()
        {
            conn = dbConnection.getInstance();
        }

        public IQueryable<Category> getAll()
        {
            var query = conn.Categories.Where(tg => tg.DeletedAt == null);
            return (IQueryable<Category>)query;
        }

        public IQueryable<Category> getById(int id)
        {
            var query = conn.Categories.Where(con => con.ID == id && con.DeletedAt == null);
            return query;
        }

        public Category insert(Category category)
        {
            var query = conn.Categories.Add(category);
            conn.SaveChanges();
            return query;
        }

        public Category insert(Category category, List<Product> products)
        {
            var query = conn.Categories.Add(category);
            conn.SaveChanges();
            for (int i = 0; i < products.Count; i++)
            {
                products[i].CatID = query.ID;
                query.Products.Add(products[i]);
            }
            conn.SaveChanges();
            return query;
        }

        public Category insert(int id, List<Product> products)
        {
            try
            {
                var query = conn.Categories.Where(co => co.ID == id && co.DeletedAt == null).FirstOrDefault();
                for (int i = 0; i < products.Count; i++)
                {
                    products[i].CatID = query.ID;
                    query.Products.Add(products[i]);
                }
                conn.SaveChanges();
                return query;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool update(int id, Category category)
        {
            try
            {
                var query = conn.Categories.Where(co => co.ID == id).FirstOrDefault();
                query.Name = category.Name;

                conn.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Category checkNameExist(string name)
        {
            var nameSlug = FunctionHelper.ConvertToSlug(name);
            var query = conn.Categories.Where(co => co.Name_Slug.ToString().Equals(nameSlug) && co.DeletedAt == null).FirstOrDefault();
            return query;
        }

        public bool delete(Category category)
        {
            try
            {
                category.DeletedAt = DateTime.Now;
                conn.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool deleteById(int id)
        {
            try
            {
                var query = conn.Categories.Where(co => co.ID == id && co.DeletedAt == null).FirstOrDefault();
                if (query == null)
                    return false;

                query.DeletedAt = DateTime.Now;
                conn.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
