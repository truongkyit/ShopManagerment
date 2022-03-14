using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAO
{
    class UserDAO
    {
        private MyShopEntities conn;

        public UserDAO()
        {
            conn = dbConnection.getInstance();
        }
        public User getByUserName(string username)
        {
            return conn.Users.Where(item => item.UserName.Equals(username)).FirstOrDefault();
        }
    }
}
