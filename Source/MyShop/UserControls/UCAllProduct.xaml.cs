using MyShop.Common;
using MyShop.Form;
using MyShop.State;
using MyShop.ValueObject.OrderVO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for UCAllProduct.xaml
    /// </summary>
    public partial class UCAllProduct : UserControl
    {
        public Product productSelected { get; set; }
        public int quantity { get; set; }

        private OrderCreateVO vo;
        public UCAllProduct(OrderCreateVO orderCreateVo)
        {
            InitializeComponent();

            Loaded += UCAllProduct_Loaded;

            btnNext.Click += BtnNext_Click;
            btnPrevius.Click += BtnPrevius_Click;
            ProductLV.SelectionChanged += ProductLV_SelectionChanged;
            ProductLV.PreviewMouseDown += ProductLV_PreviewMouseDown;

            this.btnDone.Click += BtnDone_Click;
            this.btnCancel.Click += BtnCancel_Click;
            this.tbQuantity.TextChanged += TbQuantity_TextChanged;
            this.tbQuantity.PreviewTextInput += TbQuantity_PreviewTextInput;
            tbFindProduct.TextChanged += TbFindProduct_TextChanged;

            quantity = 0;
            productSelected = null;
            vo = orderCreateVo;
        }

        private void TbFindProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
           if(tbFindProduct.Text.Length > 0)
            {
                ProductState.name = tbFindProduct.Text;
                FunctionHelper.Pagination(ActionPagination.Load, ActionProduct.FilterByName);
            }
        }

        private void TbQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void TbQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                quantity = int.Parse(tbQuantity.Text);
            }
            catch (Exception ex)
            {
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            passEv?.Invoke(productSelected, quantity, false);
        }

        public delegate void passEvent(Product productSelected, int quantity, bool isUpdate);
        public event passEvent passEv;
        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            if(quantity <= 0 || productSelected == null)
            {
                MessageBox.Show("Chọn sản phẩm và số lượng để thêm vào đơn hàng!");
                return;
            }

            var productCheck = vo.orderProductVOs.Where(item => item.ProductID == productSelected.ID).FirstOrDefault();
            if(productCheck != null)
            {
                MessageBox.Show("Sản phẩm này đã có trong giỏ hàng rùi!!");
                return;
            }
             var countQuan = (int)productSelected.Quantity;
            if (quantity > countQuan)
            {
                MessageBox.Show($"Số lượng sản phẩm còn lại là {countQuan}, bạn chỉ được phép thêm tối đa {countQuan} sản phẩm này!(Tính tổng cả số lượng hiện tại trong đơn hàng)");
                return;
            }        
            passEv?.Invoke(productSelected, quantity, true);
        }

        private void ProductLV_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
                return;
        }

        private void ProductLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductLV.SelectedIndex != -1)
            {
                var product = ProductLV.SelectedItem as Product;
                productSelected = product;
            }
        }

        private void BtnPrevius_Click(object sender, RoutedEventArgs e)
        {
            FunctionHelper.Pagination(ActionPagination.Previus, ActionProduct.GetAll);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            FunctionHelper.Pagination(ActionPagination.Next, ActionProduct.GetAll);
        }

        private void UCAllProduct_Loaded(object sender, RoutedEventArgs e)
        {
            FunctionHelper.Pagination(ActionPagination.Load, ActionProduct.GetAll);
            ProductLV.ItemsSource = ProductState.productsState;
            Pagination.DataContext = ProductState.pagination;
        }
    }
}
