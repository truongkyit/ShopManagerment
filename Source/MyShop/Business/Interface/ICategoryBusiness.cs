using MyShop.ValueObject.CategoryVO;
using MyShop.ValueObject.ProductVO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Business.Interface
{
    public interface ICategoryBusiness
    {
        BindingList<Category> getAll();
        Category getById(int id);
        Category checkNameExist(string name);
        Category InsertData(CategoryCreateVO data);
        Category InsertData(CategoryCreateVO data, List<ProductCreateVO> products);
        Category InsertData(int id, List<ProductCreateVO> products);
        bool update(int id, CategoryUpdateVO data);
        bool delete(Category category);
        bool deleteById(int id);

    }
}
