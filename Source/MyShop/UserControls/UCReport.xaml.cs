using Aspose.Cells.Charts;
using LiveCharts;
using LiveCharts.Wpf;
using MyShop.UserControls.Report;
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

namespace MyShop.UserControls
{
    /// <summary>
    /// Interaction logic for UCReport.xaml
    /// </summary>
    public partial class UCReport : UserControl
    {
        List<ReportObject> listRp = new List<ReportObject>();
        UCProductNearlySoldOut uc = new UCProductNearlySoldOut();
        UCProductTopOfSale ucTOS = new UCProductTopOfSale();
        SaleByMonth ucM = new SaleByMonth();
        public UCReport()
        {
            InitializeComponent();

            Loaded += UCReport_Loaded;

            btnGeneralReport.Click += BtnGeneralReport_Click;

            listRp.Add(new ReportObject() { Value = ReportType.NearlyOutOfSale, Description = "Tình trạng sản phẩm" });
            listRp.Add(new ReportObject() { Value = ReportType.TopOfSaleAmount, Description = "Top sản phẩm có doanh thu cao nhất" });
            listRp.Add(new ReportObject() { Value = ReportType.SaleByMonth, Description = "Báo cáo tình hình doanh thu theo tháng trong năm" });
        }

        private void BtnGeneralReport_Click(object sender, RoutedEventArgs e)
        {
            if (cbbReport.SelectedIndex > -1)
            {
                var value = (cbbReport.SelectedValue as ReportObject).Value;
                switch (value)
                {
                    case ReportType.NearlyOutOfSale:
                        MainRp.Content = uc;
                        break;
                    case ReportType.TopOfSaleAmount:
                        MainRp.Content = ucTOS;
                        break;
                    case ReportType.SaleByMonth:
                        MainRp.Content = ucM;
                        break;
                    default: break;
                }
            }
        }

        private void UCReport_Loaded(object sender, RoutedEventArgs e)
        {


            cbbReport.ItemsSource = listRp;
        }
    }

    public enum ReportType
    {
        NearlyOutOfSale = 1,
        TopOfSaleAmount = 2,
        SaleByMonth = 3
    };

    public class ReportObject
    {
        public ReportType Value { get; set; }
        public string Description { get; set; }

    }
}
