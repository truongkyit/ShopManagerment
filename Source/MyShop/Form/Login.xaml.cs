using MyShop.Business;
using MyShop.Common;
using MyShop.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Fluent.IRibbonWindow
    {
        private UserBusiness usBus = new UserBusiness();
        public Login()
        {
            InitializeComponent();

            this.btnExit.Click += BtnExit_Click;
            this.btnLogin.Click += BtnLogin_Click;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = this.username.Text;
            string password = this.password.Password.ToString();
            var us = usBus.getUser(username);
            if(us == null)
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                return;
            }
            if(HashPassword.ValidatePassword(password,us.Password) == false)
            {

                MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                return;
            }

            ProfileState.profile.Name = us.Name;
            ProfileState.profile.RoleId = us.RoleID;
            DialogResult = true;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }


}
