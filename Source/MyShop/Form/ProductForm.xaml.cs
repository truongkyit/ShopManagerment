using MyShop.Business;
using MyShop.Common;
using MyShop.State;
using MyShop.ValueObject.ProductVO;
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
    /// Interaction logic for ProductForm.xaml
    /// </summary>
    /// 
    public partial class ProductForm : Fluent.IRibbonWindow
    {

        public Boolean isSuccess = false;
        public ProductCreateVO product = new ProductCreateVO();
        public ProductForm()
        {
            InitializeComponent();

            Loaded += ProductForm_Loaded;
            btnDone.Click += BtnDone_Click;
            btnClose.Click += BtnClose_Click;
            cbbCategory.SelectionChanged += CbbCategory_SelectionChanged;
        }

        private void CbbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbbCategory.SelectedIndex != -1)
                   product.catId = (cbbCategory.SelectedItem as Category).ID;
        }

        private void ProductForm_Loaded(object sender, RoutedEventArgs e)
        {
            cbbCategory.ItemsSource = CategoryState.categoriesState;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            var productBus = new ProductBusiness();
            isSuccess = true;
            product.name = tbName.Text;
            product.quantity = int.Parse(tbQuantity.Text);
            product.price = int.Parse(tbPrice.Text);
            productBus.insert(product);
            FunctionHelper.Pagination(ActionPagination.Load, ActionProduct.GetAll);

            this.Close();
        }
    }
}
