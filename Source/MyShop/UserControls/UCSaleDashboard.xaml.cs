using MyShop.Business;
using MyShop.Common;
using MyShop.Form;
using MyShop.State;
using MyShop.ValueObject.OrderProductVO;
using MyShop.ValueObject.OrderVO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for UCSaleDashboard.xaml
    /// </summary>
    public partial class UCSaleDashboard : UserControl
    {
        private OrderBusiness orderBus = new OrderBusiness();
        private OrderStatusBussiness orderStatusBus = new OrderStatusBussiness();

        bool isAction = false;
        bool isSelect = false;
        public UCSaleDashboard()
        {
            InitializeComponent();

            Loaded += UCSaleDashboard_Loaded;

            btnAddProduct.Click += BtnAddProduct_Click;
            btnDeleteProduct.Click += BtnDeleteProduct_Click;
            MainWindow.evenHandler += MainWindow_evenHandler;

            OrderHistoryLV.SelectionChanged += OrderHistoryLV_SelectionChanged;

            this.btnDone.Click += BtnDone_Click;
            this.btnCancel.Click += BtnCancel_Click;
            this.btnPrevius.Click += BtnPrevius_Click;
            this.btnNext.Click += BtnNext_Click;

         
            this.tbPhoneNumber.PreviewTextInput += TbPhoneNumber_PreviewTextInput;

            this.btnUnFilterAll.Click += BtnUnFilterAll_Click;
            this.tbFindByPrice.PreviewTextInput += TbFindByPrice_PreviewTextInput;

            this.btnFilterAll.Click += BtnFilterAll_Click;

        }

        private void ChangeListOrder()
        {
            var getPaginationOrder = orderBus.getAll(OrderState.pagination.skip, OrderState.pagination.limit, OrderState.filterOrder);
            ReFillList(getPaginationOrder.orders);
            OrderState.pagination.total = getPaginationOrder.count;
            CalPagination();
        }

        private void BtnFilterAll_Click(object sender, RoutedEventArgs e)
        {
            ChangeListOrder();
        }

        private void TbFindByPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void BtnUnFilterAll_Click(object sender, RoutedEventArgs e)
        {
            OrderState.filterOrder.Reset();
            ChangeListOrder();
        }

        private void TbPhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        string text = "";

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            OrderState.pagination.previus = true;
            OrderState.pagination.skip += OrderState.pagination.limit;
            ChangeListOrder();
        }

        private void BtnPrevius_Click(object sender, RoutedEventArgs e)
        {
            OrderState.pagination.next = true;
            OrderState.pagination.skip -= OrderState.pagination.limit;
            ChangeListOrder();
        }

        private void BtnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (OrderProductLV.SelectedItems.Count > 0)
            {
                var itemRm = OrderProductLV.SelectedItems[0] as OrderProductCreateVO;
                //rest of your logic
                orderAction.orderProductVOs.Remove(itemRm);
                orderAction.TotalAmount = orderAction.orderProductVOs.Select(item => item.TotalAmount).Sum();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            OrderState.Action = 1;
            ActionStatusOrder();
            orderAction.reset();
            OrderHistoryLV.SelectedIndex = -1;
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            string ErrMess = orderAction.Validate();
            if(ErrMess.Length > 0)
            {
                MessageBox.Show(ErrMess);
                return;
            }

            if (OrderState.Action == 2)
            {
                var p = orderBus.update(orderAction.Id, orderAction.toOrderUpdateVO());
            }
            else
            {
                var p = orderBus.InsertData(orderAction);
                OrderState.ordersState.Add(p);
            }
            OrderState.Action = 1;
            ActionStatusOrder();
            ChangeListOrder();
            orderAction.reset();
            OrderHistoryLV.SelectedIndex = -1;
        }


        private void OrderHistoryLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderHistoryLV.SelectedIndex != -1)
            {
                var order = OrderHistoryLV.SelectedItem as Order;
                if (isAction == false)
                {
                    orderAction.PassValue(order);
                    isSelect = true;
                }
            }
        }

        OrderCreateVO orderAction = new OrderCreateVO();
        private void UCSaleDashboard_Loaded(object sender, RoutedEventArgs e)
        {
            var getPaginationOrder = orderBus.getAll(OrderState.pagination.skip, OrderState.pagination.limit,text);
            ReFillList(getPaginationOrder.orders);
            OrderState.pagination.total = getPaginationOrder.count;
            OrderHistoryLV.ItemsSource = OrderState.ordersState;
            OrderDetail.DataContext = orderAction;
            OrderProductLV.ItemsSource = orderAction.orderProductVOs;
            paginationOrder.DataContext = OrderState.pagination;
            OrderState.ordersStatus = orderStatusBus.getAll();
            cbbStatusOrder.ItemsSource = OrderState.ordersStatus;
            cbbFilterStatus.ItemsSource = OrderState.ordersStatus;
            BoxFilter.DataContext = OrderState.filterOrder;
            CalPagination();

            ActionStatusOrder();
        }

 

        private void MainWindow_evenHandler(object sender, EventArgs e)
        {
            switch (sender)
            {
                case ActionOrder.Create:
                    OrderState.Action = 3;
                    ActionStatusOrder();
                    break;
                case ActionOrder.Update:
                    if (isSelect == false)
                    {
                        MessageBox.Show("Chọn đơn hàng để chỉnh sửa nha bạn!");
                        return;
                    }
                    OrderState.Action = 2;
                    ActionStatusOrder();
                    break;
                case ActionOrder.Delete:
                    if (isAction == true)
                    {
                        MessageBox.Show("Bạn đang có một thao tác khác!!");
                        return;
                    }
                    OrderState.pagination.skip = 0;
                    if (OrderHistoryLV.SelectedIndex < 0)
                    {
                        MessageBox.Show("Chọn đơn hàng để xóa bạn ơi!!");
                        return;
                    }
                    orderBus.deleteById(orderAction.Id);
                    ChangeListOrder();
                    orderAction.reset();
                    MessageBox.Show("Xóa đơn hàng thành công!!");
                    break;
                case ActionOrder.Reload:
                    OrderState.pagination.skip = 0;
                    break;
                default: break;
            }
        }

        public static void CalPagination()
        {
            if (OrderState.pagination.skip == 0)
            {
                OrderState.pagination.previus = false;
            }
            if (OrderState.pagination.skip + OrderState.pagination.limit >= OrderState.pagination.total)
            {
                OrderState.pagination.next = false;
            }
            else OrderState.pagination.next = true;
        }

        private void ReFillList(List<Order> ord)
        {
            OrderState.ordersState.Clear();
            for (int i = 0; i < ord.Count; i++)
            {
                OrderState.ordersState.Add(ord[i]);
            }
        }

       

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            FunctionHelper.Pagination(ActionPagination.Load, ActionProduct.Reload);
            AllProductForm frm = new AllProductForm(orderAction);
            frm.ShowDialog();
            if (frm.DialogResult == true)
            {
                if (OrderState.Action == 3 || OrderState.Action == 2)
                {
                    OrderProductCreateVO ordP = new OrderProductCreateVO(frm.productSelected);
                    var ordPFind = orderAction.orderProductVOs.Where(item => item.ProductID == ordP.ProductID).FirstOrDefault();
                    if (ordPFind != null)
                    {
                        frm.quantity += ordPFind.Quantity;
                        orderAction.orderProductVOs.Remove(ordPFind);
                    }
                    ordP.Quantity = frm.quantity;
                    ordP.TotalAmount = ordP.Quantity * ordP.Amount;
                    orderAction.orderProductVOs.Add(ordP);
                    orderAction.TotalAmount = orderAction.orderProductVOs.Select(item => item.TotalAmount).Sum();
                }
            }
        }

        private void ActionStatusOrder()
        {

            if (OrderState.Action == 1)
            {
                this.Mode.Text = "Xem đơn hàng";
                EnableForm(0);
            }
            else if (OrderState.Action == 2)
            {
                if (isAction == true)
                {
                    var reuslt = MessageBox.Show("Bạn đang thực hiện một hành động khác, bạn muốn hủy bỏ ngay?", "Thông báo", MessageBoxButton.YesNo);
                    if (reuslt == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                if (OrderHistoryLV.SelectedIndex == -1)
                {
                    MessageBox.Show("Chọn sản phẩm để chỉnh sửa!");
                    return;
                }
                var order = OrderHistoryLV.SelectedItem as Order;
                this.Mode.Text = "Cập nhật đơn hàng";
                EnableForm(1);
                orderAction.PassValue(order);
            }
            else
            {
                if (isAction == true)
                {
                    var reuslt = MessageBox.Show("Bạn đang thực hiện một hành động khác, bạn muốn hủy bỏ ngay?", "Thông báo", MessageBoxButton.YesNo);
                    if (reuslt == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                this.Mode.Text = "Tạo đơn hàng";
                if (isAction == true)
                {
                    MessageBox.Show("Bạn đang tạo đơn hàng rồi!");
                    return;
                }
                EnableForm(1);
                OrderCreateVO orderSelected = new OrderCreateVO();
                orderAction.PassValue(orderSelected);
            }
        }

        private void EnableForm(int index)
        {
            if (index == 1)
            {
                isAction = true;
                this.btnAddProduct.IsEnabled = true;
                this.btnCancel.IsEnabled = true;
                this.btnDone.IsEnabled = true;
                this.btnDeleteProduct.IsEnabled = true;
                this.tbAddress.IsEnabled = true;
                this.tbName.IsEnabled = true;
                this.tbPhoneNumber.IsEnabled = true;
                this.cbbStatusOrder.IsEnabled = true;
            }
            else
            {
                isAction = false;
                this.btnAddProduct.IsEnabled = false;
                this.btnCancel.IsEnabled = false;
                this.btnDone.IsEnabled = false;
                this.btnDeleteProduct.IsEnabled = false;
                this.tbAddress.IsEnabled = false;
                this.tbName.IsEnabled = false;
                this.tbPhoneNumber.IsEnabled = false;
                this.cbbStatusOrder.IsEnabled = false;
            }
        }
    }
}
