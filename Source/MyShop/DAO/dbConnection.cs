using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAO
{
    public static class dbConnection
    {
        private static MyShopEntities  _conn;

        public static MyShopEntities getInstance()
        {
            if(_conn == null)
            {
                _conn = new MyShopEntities();
            }
            return _conn;
        }

    }
}
