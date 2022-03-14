using LiveCharts;
using LiveCharts.Wpf;
using MyShop.Business;
using MyShop.Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShop.UserControls.Report
{
    /// <summary>
    /// Interaction logic for UCProductNearlySoldOut.xaml
    /// </summary>
    public partial class UCProductNearlySoldOut : UserControl
    {
        private ProductBusiness _productBus = new ProductBusiness();

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        List<OptionProduct> options = new List<OptionProduct>();
        public UCProductNearlySoldOut()
        {
            InitializeComponent();

            Loaded += UCProductNearlySoldOut_Loaded;
            this.btnGeneralReport.Click += BtnGeneralReport_Click;
        }

        private void BtnGeneralReport_Click(object sender, RoutedEventArgs e)
        {
            ProductPaginationBus product;
            product = _productBus.getStatusQuantity(0, 10, (cbbOption.SelectedValue as OptionProduct).optionValue);
            
            SeriesCollection = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "Số lượng sản phẩm còn lại",
                    Values = new ChartValues<double> {}
                }
            };

            List<string> LabelTemp = new List<string>();

            foreach (var item in product.products)
            {
                LabelTemp.Add(item.Name);
                SeriesCollection[0].Values.Add(double.Parse(item.Quantity.ToString()));
            }

            Labels = LabelTemp.ToArray();

            Formatter = value => value.ToString("N");
            Chart.Series = SeriesCollection;
            Label.Labels = Labels;
            Formater.LabelFormatter = Formatter;
            MessageBox.Show("Thành công!");
        }

        private void UCProductNearlySoldOut_Loaded(object sender, RoutedEventArgs e)
        {
            options.Add(new OptionProduct() { description = "Sắp hết hàng", optionValue = (int)Option.BelowQuantity });
            options.Add(new OptionProduct() { description = "Số lượng nhiều", optionValue = (int)Option.TopQuantity });
            cbbOption.ItemsSource = options;
        }
    }

   

    public class OptionProduct
    {
        public string description { get; set; }
        public int optionValue { get; set; }
    }
}
