﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAO
{
    class OrderProductDAO
    {
        private MyShopEntities conn;

        public OrderProductDAO()
        {
            conn = dbConnection.getInstance();
        }

    }
}
