using MyShop.Business;
using MyShop.Common;
using MyShop.State;
using MyShop.ValueObject.ProductVO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShop.UserControls
{
    /// <summary>
    /// Interaction logic for UCAdminDashboard.xaml
    /// </summary>
    public partial class UCAdminDashboard : UserControl
    {
        CategoryBusiness categoryBus = new CategoryBusiness();
        ProductBusiness productBus = new ProductBusiness();
        FileInfo imageSelect = null;
        bool actionProduct = false;
        bool isUpdate = false;

        public UCAdminDashboard()
        {
            InitializeComponent();

            Loaded += UCAdminDashboard_Loaded;
            btnPrevius.Click += BtnPrevius_Click;
            btnNext.Click += BtnNext_Click;
            cbbCategory.SelectionChanged += CbbCategory_SelectionChanged;
            ProductLV.SelectionChanged += ProductLV_SelectionChanged;

            btnUnFilter.Click += BtnUnFilter_Click;

            btnDone.Click += BtnDone_Click;
            btnCancel.Click += BtnCancel_Click;
            btnImage.Click += BtnImage_Click;
            MainWindow.evenHandler += MainWindow_evenHandler;

            tbFindProduct.TextChanged += TbFindProduct_TextChanged;
            tbPrice.PreviewTextInput += TbPrice_PreviewTextInput;
            tbQuantity.PreviewTextInput += TbQuantity_PreviewTextInput;
        }

        private void TbQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TbFindProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbFindProduct.Text.Length > 0)
            {
                ProductState.name = tbFindProduct.Text;
                if (ProductState.categoryId > -1)
                {
                    FunctionHelper.Pagination(ActionPagination.FindProduct, ActionProduct.FilterByCategoryAndName);
                }
                else
                {
                    FunctionHelper.Pagination(ActionPagination.FindProduct, ActionProduct.FilterByName);
                }
            }
            else
            {
                ProductState.name = "";
                if (ProductState.categoryId > -1)
                {
                    FunctionHelper.Pagination(ActionPagination.FindProduct, ActionProduct.FilterByCategory);
                }
                else
                {
                    FunctionHelper.Pagination(ActionPagination.FindProduct, ActionProduct.GetAll);
                }
            }
        }

        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            if (screen.ShowDialog() == true)
            {
                var str = screen.FileName;
                imageSelect = new FileInfo(str);
                productAction.passImage(str);
            }
        }

        private string PassImage()
        {
            var newName = $"{Guid.NewGuid()}{imageSelect.Extension}";
            imageSelect.CopyTo($"{AppDomain.CurrentDomain.BaseDirectory}Assert\\image\\{newName}");
            return newName;
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            string image = "";
            if (imageSelect != null)
            {
                image = PassImage();
            }
            if (isUpdate == false)
            {
                var product = new ProductCreateVO();
                product.name = tbName.Text;
                product.quantity = int.Parse(tbQuantity.Text);
                product.price = int.Parse(tbPrice.Text);
                product.catId = (cbbCategorySelect.SelectedItem as Category).ID;
                product.imagePath = imageSelect != null ? image : "";
                var validateString = product.validate();
                if (validateString.Length > 0)
                {
                    MessageBox.Show(validateString);
                    return;
                }
                productBus.insert(product);
                FunctionHelper.CallPaginationFilter(ActionPagination.Load);
            }
            else
            {
                var product = new ProductUpdateVO();
                product.name = tbName.Text;
                product.quantity = int.Parse(tbQuantity.Text);
                product.price = int.Parse(tbPrice.Text);
                product.catId = (cbbCategorySelect.SelectedItem as Category).ID;
                product.imagePath = imageSelect != null ? image : productSelected.ImagePath;
                product.SKU = productSelected.SKU;
                var validateString = product.validate();
                if (validateString.Length > 0)
                {
                    MessageBox.Show(validateString);
                    return;
                }
                productBus.update(productSelected.ID, product);
                FunctionHelper.CallPaginationFilter(ActionPagination.Load);
            }

            DisableForm();
            isUpdate = false;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DisableForm();
        }

        private void BtnUnFilter_Click(object sender, RoutedEventArgs e)
        {
            cbbCategory.SelectedIndex = -1;
            ProductState.pagination.skip = 0;
            FunctionHelper.Pagination(ActionPagination.Load, ActionProduct.GetAll);
            tbFindProduct.Text = "";

        }

        public Product productSelected;
        public ProductCreateVO productAction = new ProductCreateVO();

        private void ProductLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductLV.SelectedIndex != -1)
            {
                var product = ProductLV.SelectedItem as Product;
                productSelected = product;
                if (actionProduct == false)
                    FilterProductForm();
            }
        }


        private void CbbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbCategory.SelectedIndex == -1)
            {
                ProductState.categoryId = -1;
                return;
            }
            ProductState.categoryId = (cbbCategory.SelectedItem as Category).ID;
            FunctionHelper.Filter();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (cbbCategory.SelectedIndex == -1)
            {
                if (ProductState.name.Length > 0)
                    FunctionHelper.Pagination(ActionPagination.Next, ActionProduct.FilterByName);
                else
                    FunctionHelper.Pagination(ActionPagination.Next, ActionProduct.GetAll);

            }
            else
            {
                if (ProductState.name.Length > 0)
                    FunctionHelper.Pagination(ActionPagination.Next, ActionProduct.FilterByCategoryAndName);
                else FunctionHelper.Pagination(ActionPagination.Next, ActionProduct.FilterByCategory);
            }
        }

        private void BtnPrevius_Click(object sender, RoutedEventArgs e)
        {
            if (cbbCategory.SelectedIndex == -1)
                FunctionHelper.Pagination(ActionPagination.Previus, ActionProduct.GetAll);
            else FunctionHelper.Pagination(ActionPagination.Previus, ActionProduct.FilterByCategory);
        }

        private void UCAdminDashboard_Loaded(object sender, RoutedEventArgs e)
        {
            CategoryState.categoriesState = categoryBus.getAll();
            FunctionHelper.Pagination(ActionPagination.Load, ActionProduct.GetAll);
            ProductLV.ItemsSource = ProductState.productsState;
            cbbCategory.ItemsSource = CategoryState.categoriesState;
            cbbCategorySelect.ItemsSource = CategoryState.categoriesState;
            Pagination.DataContext = ProductState.pagination;
            formProduct.DataContext = productAction;
            DisableForm();

        }

        private void MainWindow_evenHandler(object sender, EventArgs e)
        {
            switch (sender)
            {
                case ActionProduct.Delete:
                    productBus.delete(productSelected);
                    FunctionHelper.CallPaginationFilter(ActionPagination.Delete);
                    break;
                case ActionCategory.Delete:
                    if (ProductState.categoryId <= 0)
                    {
                        MessageBox.Show("Please select category to delete");
                        return;
                    }
                    Category categorySelect = categoryBus.getById(ProductState.categoryId);
                    var t = categoryBus.deleteById(ProductState.categoryId);
                    if (t == true)
                    {
                        CategoryState.categoriesState.Remove(categorySelect);
                        cbbCategory.SelectedIndex = -1;
                        ProductState.pagination.skip = 0;
                        FunctionHelper.Pagination(ActionPagination.Load, ActionProduct.GetAll);
                    }
                    break;
                case ActionPagination.Import:
                    FunctionHelper.CallPaginationFilter(ActionPagination.Load);
                    break;
                case ActionProduct.Update:
                    isUpdate = true;
                    EnableForm(0);
                    break;
                case ActionProduct.Create:
                    EnableForm(1);
                    break;
                case ActionCategory.Update:
                    DisableForm();
                    break;
                case ActionProduct.Reload:
                    tbFindProduct.Text = "";
                    ProductState.name = "";
                    if (ProductState.categoryId != -1)
                    {
                        FunctionHelper.Filter();
                    }
                    else
                    {
                        FunctionHelper.Pagination(ActionPagination.Load, ActionProduct.Reload);
                    }
                    break;
                default:
                    break;
            }

        }

        public void ClearForm()
        {
            tbName.Clear();
            tbPrice.Clear();
            tbQuantity.Clear();
            cbbCategorySelect.SelectedIndex = -1;
            isUpdate = false;
            productAction.clear();
        }

        public void DisableForm()
        {
            ClearForm();
            tbName.IsEnabled = false;
            tbPrice.IsEnabled = false;
            tbQuantity.IsEnabled = false;
            cbbCategorySelect.IsEnabled = false;
            btnImage.IsEnabled = false;
            btnCancel.IsEnabled = false;
            btnDone.IsEnabled = false;
            imageSelect = null;
            actionProduct = false;
            ContentAction.Text = null;

        }

        //public void ClearForm()
        //{

        //}

        public void EnableForm(int action)
        {
            if (actionProduct == true)
            {
                if (MessageBox.Show("Bạn có chắc hủy thao tác hiện tại?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;
                else ClearForm();
            }
            if (action == 1)
            {
                ContentAction.Text = "Tạo sản phẩm mới";
                ClearForm();
            }
            else
            {
                if (productSelected == null)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần chỉnh sửa");
                    return;
                }
                ContentAction.Text = "Cập nhật sản phẩm";
            }

            tbName.IsEnabled = true;
            tbPrice.IsEnabled = true;
            tbQuantity.IsEnabled = true;
            cbbCategorySelect.IsEnabled = true;
            btnImage.IsEnabled = true;
            btnCancel.IsEnabled = true;
            btnDone.IsEnabled = true;

            actionProduct = true;
        }

        public void FilterProductForm()
        {
            productAction.PassValue(productSelected);
        }

    }
}
