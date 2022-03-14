using Aspose.Cells;
using MyShop.Business;
using MyShop.UserControls;
using MyShop.ValueObject.CategoryVO;
using MyShop.ValueObject.ProductVO;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyShop.Common;
using MyShop.DAO;
using MyShop.Form;
using MyShop.State;

namespace MyShop
{

    public partial class MainWindow : Fluent.IRibbonWindow
    {
        ProductBusiness productBus = new ProductBusiness();
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            //Event Group Import
            btnDataImport.Click += BtnDataImport_Click;

            //Event Group Category
            btnCategoryAdd.Click += BtnCategoryAdd_Click;
            btnCategoryEdit.Click += BtnCategoryEdit_Click;
            btnCategoryDelete.Click += BtnCategoryDelete_Click;

            //Event Group Product
            btnProductAdd.Click += BtnProductAdd_Click;
            btnProductDelete.Click += BtnProductDelete_Click;
            btnProductEdit.Click += BtnProductEdit_Click;

            btnOrderCreate.Click += BtnOrderCreate_Click;
            btnOrderUpdate.Click += BtnOrderUpdate_Click;
            btnOrderDelete.Click += BtnOrderDelete_Click;



        }

        private void BtnOrderDelete_Click(object sender, RoutedEventArgs e)
        {
            evenHandler?.Invoke(ActionOrder.Delete, null);
        }

        private void BtnOrderUpdate_Click(object sender, RoutedEventArgs e)
        {
            evenHandler?.Invoke(ActionOrder.Update, null);
        }

        private void BtnOrderCreate_Click(object sender, RoutedEventArgs e)
        {
            evenHandler?.Invoke(ActionOrder.Create, null);
        }

        private void BtnCategoryEdit_Click(object sender, RoutedEventArgs e)
        {
            if(ProductState.categoryId == -1)
            {
                MessageBox.Show("Vui lòng chọn danh mục để chỉnh sửa!");
                return;
            }
            CategoryForm frmCategory = new CategoryForm(false);
            this.Hide();
            frmCategory.ShowDialog();
            if(frmCategory.isSuccess == true)
            {
                evenHandler?.Invoke(ActionCategory.Update, null);

            }
            this.Show();
        }

        private void BtnProductEdit_Click(object sender, RoutedEventArgs e)
        {
            evenHandler?.Invoke(ActionProduct.Update, null);
        }

        public static event EventHandler evenHandler;

        private void BtnCategoryDelete_Click(object sender, RoutedEventArgs e)
        {
            evenHandler?.Invoke(ActionCategory.Delete, null);
        }

        private void BtnProductDelete_Click(object sender, RoutedEventArgs e)
        {
            evenHandler?.Invoke(ActionProduct.Delete, null);

        }

        private void BtnProductAdd_Click(object sender, RoutedEventArgs e)
        {
            evenHandler?.Invoke(ActionProduct.Create, null);
        }

        private void BtnCategoryAdd_Click(object sender, RoutedEventArgs e)
        {
            CategoryForm frmCategory = new CategoryForm(true);
            this.Hide();
            frmCategory.ShowDialog();
            this.Show();
        }

        private void BtnDataImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Exel files (*.xlsx)|*.xlsx";
            openFileDialog.DefaultExt = ".xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                bool check = false;
                Action myaction = () =>
                {
                    check = FunctionHelper.ImportFile(openFileDialog.FileName);

                };
                this.Hide();
                FunctionHelper.LoadingEvent(myaction);
                this.Show();
                if (check)
                {
                    MessageBox.Show("Thêm dữ liệu thành công!");
                }
                else
                {
                    MessageBox.Show("Có một số trường hợp thêm thất bại vui lòng kiểm tra lại dữ liệu!");
                }
                    evenHandler?.Invoke(ActionPagination.Import, null);

            }
        }

        private CategoryBusiness _categoryBusiness = new CategoryBusiness();

        UCAdminDashboard adminDashboard = new UCAdminDashboard();
        UCSaleDashboard saleDashboard = new UCSaleDashboard();
        UCReport report = new UCReport();
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            this.Hide();
            FunctionHelper.LoadingEvent(Simulator);
            var screens = new ObservableCollection<TabItem>()
                    {
                    new TabItem() { Content = adminDashboard},
                    new TabItem() { Content = saleDashboard},
                    new TabItem() { Content = report}
                    };
            tabs.ItemsSource = screens;
            Login lg = new Login();
            lg.ShowDialog();
            if (lg.DialogResult == false)
            {
                System.Windows.Application.Current.Shutdown();
            }
            this.Show();


            ribbon.SelectedTabChanged += Ribbon_SelectedTabChanged;
            
        }

        private void Ribbon_SelectedTabChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = ribbon.SelectedTabIndex;
            
            if (ribbon.SelectedTabIndex == 0)
            {
                evenHandler?.Invoke(ActionProduct.Reload, null);
            }
            else if(ribbon.SelectedTabIndex == 1)
            {
                evenHandler?.Invoke(ActionOrder.Reload, null);
            }
            else if(ribbon.SelectedTabIndex == 2)
            {
                if(ProfileState.profile.RoleId != (int)RoleId.SupperAdmin)
                {
                    ribbon.SelectedTabIndex = 0;
                    MessageBox.Show("Bạn ko có quyền truy cập vào đây!!");
                }
            }
        }

        void Simulator()
        {
            for (int i = 0; i < 500; i++)
            {
                Thread.Sleep(5);
            }
        }
    }
}





