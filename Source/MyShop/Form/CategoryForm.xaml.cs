using MyShop.Business;
using MyShop.Common;
using MyShop.State;
using MyShop.ValueObject.CategoryVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyShop.Form
{
    /// <summary>
    /// Interaction logic for CategoryForm.xaml
    /// </summary>
    public partial class CategoryForm : Fluent.IRibbonWindow
    {
        private bool isCreate = false;
        public bool isSuccess = false;
        string name = "";
        CategoryBusiness categoryBus = new CategoryBusiness();
        Category category;
        public CategoryForm(bool action)
        {
            InitializeComponent();

            Loaded += CategoryForm_Loaded;

            this.btnSave.Click += BtnSave_Click;
            this.btnExit.Click += BtnExit_Click;
            this.tbCategoryName.TextChanged += TbCategoryName_TextChanged;
            isCreate = action;
        }

        private void TbCategoryName_TextChanged(object sender, TextChangedEventArgs e)
        {
            name = this.tbCategoryName.Text;
        }

        private void CategoryForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (isCreate == false)
            {
                lbCategoryName.Content = "Cập nhật tên loại sản phẩm";
                category = CategoryState.categoriesState.Where(t => t.ID == ProductState.categoryId).SingleOrDefault();
                tbCategoryName.Text = category.Name;
            }
            else
            {
                lbCategoryName.Content = "Tạo mới tên loại sản phẩm";
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    if (isCreate == true)
                    {
                        var categoryVo = new CategoryCreateVO()
                        {
                            Name = name
                        };

                        var validateString = categoryVo.validate();
                        if (validateString.Length > 0)
                        {
                            MessageBox.Show(validateString);
                            return;
                        }
                        var g = categoryBus.InsertData(categoryVo);

                        CategoryState.categoriesState.Add(g);
                    }
                    else
                    {
                        var categoryVo = new CategoryUpdateVO()
                        {
                            Name = name
                        };

                        var validateString = categoryVo.validate();
                        if (validateString.Length > 0)
                        {
                            MessageBox.Show(validateString);
                            return;
                        }

                        int id = ProductState.categoryId;
                        CategoryState.categoriesState.Remove(category);
                        categoryBus.update(id, categoryVo);
                        var categoryGet = categoryBus.getById(id);
                        CategoryState.categoriesState.Add(categoryGet);
                        isSuccess = true;
                    }
                });
            };
            FunctionHelper.LoadingEvent(action);
            this.Close();
        }
    }
}
