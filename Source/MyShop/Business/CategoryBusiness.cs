using MyShop.Business.Interface;
using MyShop.Common;
using MyShop.DAO;
using MyShop.State;
using MyShop.ValueObject.CategoryVO;
using MyShop.ValueObject.ProductVO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop.Business
{
    public class CategoryBusiness : ICategoryBusiness
    {
        private CategoryDAO _categoryDAO;

        public CategoryBusiness()
        {
            _categoryDAO = new CategoryDAO();
        }

        public Category checkNameExist(string name)
        {
            var t = _categoryDAO.checkNameExist(name);
            return t;
        }

        public bool delete(Category category)
        {
            return _categoryDAO.delete(category);
        }

        public bool deleteById(int id)
        {
            return _categoryDAO.deleteById(id);
        }

        public BindingList<Category> getAll()
        {
            var t = _categoryDAO.getAll();
            return new BindingList<Category>(t.ToList());
        }

        public Category getById(int id)
        {
            var t = _categoryDAO.getById(id);
            return t.FirstOrDefault();
        }

        public Category InsertData(CategoryCreateVO data)
        {
            var category = new Category()
            {
                Name = data.Name,
                CreatedAt = data.createdAt,
                UpdatedAt = data.UpdatedAt
            };
            var t = _categoryDAO.insert(category);

            return t;
        }

        public Category InsertData(CategoryCreateVO categorydata, List<ProductCreateVO> productsdata)
        {
            var category = new Category()
            {
                Name = categorydata.Name,
                Name_Slug = FunctionHelper.ConvertToSlug(categorydata.Name),
                CreatedAt = categorydata.createdAt,
                UpdatedAt = categorydata.UpdatedAt
            };

            var products = new List<Product>();
            for (int i = 0; i < productsdata.Count(); i++)
            {
                products.Add(new Product()
                {
                    Name = productsdata[i].name,
                    Name_Slug = FunctionHelper.ConvertToSlug(productsdata[i].name),
                    Price = productsdata[i].price,
                    Quantity = productsdata[i].quantity,
                    ImagePath = productsdata[i].imagePath,
                    CreatedAt = productsdata[i].createdAt,
                    UpdatedAt = productsdata[i].updatedAt,
                    SKU = productsdata[i].SKU

                });
            }
            var t = _categoryDAO.insert(category, products);
            return t;
        }

        public Category InsertData(int id, List<ProductCreateVO> productsdata)
        {
            var products = new List<Product>();
            for (int i = 0; i < productsdata.Count(); i++)
            {
                products.Add(new Product()
                {
                    Name = productsdata[i].name,
                    Name_Slug = FunctionHelper.ConvertToSlug(productsdata[i].name),
                    Price = productsdata[i].price,
                    Quantity = productsdata[i].quantity,
                    ImagePath = productsdata[i].imagePath,
                    CreatedAt = productsdata[i].createdAt,
                    UpdatedAt = productsdata[i].updatedAt,
                    SKU = productsdata[i].SKU

                });
            }

            return _categoryDAO.insert(id, products);
        }

        public bool update(int id, CategoryUpdateVO data)
        {
            var category = new Category()
            {
                UpdatedAt = data.UpdatedAt,
                Name = data.Name
            };
            category.Name_Slug = FunctionHelper.ConvertToSlug(category.Name);

            return _categoryDAO.update(id, category);
        }
    }
}
