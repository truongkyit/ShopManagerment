using LiveCharts;
using LiveCharts.Wpf;
using MyShop.Business;
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
    /// Interaction logic for UCProductTopOfSale.xaml
    /// </summary>
    public partial class UCProductTopOfSale : UserControl
    {
        private ProductBusiness _productBus = new ProductBusiness();

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public SeriesCollection SeriesCollection1 { get; set; }
        public string[] Labels1 { get; set; }
        public Func<double, string> Formatter1 { get; set; }

        public ReportProductQuery param { get; set; }
        public UCProductTopOfSale()
        {
            InitializeComponent();

            Loaded += UCProductTopOfSale_Loaded;
            this.GeneralReport.Click += GeneralReport_Click;
        }

        private void GeneralReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Thành công!");
            var product = _productBus.getProdctTopSaleWithAmount(param);

            SeriesCollection = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "Tổng giá trị sản phẩm bán được (đơn vị tiền tệ  VND)",
                    Values = new ChartValues<double> {}
                }
            };

            SeriesCollection.Add(new RowSeries
            {
                Title = "Số lượng sản phẩm bán được",
                Values = new ChartValues<double> { }
            });

            List<string> LabelTemp = new List<string>();

            foreach (var item in product)
            {
                LabelTemp.Add(item.Products.Name);
                SeriesCollection[0].Values.Add(item.Sum);
                SeriesCollection[1].Values.Add((double)item.totalQuantity);
            }


            Labels = LabelTemp.ToArray();

            Formatter = value => value.ToString("N");

            var productQuantity = _productBus.getProdctTopSaleWithQuantity(param);

            SeriesCollection1 = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "Tổng số lượng sản phẩm bán được",
                    Values = new ChartValues<int> {}
                }
            };

            LabelTemp.Clear();

            foreach (var item in productQuantity)
            {
                LabelTemp.Add(item.Products.Name);
                SeriesCollection1[0].Values.Add(item.totalQuantity);
            }


            Labels1 = LabelTemp.ToArray();

            Formatter1 = value => value.ToString("N");
            ChartAmount.Series = SeriesCollection;
            FormatterAmount.LabelFormatter = Formatter;
            LabelSaleAmount.Labels = Labels;

            ChartQuantity.Series = SeriesCollection1;
            FormatterQuantity.LabelFormatter = Formatter1;
            LabelSaleQuantity.Labels = Labels1;


        }

        private void UCProductTopOfSale_Loaded(object sender, RoutedEventArgs e)
        {
            param = new ReportProductQuery();
            this.reportOption.DataContext = param;
            DataContext = this;
        }
    }
}
