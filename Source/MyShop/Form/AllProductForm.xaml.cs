using MyShop.UserControls;
using MyShop.ValueObject.OrderVO;
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
    /// Interaction logic for AllProductForm.xaml
    /// </summary>
    public partial class AllProductForm : Fluent.IRibbonWindow
    {
        public Product productSelected { get; set; }
        public int quantity { get; set; }

        private OrderCreateVO vo;
        public AllProductForm(OrderCreateVO orderCreate)
        {
            InitializeComponent();

            Loaded += AllProductForm_Loaded;
            vo = orderCreate;
        }

        private void AllProductForm_Loaded(object sender, RoutedEventArgs e)
        {
            UCAllProduct uc = new UCAllProduct(vo);
            Uc.Content = uc;
            uc.passEv += Uc_passEv;
        }

        private void Uc_passEv(Product productSelected, int quantity, bool isUpdate)
        {
            DialogResult = isUpdate;
            this.productSelected = productSelected;
            this.quantity = quantity;
            this.Close();
        }
    }
}
