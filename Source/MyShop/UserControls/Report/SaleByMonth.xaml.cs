using LiveCharts;
using LiveCharts.Wpf;
using MyShop.Business;
using System;
using System.Collections.Generic;
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

namespace MyShop.UserControls.Report
{
    /// <summary>
    /// Interaction logic for SaleByMonth.xaml
    /// </summary>
    public partial class SaleByMonth : UserControl
    {
        private ProductBusiness _productBus = new ProductBusiness();
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        int year = DateTime.Now.Year;

        public SaleByMonth()
        {
            InitializeComponent();
            Loaded += SaleByMonth_Loaded;

            this.GeneralReport.Click += GeneralReport_Click;
            this.tbYear.PreviewTextInput += TbYear_PreviewTextInput;
        }

        private void TbYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void GeneralReport_Click(object sender, RoutedEventArgs e)
        {
            if (tbYear.Text.Length <= 0)
            {
                MessageBox.Show("Vui lòng nhập năm!!");
                return;
            }
            this.year = int.Parse(tbYear.Text);
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = $"Tổng doanh thu của các sản phẩm theo tháng trong năm {year}",
                    Values = new ChartValues<double> {0,0,0,0,0,0,0,0,0,0,0,0 }
                }
            };

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec" };
            YFormatter = value => value.ToString("C");

            var product = _productBus.getTotalAmountProductByMonth(year);
            foreach (var item in product)
            {
                SeriesCollection[0].Values[item.Month - 1] = item.Amount;
            }

            Chart.Series = SeriesCollection;
            Formatter.LabelFormatter = YFormatter;
            Label.Labels = Labels;
            MessageBox.Show("Thành công!");
        }

        private void SaleByMonth_Loaded(object sender, RoutedEventArgs e)
        {
        }


    }
}
